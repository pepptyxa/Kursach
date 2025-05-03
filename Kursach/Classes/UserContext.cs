using Kursach.Classes.Common;
using Kursach.Interfaces;
using Kursach.Model;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Kursach.Classes
{
    public class UserContext : User, IUser
    {
        public List<UserContext> AllUsers() 
        {
            List<UserContext> allUsers = new List<UserContext>();
            MySqlConnection connection = DBConnection.Connection();
            MySqlDataReader dataUsers = DBConnection.Query("SELECT * FROM \"users\"",connection);
            while (dataUsers.Read())
            {
                UserContext user = new UserContext
                {
                    Id = dataUsers.GetInt32(0),
                    Surname = dataUsers.GetString(1),
                    Name = dataUsers.GetString(2),
                    Patronomyc = dataUsers.GetString(3),
                    Password = dataUsers.GetString(4),
                    PhoneNumber = dataUsers.GetString(5),
                    Role = dataUsers.GetBoolean(6)
                };
                allUsers.Add(user);
            }
            return allUsers;
        }
        public void Save(bool Update = false) 
        {
            MySqlConnection connection = DBConnection.Connection();
            if (Update)
            {
                DBConnection.Query("UPDATE \"users\" " +
                                              "SET " +
                                                "\"surname\" = @surname, " +
                                                "\"name\" = @name, " +
                                                "\"patronomyc\" = @patronomyc, " +
                                                "\"password\" = @password, " +
                                                "\"phoneNumber\" = @phoneNumber, " +
                                                "\"role\" = @role " +
                                              "WHERE \"id\" = @id", connection,
                        new MySqlParameter("@surname", this.Surname),
                        new MySqlParameter("@name", this.Name),
                        new MySqlParameter("@patronomyc", this.Patronomyc),
                        new MySqlParameter("@password", this.Password),
                        new MySqlParameter("@phoneNumber", this.PhoneNumber),
                        new MySqlParameter("@role", this.Role),
                        new MySqlParameter("@id", this.Id));
            }
            else
            {
                DBConnection.Query("INSERT INTO \"users\" " +
                          "(\"surname\", \"name\", \"patronomyc\", \"password\", \"phoneNumber\", \"role\") " +
                          "VALUES (@surname, @name, @patronomyc, @password, @phoneNumber, @role)", connection,
                                    new MySqlParameter("@surname", this.Surname),
                                    new MySqlParameter("@name", this.Name),
                                    new MySqlParameter("@patronomyc", this.Patronomyc),
                                    new MySqlParameter("@password", this.Password),
                                    new MySqlParameter("@phoneNumber", this.PhoneNumber),
                                    new MySqlParameter("@role", this.Role));
            }
        }
        public void Delete() 
        {
            MySqlConnection connection = DBConnection.Connection();
            DBConnection.Query("DELETE FROM \"users\" WHERE \"id\" = @id", connection, new MySqlParameter("@id", this.Id));
        }
    }
}
