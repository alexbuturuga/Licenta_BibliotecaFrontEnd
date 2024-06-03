using BibleotecaInteligenta.DTOs;
using BibleotecaInteligenta.Services;
using System.Data;

namespace BibleotecaInteligenta
{
    public partial class CereriUtilizatori : Form
    {
        private BorrowedBookService _borrowedBookService;
        public int CartiCitite = 0;

        public List<BorrowedBookDTO> _borrowedBookDTOs = new List<BorrowedBookDTO>();
        public List<BorrowedBookDTO> _books = new List<BorrowedBookDTO>();

        public CereriUtilizatori(BorrowedBookService borrowedBookService)
        {
            _borrowedBookService = borrowedBookService;
            InitializeComponent();
        }

        public async void GetData()
        {
            dataGridView1.Rows.Clear();
            _books = await _borrowedBookService.GetBorrowedBooks();
            UploadData();

        }

        public void UploadData()
        {
            _borrowedBookDTOs = _books.Where(x => x.Confirmed == false).ToList();
            if (_borrowedBookDTOs.Count == 0)
            {
                label1.Visible = true;
            }
            else
            {
                label1.Visible = false;
            }
            foreach (var book in _borrowedBookDTOs)
            {
                dataGridView1.Rows.Add(
                    book.Id,
                    book.User.UserName,
                    book.Book.Title,
                    book.Confirmed,
                    "Aproba",
                    "Refuza"
                    );
            }
        }

        private void CereriUtilizatori_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 4)
                {
                    try
                    {
                        long bookIdCell = Convert.ToInt64(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                        BorrowedBookDTO book = _borrowedBookDTOs.FirstOrDefault(x => x.Id == bookIdCell);
                        MessageBox.Show(bookIdCell.ToString());
                        if (book != null)
                        {
                            book.Confirmed = true;
                            await _borrowedBookService.ConfirmBorrowedBook(book.Id);
                            GetData();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("A avut loc o eroare!");
                        GetData();
                        return;
                    }
                }
                if (e.ColumnIndex == 5)
                {
                    try
                    {
                        long bookIdCell = Convert.ToInt64(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                        await _borrowedBookService.DeleteBorrowedBook(bookIdCell);
                        GetData();
                    }
                    catch
                    {
                        MessageBox.Show("A avut loc o eroare!");
                        GetData();
                        return;
                    }
                }
            }
        }

        private bool isFormDragged = false;
        private Point formStartPosition;
        private void PaginaPrincipala_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isFormDragged = true;
                formStartPosition = new Point(e.X, e.Y);
            }
        }

        private void PaginaPrincipala_MouseMove(object sender, MouseEventArgs e)
        {
            if (isFormDragged)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - formStartPosition.X, currentScreenPos.Y - formStartPosition.Y);
            }
        }

        private void PaginaPrincipala_MouseUp(object sender, MouseEventArgs e)
        {
            isFormDragged = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}