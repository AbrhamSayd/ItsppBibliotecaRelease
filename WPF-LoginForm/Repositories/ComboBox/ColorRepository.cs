using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySqlConnector;
using WPFBiblioteca.Models.ComboBoxModels;

namespace WPFBiblioteca.Repositories.ComboBox;

internal class ColorRepository : RepositoryBase, IColorRepository
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
        int tempInt;

        ColorModel color = null;
        await using var connection = GetConnection();
        await using var command = new MySqlCommand();
        try
        {
            await connection.OpenAsync();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM colors Where Color_Id = @color_Id;";
            command.Parameters.Add("@color_Id", MySqlDbType.Int64).Value = id;
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                color = new ColorModel();
                if (int.TryParse(reader[0].ToString(), out tempInt))
                    color.ColorId = tempInt;
                color.ColorName = reader[1].ToString();
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

        return color;
    }

    public async Task<IEnumerable<ColorModel>> GetByAll()
    {
        var colorList = new List<ColorModel>();
        try
        {
            await using var connection = GetConnection();
            await using var command = new MySqlCommand();
            await connection.OpenAsync();
            command.Connection = connection;
            command.CommandText = "SELECT * from colors order by Color_Id asc";
            await using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                var color = new ColorModel
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