using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySqlConnector;
using WPFBiblioteca.Models.ComboBoxModels;

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
        int tempInt;

        CategoryModel category = null;
        await using var connection = GetConnection();
        await using var command = new MySqlCommand();
        var connectionTask = connection.OpenAsync();

        Task.WaitAll(connectionTask); //make sure the task is completed
        if (connectionTask.IsFaulted) // in case of failure
            throw new Exception("Connection failure", connectionTask.Exception);
        command.Connection = connection;
        try
        {
            await connection.OpenAsync();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM categories Where Category_Id = @category_Id;";
            command.Parameters.Add("@category_Id", MySqlDbType.Int64).Value = id;
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                category = new CategoryModel();
                if (int.TryParse(reader[0].ToString(), out tempInt))
                    category.CategoryId = tempInt;
                category.Description = reader[1].ToString();
            }

            await connection.CloseAsync();
            _errorCode = "400";
        }
        catch (MySqlException ex)
        {
            _errorCode = ex.ToString();
        }
        finally
        {
            await connection.CloseAsync();
        }

        return category;
    }

    public async Task<CategoryModel> GetByName(string isbn)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<CategoryModel>> GetByAll()
    {
        var categoryList = new List<CategoryModel>();
        try
        {
            await using var connection = GetConnection();
            await using var command = new MySqlCommand();
            var connectionTask = connection.OpenAsync();

            Task.WaitAll(connectionTask); //make sure the task is completed
            if (connectionTask.IsFaulted) // in case of failure
                throw new Exception("Connection failure", connectionTask.Exception);
            command.Connection = connection;
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
        catch (MySqlException e)
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