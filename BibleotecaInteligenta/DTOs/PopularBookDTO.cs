namespace BibleotecaInteligenta.DTOs
{
    public class PopularBookDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int PageNumber { get; set; }
        public bool Avalabile { get; set; }
        public DateTime AppearDate { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public string Publisher { get; set; }
        public AuthorDTO Author { get; set; }
        public long AuthorId { get; set; }
        public LanguageDTO Language { get; set; }
        public long LanguageId { get; set; }
        public int Reviews { get; set; }
    }
}