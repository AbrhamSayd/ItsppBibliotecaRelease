using System.Collections.Generic;
using System.Threading.Tasks;
using WPFBiblioteca.Models;

namespace WPFBiblioteca.Repositories;

public interface ILendingRepository
{
    Task<string> Add(LendingModel lending, UserModel currentUser);
    Task<string> Edit(LendingModel lending, int lendingId);
    Task<string> Delete(int lendingId);
    Task<LendingModel> GetById(int lendingId);
    Task<LendingModel> GetByName(string firstName);
    Task<IEnumerable<LendingModel>> GetByAll();
    string GetError();
}