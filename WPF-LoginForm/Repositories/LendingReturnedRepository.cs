using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MySqlConnector;
using WPFBiblioteca.Helpers;
using WPFBiblioteca.Models;

namespace WPFBiblioteca.Repositories;

public class LendingReturnedRepository : RepositoryBase, ILendingReturnedRepository
{
    private string _errorCode;

    public async Task<IEnumerable<LendingReturnedModel>> GetByAll()
    {
        var lendingList = new List<LendingReturnedModel>();
        await using var connection = GetConnection();
        await using var command = new MySqlCommand();
        try
        {
            await connection.OpenAsync();
            command.Connection = connection;
            command.CommandText =
                @"SELECT
            lendings_returned.Lending_Id,
            lendings_returned.Book_Id,
            CONCAT(members.First_Name, ' ', members.Last_Name) AS Member_Name,
                books.Name AS Book_Name,
                lendings_returned.Member_Id,
            lendings_returned.Date_Time_Borrowed,
            lendings_returned.Username_Lent,
            lendings_returned.Date_Time_Returned,
            lendings_returned.Username_Returned,
            lendings_returned.Fined_Amount,
            lendings_returned.Remarks
                FROM lendings_returned 
                LEFT OUTER JOIN books
                ON lendings_returned.Book_Id = books.Book_Id
            LEFT OUTER JOIN members
            ON lendings_returned.Member_Id = members.Member_Id";
            await using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                var lending = new LendingReturnedModel
                {
                    LendingId = ValidationHelper.TryConvert.ToInt32(reader[0].ToString(), 0),
                    BookId = ValidationHelper.TryConvert.ToInt32(reader[1].ToString(), 0),
                    BookName = reader[3].ToString() ?? "No encontrado",
                    MemberId = ValidationHelper.TryConvert.ToInt32(reader[4].ToString(), 0),
                    MemberName = reader[2].ToString() ?? "No encontrado",
                    DateTimeBorrowed = ValidationHelper.TryConvert.ToDateTime(reader[5].ToString(), DateTime.Now),
                    UsernameLent = reader[6].ToString() ?? "No encontrado",
                    DateTimeReturned = ValidationHelper.TryConvert.ToDateTime(reader[7].ToString(), DateTime.Now),
                    UsernameReturned = reader[8].ToString() ?? "No encontrado",
                    FinedAmount = ValidationHelper.TryConvert.ToInt32(reader[9].ToString(), 0),
                    Remarks = reader[9].ToString() ?? "Sin notas"
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

    public async Task<string> Insert(LendingModel lending, UserModel user)
    {
        try
        {
            await using var connection = GetConnection();
            await using (var command = new MySqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText =
                    "INSERT INTO lendings_returned (Lending_Id, Book_Id, Member_Id, Date_Time_Borrowed, Username_Lent, Date_Time_Returned, Username_Returned, Remarks) VALUES (@lending_Id, @book_Id, @member_Id, @date_Time_Borrowed, @username_Lent, NOW(),@username_Returned, @remarks)";
                command.Parameters.Add("@lending_Id", MySqlDbType.Int64).Value = lending.LendingId;
                command.Parameters.Add("@book_Id", MySqlDbType.Int64).Value = lending.BookId;
                command.Parameters.Add("@member_Id", MySqlDbType.Int64).Value = lending.MemberId;
                command.Parameters.Add("@date_Time_Borrowed", MySqlDbType.DateTime).Value =
                    lending.DateTimeBorrowed;
                command.Parameters.Add("@username_Lent", MySqlDbType.VarChar).Value = lending.UsernameLent;
                command.Parameters.Add("@username_Returned", MySqlDbType.VarChar).Value = user.FirstName;
                command.Parameters.Add("@remarks", MySqlDbType.VarChar).Value = lending.Remarks;
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

    public string GetError()
    {
        return _errorCode;
    }
}