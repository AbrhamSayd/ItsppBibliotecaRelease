using MySqlConnector;

namespace WPFBiblioteca.Repositories
{
    public abstract class RepositoryBase
    {
        
        private readonly string _connectionString;
        public RepositoryBase()
        {
            //BaseDeDatosItspp22.
            string server, database, uid, password;// se establece coneccion a base de datos externa,.
            server = "bjleh1b6zqctbrjujbr0-mysql.services.clever-cloud.com";
            database = "bjleh1b6zqctbrjujbr0";
            uid = "uapdgnuxdwlim1mi";
            password = "XgIn3rTTb4KH4I6wIQ5G";
            _connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            
        }
        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
