using System.Collections.Generic;
using System.Threading.Tasks;
using WPFBiblioteca.Models;

namespace WPFBiblioteca.Repositories;

public interface ICategoryRepository
{
    Task<string> Add(CategoryModel category);
    Task<string> Edit(CategoryModel category, int id);
    Task<string> Delete(int id);
    Task<CategoryModel> GetById(int id);
    Task<CategoryModel> GetByName(string isbn);
    Task<IEnumerable<CategoryModel>> GetByAll();
    string GetError();
}