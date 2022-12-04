using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MySqlConnector;
using WPFBiblioteca.Helpers;
using WPFBiblioteca.Models;
using static WPFBiblioteca.Helpers.ValidationHelper;

namespace WPFBiblioteca.Repositories;

public class LendingRepository : RepositoryBase, ILendingRepository
{
    private string _errorCode;


    public async Task<string> Add(LendingModel lending, UserModel currentUser)
    {
        try
        {
            await using var connection = GetConnection();
            await using (var command = new MySqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText =
                    "INSERT INTO lendings (Book_Id, Member_Id, Date_Time_Borrowed, Username_Lent, Remarks) VALUES (@book_Id, @member_Id, NOW(), @username, @remarks)";
                command.Parameters.Add("@book_Id", MySqlDbType.Int64).Value = lending.BookId;
                command.Parameters.Add("@member_Id", MySqlDbType.Int64).Value = lending.MemberId;
                command.Parameters.Add("@username", MySqlDbType.String).Value = currentUser.Username;
                command.Parameters.Add("@remarks", MySqlDbType.String).Value = lending.Remarks;

                await command.ExecuteScalarAsync(CancellationToken.None);
                _errorCode = "400";
            }
        }
        catch (Exception e)
        {
            _errorCode = e.ToString();
            throw;
        }


        return _errorCode;
    }

    public async Task<string> Edit(LendingModel lending, int lendingId)
    {
        try
        {
            await using var connection = GetConnection();
            await using (var command = new MySqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText =
                    @"UPDATE lendings
                SET Book_Id = @book_Id,
                    Member_Id = @member_Id,
                    Remarks = @remarks
                WHERE Lending_Id = @lending_Id";
                command.Parameters.Add("@book_Id", MySqlDbType.Int64).Value = lending.BookId;
                command.Parameters.Add("@member_Id", MySqlDbType.Int64).Value = lending.MemberId;
                command.Parameters.Add("@remarks", MySqlDbType.String).Value = lending.Remarks;
                command.Parameters.Add("@lending_Id", MySqlDbType.Int64).Value = lendingId;
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

    public async Task<string> Delete(int lendingId)
    {
        await using var connection = GetConnection();
        await using (var command = new MySqlCommand())
        {
            try
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText =
                    @"DELETE
                FROM lendings
                WHERE Lending_Id = @lending_Id";
                command.Parameters.Add("@lending_Id", MySqlDbType.Int64).Value = lendingId;
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

    public async Task<IEnumerable<LendingModel>> GetByMemberId(int memberId)
    {
        var lendingList = new List<LendingModel>();
        await using var connection = GetConnection();
        await using var command = new MySqlCommand();
        try
        {
            await connection.OpenAsync();
            command.Connection = connection;
            command.CommandText =
                @"SELECT
            lendings.Book_Id,
            CONCAT(members.First_Name, ' ', members.Last_Name) AS Member_Name,
                books.Name AS Book_Name,
                lendings.Member_Id,
            lendings.Date_Time_Borrowed,
            lendings.Username_Lent,
            lendings.Remarks,
            lendings.Lending_Id
                FROM lendings
                LEFT OUTER JOIN books
                ON lendings.Book_Id = books.Book_Id
            LEFT OUTER JOIN members
            ON lendings.Member_Id = members.Member_Id WHERE lendings.Member_Id = @member_Id";
            command.Parameters.Add("@member_Id", MySqlDbType.Int64).Value = memberId;
            await using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                var lending = new LendingModel
                {
                    LendingId = Convert.ToInt32(reader[0].ToString()),
                    BookId = Convert.ToInt32(reader[1].ToString()),
                    MemberId = Convert.ToInt32(reader[2].ToString()),
                    DateTimeBorrowed = DateTime.Parse(reader[3].ToString() ?? string.Empty),
                    UsernameLent = reader[4].ToString(),
                    Remarks = reader[5].ToString(),
                    MemberName = reader[6].ToString(),
                    BookName = reader[7].ToString()
                };

                lendingList.Add(lending);
                _errorCode = "400";
            }
        }
        catch (MySqlException e)
        {
            _errorCode = e.ToString();
            throw;
        }

        return lendingList;
    }

    public async Task<IEnumerable<LendingModel>> GetByBook(int bookId)
    {
        var lendingList = new List<LendingModel>();
        await using var connection = GetConnection();
        await using var command = new MySqlCommand();
        try
        {
            await connection.OpenAsync();
            command.Connection = connection;
            command.CommandText =
                @"SELECT
            lendings.Book_Id,
            CONCAT(members.First_Name, ' ', members.Last_Name) AS Member_Name,
                books.Name AS Book_Name,
                lendings.Member_Id,
            lendings.Date_Time_Borrowed,
            lendings.Username_Lent,
            lendings.Date_Time_Returned,
            lendings.Username_Returned,
            lendings.Fined_Amount,
            lendings.Remarks,
            lendings.Lending_Id
                FROM lendings
                LEFT OUTER JOIN books
                ON lendings.Book_Id = books.Book_Id
            LEFT OUTER JOIN members
            ON lendings.Member_Id = members.Member_Id WHERE lendings.Book_Id = @book_Id";
            command.Parameters.Add("@book_Id", MySqlDbType.Int64).Value = bookId;
            await using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                var lending = new LendingModel
                {
                    LendingId = Convert.ToInt32(reader[0].ToString()),
                    BookId = Convert.ToInt32(reader[1].ToString()),
                    MemberId = Convert.ToInt32(reader[2].ToString()),
                    DateTimeBorrowed = DateTime.Parse(reader[3].ToString() ?? string.Empty),
                    UsernameLent = reader[4].ToString(),
                    Remarks = reader[5].ToString(),
                    MemberName = reader[6].ToString(),
                    BookName = reader[7].ToString()
                };

                lendingList.Add(lending);
                _errorCode = "400";
            }
        }
        catch (MySqlException e)
        {
            _errorCode = e.ToString();
            throw;
        }

        return lendingList;
    }

    public async Task<IEnumerable<LendingModel>> GetByAll()
    {
        var memberRepository = new MemberRepository();
        var userRepository = new UserRepository();
        var lendingList = new List<LendingModel>();
        await using var connection = GetConnection();
        await using var command = new MySqlCommand();
        try
        {
            await connection.OpenAsync();
            command.Connection = connection;
            command.CommandText =
                @"SELECT
              lendings.Lending_Id,
              lendings.Book_Id,
              books.Name AS Book_Name,
              lendings.Member_Id,
              lendings.Date_Time_Borrowed,
              lendings.Username_Lent,
              lendings.Remarks
            FROM lendings
              INNER JOIN books
                ON lendings.Book_Id = books.Book_Id";
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var member = Task.Run(() => memberRepository.GetById(TryConvert.ToInt32(reader[3].ToString(), 0)))
                    .Result;
                var user = Task.Run(() => userRepository.GetByUsername(reader[5].ToString())).Result;
                DateTime.TryParse(reader[4].ToString(), out var dateValue);
                var lending = new LendingModel
                {
                    LendingId = TryConvert.ToInt32(reader[0].ToString(), 0),
                    BookId = TryConvert.ToInt32(reader[1].ToString(), 0),
                    MemberName = member.FirstName + " " + member.LastName,
                    BookName = reader[2].ToString(),
                    MemberId = TryConvert.ToInt32(reader[2].ToString(), 0),
                    DateTimeBorrowed = dateValue,
                    UsernameLent = user.FirstName,
                    Remarks = reader[6].ToString(),
                };

                lendingList.Add(lending);
                _errorCode = "400";
            }
        }
        catch (MySqlException e)
        {
            _errorCode = e.ToString();
            throw;
        }

        return lendingList;
    }

    public string GetError()
    {
        return _errorCode;
    }

    public Task<LendingModel> GetById(int lendingId)
    {
        throw new NotImplementedException();
    }

    public Task<LendingModel> GetByBook(string firstName)
    {
        throw new NotImplementedException();
    }
}
