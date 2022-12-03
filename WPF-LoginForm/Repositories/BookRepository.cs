using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MySqlConnector;
using WPFBiblioteca.Models;

namespace WPFBiblioteca.Repositories;

public class BookRepository : RepositoryBase, IBookRepository
{
    private string _errorCode;


    public async Task<string> Add(BookModel book, int categoryId)
    {
        try
        {
            await using var connection = GetConnection();
            await using (var command = new MySqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText =
                    "INSERT INTO books(Isbn, Name, Author, Editorial, Published_Year, stock, Color_Id, Category_Id, Location, Remarks) VALUES(@isbn,@name,@author,@editoral,@published_Year,@stock,@colorId,@category_Id,@location,@remarks)";
                command.Parameters.Add("@isbn", MySqlDbType.VarChar).Value = book.Isbn;
                command.Parameters.Add("@name", MySqlDbType.VarChar).Value = book.Name;
                command.Parameters.Add("@author", MySqlDbType.VarChar).Value = book.Author;
                command.Parameters.Add("@editoral", MySqlDbType.VarChar).Value = book.Editorial;
                command.Parameters.Add("@published_Year", MySqlDbType.DateTime).Value = book.PublishedYear;
                command.Parameters.Add("@stock", MySqlDbType.Int64).Value = book.Stock;
                command.Parameters.Add("@colorId", MySqlDbType.Int16).Value = book.ColorId;
                command.Parameters.Add("@category_Id", MySqlDbType.Int64).Value = book.CategoryId;
                command.Parameters.Add("@location", MySqlDbType.String).Value = book.Location;
                command.Parameters.Add("@remarks", MySqlDbType.String).Value = book.Remarks;
                await command.ExecuteScalarAsync(CancellationToken.None);
                _errorCode = "400";
            }
        }
        catch (MySqlException e)
        {
            _errorCode = e.ToString();
            throw;
        }


        return _errorCode;
    }

    public async Task<string> Edit(BookModel book, int id)
    {
        await using var connection = GetConnection();
        await using (var command = new MySqlCommand())
        {
            try
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText =
                    "UPDATE" +
                    " books" +
                    " SET" +
                    " Isbn = @isbn," +
                    " NAME = @name," +
                    " Author = @author," +
                    " Editorial = @editorial," +
                    " Published_Year = @published_Year," +
                    " Stock = @stock," +
                    " Color_Id = @colorId," +
                    " Category_Id = @category_Id," +
                    " Location = @location," +
                    " Remarks = @remarks" +
                    " WHERE" +
                    " Book_Id = @book_Id;";
                command.Parameters.Add("@book_Id", MySqlDbType.Int64).Value = book.Id;
                command.Parameters.Add("@isbn", MySqlDbType.VarChar).Value = book.Isbn;
                command.Parameters.Add("@name", MySqlDbType.VarChar).Value = book.Name;
                command.Parameters.Add("@author", MySqlDbType.VarChar).Value = book.Author;
                command.Parameters.Add("@editorial", MySqlDbType.VarChar).Value = book.Editorial;
                command.Parameters.Add("@published_Year", MySqlDbType.DateTime).Value = book.PublishedYear;
                command.Parameters.Add("@stock", MySqlDbType.Int64).Value = book.Stock;
                command.Parameters.Add("@colorId", MySqlDbType.Int16).Value = book.ColorId;
                command.Parameters.Add("@category_Id", MySqlDbType.Int64).Value = book.CategoryId;
                command.Parameters.Add("@location", MySqlDbType.String).Value = book.Location;
                command.Parameters.Add("@remarks", MySqlDbType.String).Value = book.Remarks;
                await command.ExecuteScalarAsync(CancellationToken.None);
                await command.ExecuteScalarAsync(CancellationToken.None);
                _errorCode = "400";
            }
            catch (MySqlException e)
            {
                _errorCode = e.ToString();
                throw;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        return _errorCode;
    }

    public async Task<string> Delete(int id)
    {
        await using var connection = GetConnection();
        await using (var command = new MySqlCommand())
        {
            try
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText =
                    "DELETE FROM books WHERE Book_Id = @book_Id;";
                command.Parameters.Add("@book_Id", MySqlDbType.Int64).Value = id;

                await command.ExecuteScalarAsync(CancellationToken.None);
                _errorCode = "400";
            }
            catch (Exception e)
            {
                _errorCode = e.ToString();
                throw;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        return _errorCode;
    }

    public async Task<BookModel> GetById(int id)
    {
        int tempInt;
        BookModel book = null;
        await using var connection = GetConnection();
        await using var command = new MySqlCommand();
        try
        {
            await connection.OpenAsync();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM books Where Book_Id = @book_Id;";
            command.Parameters.Add("@book_Id", MySqlDbType.VarChar).Value = id.ToString();
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                book = new BookModel();
                if (int.TryParse(reader[0].ToString(), out tempInt))
                    book.Id = tempInt;
                book.Isbn = reader[1].ToString();
                book.Name = reader[2].ToString();
                book.Author = reader[3].ToString();
                book.Editorial = reader[4].ToString();
                if (int.TryParse(reader[5].ToString(), out tempInt))
                    book.PublishedYear = tempInt;
                book.Stock = int.Parse(reader[6].ToString() ?? string.Empty);
                if (int.TryParse(reader[7].ToString(), out tempInt))
                    book.ColorId = tempInt;
                book.CategoryId = int.Parse(reader[8].ToString() ?? string.Empty);
                book.Location = reader[9].ToString();
                book.Remarks = reader[10].ToString();
            }


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

        return book;
    }
    public async Task<BookModel> GetById(long id)
    {
        int tempInt;
        BookModel book = null;
        await using var connection = GetConnection();
        await using var command = new MySqlCommand();
        try
        {
            await connection.OpenAsync();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM books Where Isbn = @book_Id;";
            command.Parameters.Add("@book_Id", MySqlDbType.VarChar).Value = id.ToString();
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                book = new BookModel();
                if (int.TryParse(reader[0].ToString(), out tempInt))
                    book.Id = tempInt;
                book.Isbn = reader[1].ToString();
                book.Name = reader[2].ToString();
                book.Author = reader[3].ToString();
                book.Editorial = reader[4].ToString();
                if (int.TryParse(reader[5].ToString(), out tempInt))
                    book.PublishedYear = tempInt;
                book.Stock = int.Parse(reader[6].ToString() ?? string.Empty);
                if (int.TryParse(reader[7].ToString(), out tempInt))
                    book.ColorId = tempInt;
                book.CategoryId = int.Parse(reader[8].ToString() ?? string.Empty);
                book.Location = reader[9].ToString();
                book.Remarks = reader[10].ToString();
            }


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

        return book;
    }

    public async Task<BookModel> GetByIsbn(string isbn)
    {
        int tempInt;
        BookModel book = null;
        await using var connection = GetConnection();
        await using var command = new MySqlCommand();
        try
        {
            await connection.OpenAsync();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM users Where Isbn = @isbn;";
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                book = new BookModel();
                if (int.TryParse(reader[0].ToString(), out tempInt))
                    book.Id = tempInt;
                book.Isbn = reader[1].ToString();
                book.Name = reader[2].ToString();
                book.Author = reader[3].ToString();
                book.Editorial = reader[4].ToString();
                if (int.TryParse(reader[5].ToString(), out tempInt))
                    book.PublishedYear = tempInt;
                book.Stock = int.Parse(reader[6].ToString() ?? string.Empty);
                if (int.TryParse(reader[7].ToString(), out tempInt))
                    book.ColorId = tempInt;
                book.CategoryId = int.Parse(reader[8].ToString() ?? string.Empty);
                book.Location = reader[9].ToString();
                book.Remarks = reader[10].ToString();
            }

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

        return book;
    }

    public async Task<IEnumerable<BookModel>> GetByAll()
    {
        var bookList = new List<BookModel>();
        await using var connection = GetConnection();
        await using var command = new MySqlCommand();
        try
        {
            await connection.OpenAsync();
            command.Connection = connection;
            command.CommandText =
                @"SELECT
            books.*,
            categories.Description,
            colors.Color_Name
                FROM books
                LEFT OUTER JOIN  colors
                ON books.Color_Id = colors.Color_Id
            LEFT OUTER  JOIN categories
            ON books.Category_Id = categories.Category_Id";
            await using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                var book = new BookModel();
                if (int.TryParse(reader[0].ToString(), out var tempInt))
                    book.Id = tempInt;
                book.Isbn = reader[1].ToString();
                book.Name = reader[2].ToString();
                book.Author = reader[3].ToString();
                book.Editorial = reader[4].ToString();
                if (int.TryParse(reader[5].ToString(), out tempInt))
                    book.PublishedYear = tempInt;
                book.Stock = int.Parse(reader[6].ToString() ?? string.Empty);
                if (int.TryParse(reader[7].ToString(), out tempInt))
                    book.ColorId = tempInt;
                book.CategoryId = int.Parse(reader[8].ToString() ?? string.Empty);
                book.Location = reader[9].ToString();
                book.Remarks = reader[10].ToString();
                book.Category = reader[11].ToString() ?? "No asignado";
                book.Color = reader[12].ToString() ?? "No asignado";
                bookList.Add(book);
                _errorCode = "400";
            }
        }
        catch (MySqlException e)
        {
            _errorCode = e.ToString();
            throw;
        }

        return bookList;
    }

    public string GetError()
    {
        return _errorCode;
    }
}