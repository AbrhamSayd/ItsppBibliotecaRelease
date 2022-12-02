using System;
using MySqlConnector;

namespace WPFBiblioteca.Repositories;

public abstract class RepositoryBase
{
    private readonly string _connectionString;

    protected RepositoryBase()
    {
        //BaseDeDatosItspp22.
        const string server = "localhost";
        const string database = "bjleh1b6zqctbrjujbr0";
        const string uid = "root";
        const string password = "itspp"; // se establece coneccion a base de datos externa,.
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