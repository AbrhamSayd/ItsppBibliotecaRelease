using System.Collections.Generic;
using System.Threading.Tasks;
using WPFBiblioteca.Models;

namespace WPFBiblioteca.Repositories;

public interface ILendingRepository
{
    Task<string> Add(LendingModel lending);
    Task<string> Edit(LendingModel lending, int lendingId);
    Task<string> Delete(int lendingId);
    Task<MemberModel> GetById(int lendingId);
    Task<MemberModel> GetByName(string firstName);
    Task<IEnumerable<LendingModel>> GetByAll();
    string GetError();
}