using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFBiblioteca.Models
{
    public class LendingReturnedModel
    {
        public int LendingId { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public DateTime DateTimeBorrowed { get; set; }
        public string UsernameLent { get; set; }
        public DateTime DateTimeReturned { get; set; }
        public string UsernameReturned { get; set; }
        public int? FinedAmount { get; set; }
        public string Remarks { get; set; }
    }
}
