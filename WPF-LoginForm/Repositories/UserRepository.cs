using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WPFBiblioteca.Models;

namespace WPFBiblioteca.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public async Task<bool> Add(UserModel userModel)
        {

            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
               await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText =
                "insert into users(Username_ID,Username,Password,First_Name,Last_Name,User_Type) values (@username_id,@username,@password,@first_name,@last_name,@user_type)";
                command.Parameters.Add("@username_id", MySqlDbType.Int64).Value = userModel.Id;
                command.Parameters.Add("@username", MySqlDbType.VarChar).Value = userModel.Username;
                command.Parameters.Add("@password", MySqlDbType.VarChar).Value = userModel.Password;
                command.Parameters.Add("@first_name", MySqlDbType.VarChar).Value = userModel.FirstName;
                command.Parameters.Add("@last_name", MySqlDbType.VarChar).Value = userModel.LastName;
                command.Parameters.Add("@user_type", MySqlDbType.VarChar).Value = userModel.UserType;
                try
                {
                    await command.ExecuteScalarAsync();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                    return false;
                }
                
            }

            
        }

        public async Task<bool> AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
               await connection.OpenAsync();//abrimos la connecion con MySQL
                command.Connection = connection;//asignamos al comando la coneccion a mysql
                command.CommandText = "select * from users where Username=@username and password=@password";//Query para evualar nuestro nombre de usuario y contraseñá
                command.Parameters.Add("@username", MySqlDbType.VarChar).Value = credential.UserName;//definimos parametro de username
                command.Parameters.Add("@password", MySqlDbType.VarChar).Value = credential.Password;//definimos parametro de password
                 validUser = command.ExecuteScalar() == null ? false : true;
            }
            return validUser;//retorna si es que nuestro logeo fue correcto
        }

        public void Edit(UserModel userModel)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<UserModel>> GetByAll()
        {
            var userList = new List<UserModel>();
            using var connection = GetConnection();
            using (var command = new MySqlCommand())
            {
               await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = "SELECT * from users order by First_Name asc";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var userModel = new UserModel
                        {
                            Id = int.Parse(reader[0].ToString() ?? string.Empty),
                            Username = reader[1].ToString() ?? string.Empty,
                            Password = reader[2].ToString() ?? string.Empty,
                            FirstName = reader[3].ToString() ?? string.Empty,
                            LastName = reader[4].ToString() ?? string.Empty,
                            UserType = reader[5].ToString() ?? string.Empty,
                        };
                        userList.Add(userModel);
                    }
                }
            }

            return userList;
        }
        public UserModel GetById(int id)
        {
            throw new NotImplementedException();
        }
        public UserModel GetByUsername(string username)//obtener lista de usuarios de sistema
        {
            UserModel user = null;
            using (var connection = GetConnection())//mandamos a llamar la connecion de MySQL
            using (var command = new MySqlCommand())//creamos una instancia de MySQLCommand()
            {
                connection.Open();//abrimos la connecion con MySQL
                command.Connection = connection;//asignamos al comando la coneccion a mysql
                command.CommandText = "select *from users where username=@username"; //definimos nuestro query
                command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;//definimos parametro de username
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            Id = int.Parse(reader[0].ToString()),
                            Username = reader[1].ToString(),
                            Password = string.Empty,
                            FirstName = reader[3].ToString(),
                            LastName = reader[4].ToString(),
                        };
                    }
                }
            }
            return user;
        }
        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
