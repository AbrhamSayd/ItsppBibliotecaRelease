using System;

namespace WPFBiblioteca.Models;

public class LendingModel
{
    public int LendingId { get; set; }
    public int BookId { get; set; }
    public string BookName { get; set; }
    public int MemberId { get; set; }
    public string MemberName { get; set; }
    public DateTime DateTimeBorrowed { get; set; }
    public string UsernameLent { get; set; }
    public string Remarks { get; set; }
}