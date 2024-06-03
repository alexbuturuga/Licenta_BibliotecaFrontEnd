using BibleotecaInteligenta.DTOs;
using BibleotecaInteligenta.Services;
using System.IdentityModel.Tokens.Jwt;

namespace BibleotecaInteligenta
{
    public partial class PaginaPrincipala : Form
    {
        private AuthService _authService;
        private UserService _userService;
        private HttpClient _httpClient;
        private BookService _bookService;
        private ReviewService _reviewService;
        private BorrowedBookService _borrowedBookService;
        private LanguageService _languageService;
        private AuthorService _authorService;

        public bool Admin = false;
        public string UserId;
        public int UserIdInt;
        private List<BookDTO> _books = new List<BookDTO>();
        private List<PopularBookDTO> _popularBooks = new List<PopularBookDTO>();

        private int booksNumber = 0;
        private int booksCurrentNumber = 1;

        private int popularBooksNumber = 0;
        private int popularBooksCurrentNumber = 1;
        private System.Windows.Forms.Timer hideTimer;

        public PaginaPrincipala(AuthService authService)
        {
            _authService = authService;
            _httpClient = new HttpClient();
            _bookService = new BookService(_httpClient, authService);
            _userService = new UserService(_httpClient, _authService);
            _reviewService = new ReviewService(_httpClient, authService);
            _borrowedBookService = new BorrowedBookService(_httpClient, authService);
            _authorService = new AuthorService(_httpClient, authService);
            _languageService = new LanguageService(_httpClient, authService);

            hideTimer = new System.Windows.Forms.Timer();
            hideTimer.Interval = 100; // 100 ms delay
            hideTimer.Tick += HideTimer_Tick;
            InitializeComponent();
        }

        private void HideTimer_Tick(object sender, EventArgs e)
        {
            hideTimer.Stop();
            if (!textBox1.Bounds.Contains(PointToClient(Cursor.Position)) && !listBox1.Bounds.Contains(PointToClient(Cursor.Position)))
            {
                listBox1.Visible = false;
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox1.Text.ToLower();
            var filteredBooks = _books.Where(book => book.Title.ToLower().Contains(searchText)).ToList();

            listBox1.Items.Clear();
            foreach (var book in filteredBooks)
            {
                listBox1.Items.Add(book.Title);
            }

            listBox1.Visible = filteredBooks.Count > 0;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {

                string selectedTitle = listBox1.SelectedItem.ToString();
                textBox1.Text = selectedTitle;
            }
        }

        private void Form_MouseClick(object sender, MouseEventArgs e)
        {
            if (!textBox1.Bounds.Contains(e.Location) && !listBox1.Bounds.Contains(e.Location))
            {
                hideTimer.Start();
            }
        }

        private void TextBox1_Leave(object sender, EventArgs e)
        {
            if (!listBox1.Bounds.Contains(PointToClient(Cursor.Position)))
            {
                hideTimer.Start();
            }
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            string selectedTitle = textBox1.Text;
            var selectedBook = _books.FirstOrDefault(book => book.Title == selectedTitle);
            if (selectedBook != null)
            {
                PrezentareCarte prezentareCarte = new PrezentareCarte(Convert.ToInt32(selectedBook.Id), _bookService, _reviewService, UserIdInt, _borrowedBookService);
                prezentareCarte.Show();
            }
            else
            {
                MessageBox.Show("Cartea cautata nu a fost gasita!");
            }
        }

        private void PaginaPrincipala_Load(object sender, EventArgs e)
        {
            if (_authService != null)
            {
                FirstLogin();
                IncarcareCarti();
                PopularBooks();
                IsAdmin();
                timer1.Start();
            }
            else
            {
                Console.WriteLine("AuthService nu este instanțiat.");
            }
        }
        public async void FirstLogin()
        {
            if (_authService != null)
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                var decodedToken = jwtHandler.ReadJwtToken(_authService.GetToken());
                var claims = decodedToken.Claims;
                var userIdClaim = claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim == null)
                {
                    MessageBox.Show("Eroare la autentificare, utilizatorul nu a fost gasit!");
                    return;
                }
                else
                {
                    UserId = userIdClaim.Value;
                }

                try
                {
                    var response = await _userService.GetUser(UserId);
                    Admin = response.Admin;
                    UserIdInt = Convert.ToInt32(response.Id);
                }
                catch
                {
                    try
                    {
                        var response = await _userService.CreateUser(new DTOs.UserDTO
                        {
                            Admin = false,
                            UserAccountID = userIdClaim.Value,
                            UserName = "User"
                        });
                        Admin = response.Admin;
                        UserIdInt = Convert.ToInt32(response.Id);
                        MessageBox.Show("Bun venit!");
                    }
                    catch
                    {
                        MessageBox.Show("A avut loc o eroare neasteptata!");
                    }
                }
                if (Admin == true)
                {
                    MessageBox.Show("Cont de admin.");
                    IsAdmin();
                }
            }
        }

        public void IsAdmin()
        {
            if (Admin == true)
            {
                button4.Show();
                button10.Show();
                button11.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            CereriUtilizatori f = new CereriUtilizatori(_borrowedBookService);
            f.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
        public void changeBooks()
        {
            pictureBox2.ImageLocation = @"Resources/Coperti/" + _books[booksCurrentNumber - 4].Photo;
            pictureBox2.Tag = _books[booksCurrentNumber - 4].Id;
            pictureBox3.ImageLocation = @"Resources/Coperti/" + _books[booksCurrentNumber - 3].Photo;
            pictureBox3.Tag = _books[booksCurrentNumber - 3].Id;
            pictureBox4.ImageLocation = @"Resources/Coperti/" + _books[booksCurrentNumber - 2].Photo;
            pictureBox4.Tag = _books[booksCurrentNumber - 2].Id;
            pictureBox5.ImageLocation = @"Resources/Coperti/" + _books[booksCurrentNumber - 1].Photo;
            pictureBox5.Tag = _books[booksCurrentNumber - 1].Id;
            label6.Text = $"{booksCurrentNumber}/{_books.Count()}";
        }

        public void changePopularBooks()
        {
            pictureBox9.ImageLocation = @"Resources/Coperti/" + _popularBooks[popularBooksCurrentNumber - 4].Photo;
            pictureBox9.Tag = _popularBooks[popularBooksCurrentNumber - 4].Id;
            pictureBox8.ImageLocation = @"Resources/Coperti/" + _popularBooks[popularBooksCurrentNumber - 3].Photo;
            pictureBox8.Tag = _popularBooks[popularBooksCurrentNumber - 3].Id;
            pictureBox7.ImageLocation = @"Resources/Coperti/" + _popularBooks[popularBooksCurrentNumber - 2].Photo;
            pictureBox7.Tag = _popularBooks[popularBooksCurrentNumber - 2].Id;
            pictureBox6.ImageLocation = @"Resources/Coperti/" + _popularBooks[popularBooksCurrentNumber - 1].Photo;
            pictureBox6.Tag = _popularBooks[popularBooksCurrentNumber - 1].Id;
        }
        public async void IncarcareCarti()
        {
            try
            {
                _books = await _bookService.GetBooks();
                booksNumber = _books.Count();
                if (booksNumber > 4)
                {
                    if(booksCurrentNumber < 4)
                    {
                        booksCurrentNumber = 4;
                    }
                    if (booksNumber < booksCurrentNumber)
                        booksCurrentNumber = booksNumber;
                    changeBooks();
                }
                else
                {
                    if (booksCurrentNumber == 1)
                    {

                    }
                    if (booksCurrentNumber == 2)
                    {

                    }
                    if (booksCurrentNumber == 3)
                    {

                    }
                }
            }
            catch
            {
                MessageBox.Show("Eroare la incarcarea cartilor!");
            }
        }

        public async void PopularBooks()
        {
            try
            {
                _popularBooks = await _bookService.GetPopularBooks();
                popularBooksNumber = _popularBooks.Count();
                if (popularBooksNumber > 4)
                {
                    popularBooksCurrentNumber = 4;
                    changePopularBooks();
                }
                else
                {
                    if (popularBooksCurrentNumber == 1)
                    {

                    }
                    if (popularBooksCurrentNumber == 2)
                    {

                    }
                    if (popularBooksCurrentNumber == 3)
                    {

                    }
                }
            }
            catch
            {
                MessageBox.Show("Eroare");
            }
        }
        private async void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (booksCurrentNumber < _books.Count())
            {
                booksCurrentNumber++;
                changeBooks();
            }
            else
            {
                MessageBox.Show("Nu exista alte carti!");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (booksCurrentNumber > 4)
            {
                booksCurrentNumber--;
                changeBooks();
            }
            else
            {
                MessageBox.Show("Nu exista alte carti!");
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                PrezentareCarte prezentareCarte = new PrezentareCarte(Convert.ToInt32(pictureBox2.Tag.ToString()), _bookService, _reviewService, UserIdInt, _borrowedBookService);
                prezentareCarte.Show();
            }
            catch
            {

            }

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            try
            {
                PrezentareCarte prezentareCarte = new PrezentareCarte(Convert.ToInt32(pictureBox3.Tag.ToString()), _bookService, _reviewService, UserIdInt, _borrowedBookService);
                prezentareCarte.Show();
            }
            catch
            {

            }

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            try
            {
                PrezentareCarte prezentareCarte = new PrezentareCarte(Convert.ToInt32(pictureBox4.Tag.ToString()), _bookService, _reviewService, UserIdInt, _borrowedBookService);
                prezentareCarte.Show();
            }
            catch
            {

            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            try
            {
                PrezentareCarte prezentareCarte = new PrezentareCarte(Convert.ToInt32(pictureBox5.Tag.ToString()), _bookService, _reviewService, UserIdInt, _borrowedBookService);
                prezentareCarte.Show();
            }
            catch
            {

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

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            try
            {
                PrezentareCarte prezentareCarte = new PrezentareCarte(Convert.ToInt32(pictureBox9.Tag.ToString()), _bookService, _reviewService, UserIdInt, _borrowedBookService);
                prezentareCarte.Show();
            }
            catch
            {

            }
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            try
            {
                PrezentareCarte prezentareCarte = new PrezentareCarte(Convert.ToInt32(pictureBox8.Tag.ToString()), _bookService, _reviewService, UserIdInt, _borrowedBookService);
                prezentareCarte.Show();
            }
            catch
            {

            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            try
            {
                PrezentareCarte prezentareCarte = new PrezentareCarte(Convert.ToInt32(pictureBox7.Tag.ToString()), _bookService, _reviewService, UserIdInt, _borrowedBookService);
                prezentareCarte.Show();
            }
            catch
            {

            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            try
            {
                PrezentareCarte prezentareCarte = new PrezentareCarte(Convert.ToInt32(pictureBox6.Tag.ToString()), _bookService, _reviewService, UserIdInt, _borrowedBookService);
                prezentareCarte.Show();
            }
            catch
            {

            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Pentru informatii suplimentare ne puteti gasi la Biblioteca@yahoo.com");
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Profil p = new Profil(_userService, _borrowedBookService, UserIdInt, UserId);
            p.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ToateRecenziile f = new ToateRecenziile(_reviewService);
            f.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            AdaugaCarte f = new AdaugaCarte(_bookService, _languageService, _authorService, Admin);
            f.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            IncarcareCarti();
            PopularBooks();
            IsAdmin();
        }
    }
}