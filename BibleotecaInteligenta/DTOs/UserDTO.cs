namespace BibleotecaInteligenta.DTOs
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string UserAccountID { get; set; }
        public bool Admin { get; set; }
        public string UserName { get; set; }
    }
}
