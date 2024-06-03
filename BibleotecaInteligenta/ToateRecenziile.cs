using BibleotecaInteligenta.DTOs;
using BibleotecaInteligenta.Services;

namespace BibleotecaInteligenta
{
    public partial class ToateRecenziile : Form
    {
        public int displayedReviewId = 0;
        private ReviewService _reviewService;
        private List<ReviewDTO> _reviews = new List<ReviewDTO>();
        public ToateRecenziile(ReviewService reviewService)
        {
            _reviewService = reviewService;
            InitializeComponent();
        }
        public async void initializeReviews()
        {
            _reviews = await _reviewService.GetReviews();
            LoadReviews();
        }

        public void LoadReviews()
        {
            if (_reviews.Count() > 0)
            {
                panel19.Controls.Clear();
                int grade = _reviews[displayedReviewId].Grade;
                for (int i = 0; i < grade; i++)
                {
                    PictureBox star = new PictureBox();
                    star.Image = Properties.Resources.star;
                    star.SizeMode = PictureBoxSizeMode.StretchImage;
                    star.Width = 15;
                    star.Height = 15;
                    star.Location = new Point(i * star.Width, 0);

                    panel19.Controls.Add(star);
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
        private async void ToateRecenziile_Load(object sender, EventArgs e)
        {
            initializeReviews();
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
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

        private async void button3_Click(object sender, EventArgs e)
        {
            try
            {
                await _reviewService.DeleteReview(_reviews[displayedReviewId].Id);
                displayedReviewId = 0;
                initializeReviews();
                MessageBox.Show("Review sters!");
            }
            catch
            {
                MessageBox.Show("Review deja sters!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
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