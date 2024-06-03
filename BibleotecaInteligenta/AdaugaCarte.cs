using BibleotecaInteligenta.DTOs;
using BibleotecaInteligenta.Services;
using System.IO;

namespace BibleotecaInteligenta
{
    public partial class AdaugaCarte : Form
    {
        private BookService _bookService;
        private LanguageService _languageService;
        private AuthorService _authorService;

        public bool Admin = false;
        public long LanguageId;
        public long AuthorId;
        private string _selectedImagePath = null;
        public bool EditMode = false;

        private bool _isDragging = false;
        private Point _startPoint = new Point(0, 0);

        private bool _isDragging2 = false;
        private Point _startPoint2 = new Point(0, 0);

        private List<BookDTO> _books = new List<BookDTO>();
        public BookDTO _selectedBook = new BookDTO();
        public AdaugaCarte(BookService bookService, LanguageService languageService, AuthorService authorService, bool admin)
        {
            _bookService = bookService;
            _languageService = languageService;
            _authorService = authorService;
            Admin = admin;
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (EditMode == false)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                richTextBox1.Clear();
                comboBox1.Text = "";
                comboBox2.Text = "";
                comboBox3.Text = "";
                label15.Hide();
            }
            else
            {
                EditMode = false;
                button5.Text = "Sterge";
                button6.Text = "Adauga";
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                richTextBox1.Clear();
                comboBox1.Text = "";
                comboBox2.Text = "";
                comboBox3.Text = "";
                button13.Hide();
                label15.Hide();
                pictureBox2.Image = Properties.Resources.addphoto;
            }

        }

        private async void button6_Click(object sender, EventArgs e)
        {
            if (EditMode == false)
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text) || textBox1.Text.Length <= 4)
                {
                    MessageBox.Show("Titlul trebuie să aibă mai mult de 4 caractere și să nu fie gol.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(textBox2.Text) || !int.TryParse(textBox2.Text, out _))
                {
                    MessageBox.Show("Numărul de pagini trebuie să fie un număr valid.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(textBox3.Text) || textBox3.Text.Length <= 2)
                {
                    MessageBox.Show("Textul din textBox3 trebuie să aibă mai mult de 2 caractere și să nu fie gol.");
                    return;
                }

                if (dateTimePicker1.Value >= DateTime.Now)
                {
                    MessageBox.Show("Data de apariție trebuie să fie în trecut.");
                    return;
                }

                if (comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("Te rog să selectezi un autor.");
                    return;
                }

                if (comboBox2.SelectedItem == null)
                {
                    MessageBox.Show("Te rog să selectezi o limbă.");
                    return;
                }

                if (_selectedImagePath == null)
                {
                    MessageBox.Show("Te rog să selectezi o imagine pentru copertă.");
                    return;
                }

                var sourceFile = openFileDialog1.FileName;
                System.IO.File.Copy(sourceFile, _selectedImagePath, true);

                await _bookService.CreateBook(new CreateBookDTO
                {
                    Title = textBox1.Text,
                    PageNumber = int.Parse(textBox2.Text),
                    Avalabile = true,
                    AppearDate = dateTimePicker1.Value,
                    Description = richTextBox1.Text,
                    Publisher = textBox3.Text,
                    Photo = _selectedImagePath != null ? System.IO.Path.GetFileName(_selectedImagePath) : "",
                    AuthorId = AuthorId,
                    LanguageId = LanguageId,
                });

                comboBoxBooks();
                MessageBox.Show("Cartea a fost adăugată cu succes!");

                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                richTextBox1.Clear();
                pictureBox2.Image = Properties.Resources.addphoto;
            }
            else
            {
                var sourceFile = openFileDialog1.FileName;
                System.IO.File.Copy(sourceFile, _selectedImagePath, true);

                await _bookService.EditBook(new CreateBookDTO
                {
                    Id = _selectedBook.Id,
                    Title = textBox1.Text,
                    PageNumber = int.Parse(textBox2.Text),
                    Avalabile = _selectedBook.Avalabile,
                    AppearDate = dateTimePicker1.Value,
                    Description = richTextBox1.Text,
                    Publisher = textBox3.Text,
                    Photo = _selectedImagePath != null ? System.IO.Path.GetFileName(_selectedImagePath) : _selectedBook.Photo,
                    AuthorId = AuthorId,
                    LanguageId = LanguageId,
                });
                comboBoxBooks();
                MessageBox.Show("Cartea a fost actualizată cu succes!");
            }

        }



        public async void comboBoxAuthors()
        {
            try
            {
                var authors = await _authorService.GetAuthors();
                comboBox1.Items.Clear();

                foreach (var author in authors)
                {
                    comboBox1.Items.Add(new ComboBoxItem(author.Id, $"{author.Name} {author.Surname}"));
                }
            }
            catch
            {
                MessageBox.Show("Autorii nu au putut fii incarcati!");
            }
        }

        public async void comboBoxLanguage()
        {
            try
            {
                var languages = await _languageService.GetLanguages();
                comboBox2.Items.Clear();

                foreach (var language in languages)
                {
                    comboBox2.Items.Add(new ComboBoxItem(language.Id, language.Name));
                }
            }
            catch
            {
                MessageBox.Show("Limbile nu au putut fii incarcate!");
            }

        }

        public async void comboBoxBooks()
        {
            try
            {
                _books = await _bookService.GetBooks();
                comboBox3.Items.Clear();

                foreach (var book in _books)
                {
                    comboBox3.Items.Add(new ComboBoxItem(book.Id, book.Title));
                }
            }
            catch
            {
                MessageBox.Show("Cartile nu au putut fii incarcate!");
            }

        }
        private void AdaugaCarte_Load(object sender, EventArgs e)
        {
            comboBoxAuthors();
            comboBoxLanguage();
            comboBoxBooks();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var sourceFile = openFileDialog1.FileName;
                var fileName = System.IO.Path.GetFileName(sourceFile);

                var resourcesFolder = System.IO.Path.Combine(Application.StartupPath, "Resources", "Coperti");

                if (!System.IO.Directory.Exists(resourcesFolder))
                {
                    System.IO.Directory.CreateDirectory(resourcesFolder);
                }

                var destFile = System.IO.Path.Combine(resourcesFolder, fileName);

                _selectedImagePath = destFile; // Stocăm calea imaginii selectate

                pictureBox2.Image = Image.FromFile(sourceFile); // Afișăm imaginea selectată

            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is ComboBoxItem selectedAuthor)
            {
                AuthorId = selectedAuthor.Id;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem is ComboBoxItem selectedLanguage)
            {
                LanguageId = selectedLanguage.Id;
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void panel5_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                _startPoint = new Point(e.X, e.Y);
                panel5.Capture = true;
            }
        }

        private void panel5_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                panel5.SuspendLayout();

                var newLocation = panel5.Location;
                newLocation.X += e.X - _startPoint.X;
                newLocation.Y += e.Y - _startPoint.Y;

                panel5.Location = newLocation;

                panel5.ResumeLayout();
            }
        }

        private void panel5_MouseUp(object sender, MouseEventArgs e)
        {
            _isDragging = false;
            panel5.Capture = false;
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Numele autorului este obligatoriu.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Prenumele autorului este obligatoriu.");
                return;
            }

            if (dateTimePicker2.Value >= DateTime.Now)
            {
                MessageBox.Show("Data de naștere trebuie să fie în trecut.");
                return;
            }

            try
            {
                var newAuthor = new CreateAuthorDTO
                {
                    Name = textBox4.Text,
                    Surname = textBox5.Text,
                    BirthDate = dateTimePicker2.Value,
                };

                var createdAuthor = await _authorService.CreateAuthor(newAuthor);

                MessageBox.Show($"Autorul {createdAuthor.Name} {createdAuthor.Surname} a fost creat cu succes.");
                comboBoxAuthors();

                panel5.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A apărut o eroare la crearea autorului: {ex.Message}");
            }
        }

        private void button3_Click(object sender, EventArgs e)
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

        private void button7_Click(object sender, EventArgs e)
        {
            panel5.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            panel10.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox7.Text = "";
        }

        private async void button10_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox7.Text))
            {
                MessageBox.Show("Numele limbii este obligatoriu.");
                return;
            }

            try
            {
                var newLanguage = new CreateLanguageDTO
                {
                    Name = textBox7.Text,
                };

                var createdLanguage = await _languageService.CreateLanguage(newLanguage);

                MessageBox.Show($"Limba '{createdLanguage.Name}' a fost creată cu succes.");

                comboBoxLanguage();

                panel10.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A apărut o eroare la crearea limbii: {ex.Message}");
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            if (panel10.Visible == false)
            {
                panel10.Visible = true;
            }
            else
            {
                panel10.Visible = false;
            }
        }

        private void panel10_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging2 = true;
                _startPoint2 = new Point(e.X, e.Y);
                panel10.Capture = true;
            }
        }

        private void panel10_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging2)
            {
                panel10.SuspendLayout();

                var newLocation = panel10.Location;
                newLocation.X += e.X - _startPoint2.X;
                newLocation.Y += e.Y - _startPoint2.Y;

                panel10.Location = newLocation;

                panel10.ResumeLayout();
            }
        }

        private void panel10_MouseUp(object sender, MouseEventArgs e)
        {
            _isDragging2 = false;
            panel10.Capture = false;
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

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem is ComboBoxItem selectedBook)
            {

                button13.Visible = true;
                button6.Text = "Editeaza";
                button5.Text = "Inapoi";
                EditMode = true;
                var book = _books.FirstOrDefault(x => x.Id == selectedBook.Id);
                _selectedBook = book;
                label15.Show();
                label15.Text = "Id Carte: " + book.Id.ToString();
                label15.Tag = book.Id.ToString();
                comboBox2.Text = book.Language.Name;
                comboBox1.Text = book.Author.Name + " " + book.Author.Surname;
                richTextBox1.Text = book.Description;
                textBox3.Text = book.Publisher;
                dateTimePicker1.Value = book.AppearDate;
                textBox2.Text = book.PageNumber.ToString();
                textBox1.Text = book.Title;
                string imagesFolder = Path.Combine(Application.StartupPath, "Resources", "Coperti");
                string imagePath = Path.Combine(imagesFolder, book.Photo);
                if (File.Exists(imagePath))
                {
                    pictureBox2.Image = Image.FromFile(imagePath);
                }
                else
                {
                    MessageBox.Show("Imaginea nu a fost găsită: " + imagePath);
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                _bookService.DeleteBook(_selectedBook.Id);
                MessageBox.Show("Sters cu succes!");
                EditMode = false;
                button5.Text = "Sterge";
                button6.Text = "Adauga";
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                richTextBox1.Clear();
                comboBox1.Text = "";
                comboBox2.Text = "";
                comboBox3.Text = "";
                button13.Hide();
                label15.Hide();
                pictureBox2.Image = Properties.Resources.addphoto;
                comboBoxBooks();
            }
            catch
            {
                MessageBox.Show("A avut loc o eroare la stergere!");
            }
        }
    }
}