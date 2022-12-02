using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBiblioteca.Models;

namespace WPFBiblioteca.Repositories
{
    internal interface ILendingReturnedRepository
    {
        Task<IEnumerable<LendingReturnedModel>> GetByAll();
        string GetError();
    }
}
