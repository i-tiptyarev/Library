using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Models
{
    public class Readers
    {
        [Key]
        public int Id { get; set; }
        public string FIO { get; set; }
    }
}
