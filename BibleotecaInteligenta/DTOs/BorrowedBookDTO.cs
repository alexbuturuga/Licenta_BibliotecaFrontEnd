namespace BibleotecaInteligenta.DTOs
{
    public class BorrowedBookDTO
    {
        public long Id { get; set; }
        public long BookId { get; set; }
        public long UserId { get; set; }
        public bool Confirmed { get; set; }
        public BookDTO Book { get; set; }
        public UserDTO User { get; set; }
        public DateTime BorrowStartDate { get; set; }
        public DateTime BorrowDeathLine { get; set; }
        public DateTime? BorrowEndDate { get; set; }
    }
}