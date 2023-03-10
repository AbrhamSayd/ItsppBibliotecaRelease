using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WPFBiblioteca.Models;

namespace WPFBiblioteca.Repositories;

public interface IUserRepository
{
    Task<bool> AuthenticateUser(NetworkCredential credential);
    Task<bool> VerifyMail(string mail);
    Task Add(UserModel userModel);
    Task Edit(UserModel userModel, int id);
    Task Delete(int id);
    UserModel GetById(int id);
    UserModel GetByUsername(string username);
    Task<IEnumerable<UserModel>> GetByAll();

    string GetError();
    //...
}