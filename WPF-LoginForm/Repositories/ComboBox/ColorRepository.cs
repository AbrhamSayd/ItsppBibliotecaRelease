using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBiblioteca.Models;
using WPFBiblioteca.Models.ComboBoxModels;

namespace WPFBiblioteca.Repositories.ComboBox
{
    class ColorRepository : RepositoryBase, IColorRepository
    {
        private string _errorCode;
        public async Task<string> Add(ColorModel color)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Edit(ColorModel color, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ColorModel> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ColorModel>> GetByAll()
        {
            var colorList = new List<ColorModel>();
            await using var connection = GetConnection();
            await using var command = new MySqlCommand();
            try
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = "SELECT * from colors order by Color_Id asc";
                await using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var color = new ColorModel()
                    {
                        ColorId = int.Parse(reader[0].ToString() ?? string.Empty),
                        ColorName = reader[1].ToString()
                    };
                    colorList.Add(color);
                    _errorCode = "400";
                }
            }
            catch (MySqlException e)
            {
                _errorCode = e.ToString();
                throw;
            }


            return colorList;
        }

        public string GetError()
        {
            return _errorCode;
        }
    }
}
