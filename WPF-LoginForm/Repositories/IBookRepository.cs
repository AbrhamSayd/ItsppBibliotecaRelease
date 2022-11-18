using System.Collections.Generic;
using System.Threading.Tasks;
using WPFBiblioteca.Models;

namespace WPFBiblioteca.Repositories
{
    public interface IBookRepository
    {
        Task<string>Add(BookModel book, int categoryId);
        Task<string> Edit(BookModel user, int id);
        Task<string> Delete(int id);
        Task<BookModel> GetById(int id);
        Task<BookModel> GetByName(string isbn);
        Task<IEnumerable<BookModel>> GetByAll();
        string GetError();
    }
}
