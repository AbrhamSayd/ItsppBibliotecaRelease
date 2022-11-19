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
    public class LendingRepository : RepositoryBase, ILendingRepository
    {
        private string _errorCode;//No implementado

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
                    "select * from books left join categories on categories.Category_Id = books.Category_Id";
                await using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var lending = new LendingModel
                    {
                        //NOT IMPLEMENTED

                        //Id = int.Parse(reader[0].ToString() ?? string.Empty),
                        //Isbn = reader[1].ToString(),
                        //Name = reader[2].ToString(),
                        //Author = reader[3].ToString(),
                        //Editorial = reader[4].ToString(),
                        //PublishedYear = reader[5].ToString(),
                        //Stock = int.Parse(reader[6].ToString() ?? string.Empty),
                        //Color = reader[7].ToString(),
                        //CategoryId = int.Parse(reader[8].ToString() ?? string.Empty),
                        //Location = reader[9].ToString(),
                        //Remarks = reader[10].ToString(),
                        //CategoryDescription = reader[12].ToString()
                    };
                    lendingList.Add(lending);
                    _errorCode = "400";
                }
            }
            catch (Exception e)
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
}
