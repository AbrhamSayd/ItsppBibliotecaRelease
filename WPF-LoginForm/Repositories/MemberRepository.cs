using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MySqlConnector;
using WPFBiblioteca.Helpers;
using WPFBiblioteca.Models;
using WPFBiblioteca.ViewModels.Fields;

namespace WPFBiblioteca.Repositories;

public class MemberRepository : RepositoryBase, IMemberRepository
{
    private string _errorCode;

    public async Task<string> Add(MemberModel member)
    {
        try
        {
            await using var connection = GetConnection();
            await using (var command = new MySqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText =
                    "INSERT INTO members(Member_Id, First_Name, Last_Name, Carrera, Email, Phone_Number) VALUES(@memberId, @firstName, @lastName, @carrera, @email, @phoneNumber)";
                command.Parameters.Add("@memberId", MySqlDbType.Int64).Value = member.MemberId;
                command.Parameters.Add("@firstName", MySqlDbType.VarChar).Value = member.FirstName;
                command.Parameters.Add("@lastName", MySqlDbType.VarChar).Value = member.LastName;
                command.Parameters.Add("@carrera", MySqlDbType.VarChar).Value = member.Carrera;
                command.Parameters.Add("@email", MySqlDbType.DateTime).Value = member.Email;
                command.Parameters.Add("@phoneNumber", MySqlDbType.Int64).Value = member.PhoneNumber;
                await command.ExecuteScalarAsync(CancellationToken.None);
                _errorCode = "400";
            }
        }
        catch (MySqlException e)
        {
            _errorCode = e.ToString();
            throw;
        }


        return _errorCode;
    }

    public async Task<string> Edit(MemberModel member, int memberId)
    {
        await using var connection = GetConnection();
        await using (var command = new MySqlCommand())
        {
            try
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText =
                    "UPDATE" +
                    " members" +
                    " SET" +
                    " Member_Id = @memberId," +
                    " First_Name = @firstName," +
                    " Last_Name = @lastName," +
                    " Carrera = @carrera," +
                    " Email = @email," +
                    " Phone_Number = @phoneNumber" +
                    " WHERE" +
                    " Member_Id = @dataId;";
                command.Parameters.Add("@dataId", MySqlDbType.Int64).Value = memberId;
                command.Parameters.Add("@memberId", MySqlDbType.Int64).Value = member.MemberId;
                command.Parameters.Add("@firstName", MySqlDbType.VarChar).Value = member.FirstName;
                command.Parameters.Add("@lastName", MySqlDbType.VarChar).Value = member.LastName;
                command.Parameters.Add("@carrera", MySqlDbType.VarChar).Value = member.Carrera;
                command.Parameters.Add("@email", MySqlDbType.DateTime).Value = member.Email;
                command.Parameters.Add("@phoneNumber", MySqlDbType.Int64).Value = member.PhoneNumber;
                command.ExecuteScalar();
                _errorCode = "400";
            }
            catch (Exception e)
            {
                _errorCode = e.ToString();
                throw;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        return _errorCode;
    }

    public async Task<string> Delete(int memberId)
    {
        await using var connection = GetConnection();
        await using (var command = new MySqlCommand())
        {
            try
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText =
                    "DELETE FROM members WHERE Member_Id = @memberId;";
                command.Parameters.Add("@memberId", MySqlDbType.Int64).Value = memberId;

                await command.ExecuteScalarAsync(CancellationToken.None);
                _errorCode = "400";
            }
            catch (Exception e)
            {
                _errorCode = e.ToString();
                throw;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        return _errorCode;
    }

    public async Task<MemberModel> GetById(int memberId)
    {
        MemberModel member = null;
        await using var connection = GetConnection();
        await using var command = new MySqlCommand();
        try
        {
            await connection.OpenAsync();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM members Where Member_Id = @memberId;";
            command.Parameters.Add("@memberId", MySqlDbType.Int64).Value = memberId;
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                member = new MemberModel
                {
                    MemberId = int.Parse(reader[0].ToString() ?? string.Empty),
                    FirstName = reader[1].ToString(),
                    LastName = reader[2].ToString(),
                    Carrera = reader[3].ToString(),
                    Email = reader[4].ToString(),
                    PhoneNumber = reader[5].ToString()
                };

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

        return member;
    }

    public async Task<MemberModel> GetByName(string firstName)
    {
        MemberModel member = null;
        await using var connection = GetConnection();
        await using var command = new MySqlCommand();
        try
        {
            await connection.OpenAsync();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM members Where FirstName = @firstName;";
            command.Parameters.Add("@firstName", MySqlDbType.VarChar).Value = firstName;
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                member = new MemberModel
                {
                    MemberId = int.Parse(reader[0].ToString() ?? string.Empty),
                    FirstName = reader[1].ToString(),
                    LastName = reader[2].ToString(),
                    Carrera = reader[3].ToString(),
                    Email = reader[4].ToString(),
                    PhoneNumber = reader[5].ToString(),
                    Deudor = reader.GetBoolean(6),
                    Prestamos = Convert.ToInt32(reader[7].ToString())
                };

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

        return member;
    }

    public async Task<IEnumerable<MemberModel>> GetByAll()
    {
        var memberList = new List<MemberModel>();
        await using var connection = GetConnection();
        await using var command = new MySqlCommand();
        try
        {
            await connection.OpenAsync();
            command.Connection = connection;
            command.CommandText = @"SELECT
            members.Member_Id,
            members.First_Name,
            members.Last_Name,
            members.Carrera,
            members.Email,
            members.Phone_Number,
            COUNT(lendings.Member_Id) AS Prestamos
            FROM members
            LEFT JOIN lendings ON members.Member_Id = lendings.Member_Id
            GROUP BY members.Member_Id;" ;
            await using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                var member = new MemberModel
                {
                    MemberId = int.Parse(reader[0].ToString() ?? string.Empty),
                    FirstName = reader[1].ToString(),
                    LastName = reader[2].ToString(),
                    Carrera = reader[3].ToString(),
                    Email = reader[4].ToString(),
                    PhoneNumber = reader[5].ToString(),
                    Deudor = ValidationHelper.TryConvert.ToInt32(reader[6].ToString(),0) > 0,
                    Prestamos = Convert.ToInt32(reader[6].ToString())
                };
                memberList.Add(member);
                _errorCode = "400";
            }
        }
        catch (Exception e)
        {
            _errorCode = e.ToString();
            throw;
        }


        return memberList;
    }

    public string GetError()
    {
        return _errorCode;
    }
}