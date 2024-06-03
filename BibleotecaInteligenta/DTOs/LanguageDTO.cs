namespace BibleotecaInteligenta.DTOs
{
    public class LanguageDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class CreateLanguageDTO
    {
        public string Name { get; set; }
    }
}
