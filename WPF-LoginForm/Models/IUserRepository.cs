﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WPFBiblioteca.Models
{
    public interface IUserRepository
    {
        Task<bool> AuthenticateUser(NetworkCredential credential);
        Task Add(UserModel userModel);
        void Edit(UserModel userModel);
        Task Remove(int id);
        UserModel GetById(int id);
        UserModel GetByUsername(string username);
        Task<IEnumerable<UserModel>> GetByAll();
        //...
    }
}
