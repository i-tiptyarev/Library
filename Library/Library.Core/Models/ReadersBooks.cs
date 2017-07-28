using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Models
{
    public class ReadersBooks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BookId { get; set; }
        public int ReaderId { get; set; }
    }
}
