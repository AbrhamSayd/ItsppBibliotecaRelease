using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WPFBiblioteca.Repositories
{
    public abstract class RepositoryBase
    {
        
        private readonly string _connectionString;
        public RepositoryBase()
        {
            //server = "bjleh1b6zqctbrjujbr0-mysql.services.clever-cloud.com";
            //database = "bjleh1b6zqctbrjujbr0";
            //uid = "uapdgnuxdwlim1mi";
            //password = "XgIn3rTTb4KH4I6wIQ5G";
            //BaseDeDatosItspp22.
            string server, database, uid, password;// se establece coneccion a base de datos externa,.
            server = "localhost";
            database = "newschema";
            uid = "root";
            password = "itspp";
            _connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            
        }
        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
