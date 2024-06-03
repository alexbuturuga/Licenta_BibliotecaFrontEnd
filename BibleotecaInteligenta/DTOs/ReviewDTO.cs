using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleotecaInteligenta.DTOs
{
    public class ReviewDTO
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public int Grade { get; set; }
        public BookDTO Book { get; set; }
        public long BookId { get; set; }
        public UserDTO User { get; set; }
        public long UserId { get; set; }
    }
}
