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
    public partial class LasaRecenzie : Form
    {
        public int IdCarte;
        public int IdUser;
        public ReviewService _reviewService;
        public LasaRecenzie(ReviewService reviewService, int idCarte, int idUser)
        {
            _reviewService = reviewService;
            IdCarte = idCarte;
            IdUser = idUser;
            InitializeComponent();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void LasaRecenzie_Load(object sender, EventArgs e)
        {

        }

        public async void SaveReview()
        {
            try
            {
                ReviewDTO reviewDTO = new ReviewDTO
                {
                    Description = richTextBox1.Text,
                    Grade = Convert.ToInt32(numericUpDown1.Value),
                    BookId = IdCarte,
                    UserId = IdUser,
                };
                _reviewService?.CreateReview(reviewDTO);
                MessageBox.Show("Iti multumim pentru recenzie!");
                this.Close();
            }
            catch
            {
                MessageBox.Show("Ceva nu a mers bine! Te rugam incearca din nou!");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SaveReview();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
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
