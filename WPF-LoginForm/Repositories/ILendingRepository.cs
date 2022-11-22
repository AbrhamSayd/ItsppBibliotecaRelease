using System.Collections.Generic;
using System.Threading.Tasks;
using WPFBiblioteca.Models;

namespace WPFBiblioteca.Repositories;

public interface ILendingRepository
{
    Task<string> Add(LendingModel lending, UserModel currentUser);
    Task<string> Edit(LendingModel lending, int lendingId);
    Task<string> Delete(int lendingId);
    Task<IEnumerable<LendingModel>> GetByMemberId(int memberId);
    Task<IEnumerable<LendingModel>> GetByBook(int bookId);
    Task<IEnumerable<LendingModel>> GetByAll();
    string GetError();
}