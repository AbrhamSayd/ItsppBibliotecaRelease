using System;
using MySqlConnector;

namespace WPFBiblioteca.Repositories;

public abstract class RepositoryBase
{
    private readonly string _connectionString;

    protected RepositoryBase()
    {
        //BaseDeDatosItspp22.
        const string server = "140.84.183.195";
        const string database = "biblioteca";
        const string uid = "libraryConnection";
        const string password = @"it-2spp21.Conn][1/^7"; // se establece coneccion a base de datos externa,.
        _connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" +
                            password + ";";
    }

    protected MySqlConnection GetConnection()
    {
        try
        {
            return new MySqlConnection(_connectionString);
        }
        catch (MySqlException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}