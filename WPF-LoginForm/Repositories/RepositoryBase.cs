using System;
using System.Configuration;
using MySqlConnector;

namespace WPFBiblioteca.Repositories;

public abstract class RepositoryBase
{
    private readonly string _server = ConfigurationManager.AppSettings.Get("db.server");
    private readonly string _database = ConfigurationManager.AppSettings.Get("db.database");
    private readonly string _uid= ConfigurationManager.AppSettings.Get("db.uid");
    private readonly string _password = ConfigurationManager.AppSettings.Get("db.password");
    private readonly string _connectionString;

    protected RepositoryBase()
    {
        //BaseDeDatosItspp22.
        var server = _server;
        var database = _database;
        var uid = _uid;
        var password = _password; // se establece coneccion a base de datos externa,.
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