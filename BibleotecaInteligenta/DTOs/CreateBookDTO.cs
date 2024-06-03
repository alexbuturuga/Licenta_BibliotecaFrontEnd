namespace BibleotecaInteligenta.DTOs
{
    public class CreateBookDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int PageNumber { get; set; }
        public bool Avalabile { get; set; }
        public DateTime AppearDate { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public string Publisher { get; set; }
        public long AuthorId { get; set; }
        public long LanguageId { get; set; }
    }
}
