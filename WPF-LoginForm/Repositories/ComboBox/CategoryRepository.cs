using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySqlConnector;
using WPFBiblioteca.Models;

namespace WPFBiblioteca.Repositories.ComboBox;

public class CategoryRepository : RepositoryBase, ICategoryRepository
{
    private string _errorCode;

    public async Task<string> Add(CategoryModel category)
    {
        throw new NotImplementedException();
    }

    public async Task<string> Edit(CategoryModel category, int id)
    {
        throw new NotImplementedException();
    }

    public async Task<string> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<CategoryModel> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<CategoryModel> GetByName(string isbn)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<CategoryModel>> GetByAll()
    {
        var categoryList = new List<CategoryModel>();
        await using var connection = GetConnection();
        await using var command = new MySqlCommand();
        try
        {
            await connection.OpenAsync();
            command.Connection = connection;
            command.CommandText = "SELECT * from categories order by Category_ID asc";
            await using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                var category = new CategoryModel
                {
                    CategoryId = int.Parse(reader[0].ToString() ?? string.Empty),
                    Description = reader[1].ToString()
                };
                categoryList.Add(category);
                _errorCode = "400";
            }
        }
        catch (Exception e)
        {
            _errorCode = e.ToString();
            throw;
        }


        return categoryList;
    }

    public string GetError()
    {
        throw new NotImplementedException();
    }
}