using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySqlConnector;
using WPFBiblioteca.Models;

namespace WPFBiblioteca.Repositories
{
    public class LendingReturnedRepository : RepositoryBase, ILendingReturnedRepository
    {
        private string _errorCode;

        public async Task<IEnumerable<LendingReturnedModel>> GetByAll()
        {
            DateTime dateValue;
            var lendingList = new List<LendingReturnedModel>();
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
                int tempInt;
                while (reader.Read())
                {
                    var lending = new LendingReturnedModel();

                    lending.LendingId = Convert.ToInt32(reader[0].ToString());
                    if (int.TryParse(reader[1].ToString(), out tempInt))
                        lending.BookId = tempInt;

                    lending.MemberName = reader[2].ToString();
                    lending.BookName = reader[3].ToString();
                    if (int.TryParse(reader[4].ToString(), out tempInt))
                        lending.MemberId = tempInt;
                    if (DateTime.TryParse(reader[5].ToString(), out dateValue))
                        lending.DateTimeBorrowed = dateValue;
                    lending.UsernameLent = reader[6].ToString();
                    if (DateTime.TryParse(reader[7].ToString(), out dateValue))
                        lending.DateTimeReturned = dateValue;
                    lending.UsernameReturned = reader[8].ToString();
                    if (int.TryParse(reader[9].ToString(), out tempInt))
                        lending.FinedAmount = tempInt;
                    lending.Remarks = reader[10].ToString();


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
                    command.Parameters.Add("@date_Time_Borrowed", MySqlDbType.DateTime).Value = lending.DateTimeBorrowed;
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
}