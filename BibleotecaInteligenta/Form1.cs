using BibleotecaInteligenta.DTOs;
using BibleotecaInteligenta.Services;
using Data.WeatherForecast;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Net.Http;
using System.Net.Http.Json;



namespace BibleotecaInteligenta
{
    public partial class Form1 : Form
    {
        private readonly HttpClient _httpClient;
        private bool isAuthenticated = false;
        private AuthService _authService;

        private bool isFormDragged = false;
        private Point formStartPosition = new Point(0, 0);
        public Form1()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            _authService = new AuthService();
        }

        private async void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        public void NavigateToMainPage()
        {
            MessageBox.Show("Merge!");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            int centerX = (this.ClientSize.Width - panel5.Width) / 2;
            int centerY = (this.ClientSize.Height - panel5.Height) / 2;
            panel5.Location = new Point(centerX, centerY);
            int center1X = (this.ClientSize.Width - panel6.Width) / 2;
            int center1Y = (this.ClientSize.Height - panel6.Height) / 2;
            panel6.Location = new Point(center1X, center1Y);
        }
        private async void button3_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            LoginDTO loginDto = new LoginDTO
            {
                UserName = username,
                Password = password
            };

            bool isAuthenticated = await _authService.Authenticate(username, password);

            if (isAuthenticated)
            {
                PaginaPrincipala paginaPrincipala = new PaginaPrincipala(_authService);
                paginaPrincipala.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Autentificarea nu a reușit. Vă rugăm să verificați numele de utilizator și parola și să încercați din nou.");
            }
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Numele de utilizator este obligatoriu.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox4.Text) || textBox4.Text.Length < 6)
            {
                MessageBox.Show("Parola este obligatorie și trebuie să aibă cel puțin 6 caractere.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox6.Text) || !IsValidEmail(textBox6.Text))
            {
                MessageBox.Show("Adresa de email este obligatorie și trebuie să fie validă.");
                return;
            }

            string username = textBox3.Text;
            string password = textBox4.Text;
            string email = textBox6.Text;

            RegisterDTO registerDto = new RegisterDTO
            {
                UserName = username,
                Password = password,
                Email = email,
            };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7099/api/auth/register", registerDto);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsAsync<ResponseType>();

                MessageBox.Show("Înregistrarea a reușit și sunteți autentificat!");
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                panel6.Hide();
                panel5.Show();
            }
            else
            {
                MessageBox.Show("Autentificarea nu a reușit. Numele de utilizator deja exista, sau parolele nu corespund!");
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel6.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel6.Visible = true;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isFormDragged = true;
                formStartPosition = new Point(e.X, e.Y);
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isFormDragged)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - formStartPosition.X, currentScreenPos.Y - formStartPosition.Y);
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            isFormDragged = false;
        }
    }
}