using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleotecaInteligenta.DTOs
{
    public class ComboBoxItem
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public ComboBoxItem(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
