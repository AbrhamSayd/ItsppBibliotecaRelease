using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySqlConnector;
using WPFBiblioteca.Models;

namespace WPFBiblioteca.Repositories
{
    public class LendingRepository : RepositoryBase, ILendingRepository
    {
        
        public async Task<string> Add(LendingModel lending)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Edit(LendingModel lending, int lendingId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Delete(int lendingId)
        {
            throw new NotImplementedException();
        }

        public async Task<MemberModel> GetById(int lendingId)
        {
            throw new NotImplementedException();
        }

        public async Task<MemberModel> GetByName(string firstName)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<LendingModel>> GetByAll()
        {
            throw new NotImplementedException();
        }

        public string GetError()
        {
            throw new NotImplementedException();
        }
    }
}
