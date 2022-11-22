﻿using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WPFBiblioteca.Models;

namespace WPFBiblioteca.Repositories;

public class LendingRepository : RepositoryBase, ILendingRepository
{
    private string _errorCode;

    public async Task<string> Add(LendingModel lending)
    {
        try
        {
            await using var connection = GetConnection();
            await using (var command = new MySqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText =
                    @"INSERT INTO lendings(Lending_ID, Book_ID, Member_ID, Date_Time_Barrowed, Username_Lent, Date_Time_Returned, Username_Returned, Fined_Aumount, Remarks) 
                       Values(@Lending_ID, @Book_ID, @Member_ID, @Date_Time_Barrowed, @Username_Lent, @Fined_Aumount, @Remarks)";
                command.Parameters.Add("@Lending_ID", MySqlDbType.Int64).Value = lending.LendingId;
                command.Parameters.Add("@Book_ID", MySqlDbType.Int64).Value = lending.BookId;
                command.Parameters.Add("@Member_ID", MySqlDbType.Int64).Value = lending.MemberId;
                command.Parameters.Add("@date_time_Barrowed", MySqlDbType.DateTime).Value = lending.DateTimeBorrowed;
                command.Parameters.Add("@username_Lent", MySqlDbType.String).Value = lending.UsernameLent;
                command.Parameters.Add("@fined_Amount", MySqlDbType.Int64).Value = lending.FinedAmount;
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
                    Date_Time_Returned = @date_time_Returned,
                    Username_Returned = @username_Returned,
                    Fined_Amount = @fined_Amount,
                    Remarks = @remarks
                WHERE Lending_Id = @lending_Id";
                command.Parameters.Add("@book_Id", MySqlDbType.Int64).Value = lending.BookId;
                command.Parameters.Add("@member_Id", MySqlDbType.Int64).Value = lending.MemberId;
                command.Parameters.Add("@date_time_Returned", MySqlDbType.DateTime).Value = lending.DateTimeReturned;
                command.Parameters.Add("@username_Returned", MySqlDbType.String).Value = lending.UsernameReturned;
                command.Parameters.Add("@fined_Amount", MySqlDbType.Int64).Value = lending.FinedAmount;
                command.Parameters.Add("@remarks", MySqlDbType.String).Value = lending.Remarks;
                command.Parameters.Add("@lending_Id", MySqlDbType.Int64).Value = lendingId;
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
                command.Parameters.Add("@book_Id", MySqlDbType.Int64).Value = lendingId;
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
            lendings.Date_Time_Returned,
            lendings.Username_Returned,
            lendings.Fined_Amount,
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
                    DateTimeReturned = DateTime.Parse(reader[5].ToString() ?? string.Empty),
                    UsernameReturned = reader[6].ToString(),
                    FinedAmount = Convert.ToInt32(reader[7].ToString()),
                    Remarks = reader[8].ToString(),
                    MemberName = reader[9].ToString(),
                    BookName = reader[10].ToString(),
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
                    DateTimeReturned = DateTime.Parse(reader[5].ToString() ?? string.Empty),
                    UsernameReturned = reader[6].ToString(),
                    FinedAmount = Convert.ToInt32(reader[7].ToString()),
                    Remarks = reader[8].ToString(),
                    MemberName = reader[9].ToString(),
                    BookName = reader[10].ToString(),
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
            ON lendings.Member_Id = members.Member_Id";
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
                    DateTimeReturned = DateTime.Parse(reader[5].ToString() ?? string.Empty),
                    UsernameReturned = reader[6].ToString(),
                    FinedAmount = Convert.ToInt32(reader[7].ToString()),
                    Remarks = reader[8].ToString(),
                    MemberName = reader[9].ToString(),
                    BookName = reader[10].ToString(),
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
}