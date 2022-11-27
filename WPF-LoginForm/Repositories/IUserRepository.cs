using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MySqlConnector;
using WPFBiblioteca.Models;

namespace WPFBiblioteca.Repositories;

public interface IUserRepository
{
    Task<bool> AuthenticateUser(NetworkCredential credential);
    Task Add(UserModel userModel);
    Task Edit(UserModel userModel, int id);
    Task Delete(int id);
    UserModel GetById(int id);
    UserModel GetByUsername(string username);
    Task<IEnumerable<UserModel>> GetByAll();

    MySqlException GetError();
    //...
}