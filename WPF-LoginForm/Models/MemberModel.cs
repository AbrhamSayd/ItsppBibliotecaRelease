namespace WPFBiblioteca.Models;

public class MemberModel
{
    public int MemberId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Carrera { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool Deudor { get; set; }
    public int Prestamos { get; set; }
}