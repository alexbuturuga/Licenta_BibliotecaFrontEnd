using BibleotecaInteligenta.DTOs;
using BibleotecaInteligenta.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibleotecaInteligenta
{
    public partial class PrezentareCarte : Form
    {
        public int IdCarte = 0;
        public int displayedReviewId = 0;
        public int IdUser = 0;
        private BookService _bookService;
        private BookDTO book;
        private ReviewService _reviewService;
        private BorrowedBookService _borrowedBookService;
        private List<ReviewDTO> _reviews = new List<ReviewDTO>();
        public PrezentareCarte(int idCarte, BookService bookService, ReviewService reviewService, int idUser, BorrowedBookService borrowedBookService)
        {
            IdCarte = idCarte;
            _bookService = bookService;
            _reviewService = reviewService;
            IdUser = idUser;
            _borrowedBookService = borrowedBookService;
            InitializeComponent();

        }

        public async void LoadBook()
        {
            try
            {
                book = await _bookService.GetBook(IdCarte);
                label1.Text = "Titlu: " + book.Title;
                label2.Text = "Autor: " + book.Author.Name + " " + book.Author.Surname;
                label3.Text = "Numar Pagini: " + book.PageNumber;
                label5.Text = "Data Aparitiei: " + book.AppearDate.ToString("yyyy/MM/dd");
                label6.Text = "Editura: " + book.Publisher;
                label7.Text = "Limba: " + book.Language.Name;
                richTextBox2.Text = book.Description;
                pictureBox2.Image = Image.FromFile(@"Resources/Coperti/" + book.Photo);
            }
            catch
            {
                MessageBox.Show("Cartea pe care doresti sa o accesezi, nu este disponibila sau nu mai exista!");
                this.Hide();
            }
            
        }

        public void LoadReviews()
        {

            if (_reviews != null)
            {
                panel7.Controls.Clear();
                int grade = _reviews[displayedReviewId].Grade;
                for (int i = 0; i < grade; i++)
                {
                    PictureBox star = new PictureBox();
                    star.Image = Properties.Resources.star;
                    star.SizeMode = PictureBoxSizeMode.StretchImage;
                    star.Width = 15;
                    star.Height = 15;
                    star.Location = new Point(i * star.Width, 0);

                    panel7.Controls.Add(star);
                }
                label10.Text = "Utilizator:  " + _reviews[displayedReviewId].User.UserName;
                richTextBox1.Text = _reviews[displayedReviewId].Description;
            }
            else
            {
                label10.Text = "Nu exista inca nici un review pentru aceasta carte!";
                richTextBox1.Text = "";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (_reviews != null)
            {
                if (displayedReviewId < _reviews.Count - 1)
                {
                    displayedReviewId++;
                    LoadReviews();
                }
                else
                {
                    MessageBox.Show("Nu exista alte recenzii!");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (_reviews != null)
            {
                if (displayedReviewId > 0)
                {
                    displayedReviewId--;
                    LoadReviews();
                }
                else
                {
                    MessageBox.Show("Nu exista alte recenzii!");
                }
            }
        }

        private async void PrezentareCarte_Load(object sender, EventArgs e)
        {
            _reviews = await _reviewService.GetReviewsByBookId(IdCarte);

            LoadBook();
            LoadReviews();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            LasaRecenzie f = new LasaRecenzie(_reviewService, IdCarte, IdUser);
            f.Show();
        }

        private async void button3_Click_1(object sender, EventArgs e)
        {
            //try
            //{
            await _borrowedBookService.CreateBorrowedBook(new BorrowedBookDTO
            {
                BookId = IdCarte,
                UserId = IdUser,
                Confirmed = false,
                BorrowStartDate = DateTime.Now,
                BorrowDeathLine = DateTime.Now.AddDays(30)
            });
            MessageBox.Show("Cererea de imprumut a fost creata cu succes!! Vei primi un raspuns in cel mai scurt timp. Poti verifica" +
                "statusul cerererii pe profilul tau.");
            //}
            //catch
            //{

            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private bool isFormDragged = false;
        private Point formStartPosition;
        private void PrezentareCarte_MouseMove(object sender, MouseEventArgs e)
        {
            if (isFormDragged)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - formStartPosition.X, currentScreenPos.Y - formStartPosition.Y);
            }
        }

        private void PrezentareCarte_MouseUp(object sender, MouseEventArgs e)
        {
            isFormDragged = false;
        }

        private void PrezentareCarte_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isFormDragged = true;
                formStartPosition = new Point(e.X, e.Y);
            }
        }
    }
}