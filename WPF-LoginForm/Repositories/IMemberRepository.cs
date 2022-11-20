using System.Collections.Generic;
using System.Threading.Tasks;
using WPFBiblioteca.Models;

namespace WPFBiblioteca.Repositories;

public interface IMemberRepository
{
    Task<string> Add(MemberModel member);
    Task<string> Edit(MemberModel member, int memberId);
    Task<string> Delete(int memberId);
    Task<MemberModel> GetById(int memberId);
    Task<MemberModel> GetByName(string firstName);
    Task<IEnumerable<MemberModel>> GetByAll();
    string GetError();
}