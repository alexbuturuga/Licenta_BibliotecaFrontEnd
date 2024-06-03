using BibleotecaInteligenta.DTOs;
using BibleotecaInteligenta.Services;

namespace BibleotecaInteligenta
{
    public partial class Profil : Form
    {
        private UserService _userService;
        private BorrowedBookService _borrowedBookService;
        public int UserIdInt;
        public string UserId;
        public int CartiCitite = 0;

        public List<BorrowedBookDTO> _borrowedBookDTOs = new List<BorrowedBookDTO>();
        public Profil(UserService userService, BorrowedBookService borrowedBookService, int userIdInt, string userId)
        {
            InitializeComponent();
            _userService = userService;
            _borrowedBookService = borrowedBookService;
            UserIdInt = userIdInt;
            UserId = userId;
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        public async void populeazaDataGridView()
        {
            _borrowedBookDTOs = await _borrowedBookService.GetBorrowedBooksByUserId(UserIdInt);
            if (_borrowedBookDTOs != null)
            {
                foreach (var book in _borrowedBookDTOs)
                {
                    dataGridView1.Rows.Add(
                    book.Book.Title,
                    $"{book.Book.Author.Name} {book.Book.Author.Surname}",
                    book.Confirmed,
                    book.BorrowStartDate.ToString("yyyy-MM-dd"),
                    book.BorrowEndDate?.ToString("yyyy-MM-dd") ?? "");
                }
                CartiCitite = _borrowedBookDTOs.Count;
            }
            else
            {
                MessageBox.Show("Nu exista carti inchiriate!");
            }
            label1.Text = "Carti citite: " + CartiCitite;
        }

        public async Task<string> Nume()
        {
            var user = await _userService.GetUser(UserId);
            return user.UserName;
        }
        private async void Profil_Load(object sender, EventArgs e)
        {
            label10.Text = "Nume: " + await Nume();
            populeazaDataGridView();
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            try
            {
                await _userService.EditUser(new UserEditDTO
                {
                    Id = UserIdInt,
                    UserName = textBox1.Text
                });
                MessageBox.Show("Numele a fost schimbat cu succes!");
                label10.Text = "Nume: " + textBox1.Text;
                panel5.Visible = false;
            }
            catch
            {
                MessageBox.Show("A avut loc o eroare neasteptata!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (panel5.Visible == false)
            {
                panel5.Visible = true;
            }
            else
            {
                panel5.Visible = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel5.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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
    }
}