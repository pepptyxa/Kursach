using Kursach.Classes.Common;
using Kursach.Interfaces;
using Kursach.Model;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Kursach.Classes
{
    public class RoomContext : Room, IRoom
    {
        public List<RoomContext> AllRooms()
        {
            List<RoomContext> allRooms = new List<RoomContext>();
            MySqlConnection connection = DBConnection.Connection();
            MySqlDataReader dataRooms = DBConnection.Query("SELECT * FROM \"rooms\"", connection);
            while (dataRooms.Read())
            {
                RoomContext room = new RoomContext
                {
                    Id = dataRooms.GetInt32(0),
                    Name = dataRooms.GetString(1),
                    NumSeats = dataRooms.GetInt32(2),
                    Price = dataRooms.GetInt32(3),
                    Status = dataRooms.GetBoolean(4)
                };
                allRooms.Add(room);
            }
            return allRooms;
        }
        public void Save(bool Update = false) 
        {
            MySqlConnection connection = DBConnection.Connection();
            if (Update)
            {
                DBConnection.Query("UPDATE \"rooms\" " +
                                              "SET " +
                                                "\"name\" = @name, " +
                                                "\"numSeats\" = @numSeats, " +
                                                "\"price\" = @price, " +
                                                "\"status\" = @status " +
                                              "WHERE \"id\" = @id", connection,
                        new MySqlParameter("@name", this.Name),
                        new MySqlParameter("@numSeats", this.NumSeats),
                        new MySqlParameter("@price", this.Price),
                        new MySqlParameter("@status", this.Status),
                        new MySqlParameter("@id", this.Id));
            }
            else
            {
                DBConnection.Query("INSERT INTO \"rooms\" " +
                          "(\"name\", \"numSeats\", \"price\", \"status\") " +
                          "VALUES (@name, @numSeats, @price, @status)", connection,
                                    new MySqlParameter("@name", this.Name),
                                    new MySqlParameter("@numSeats", this.NumSeats),
                                    new MySqlParameter("@price", this.Price),
                                    new MySqlParameter("@status", this.Status));
            }
        }

        public void Delete()
        {
            MySqlConnection connection = DBConnection.Connection();
            DBConnection.Query("DELETE FROM \"rooms\" WHERE \"id\" = @id", connection, new MySqlParameter("@id", this.Id));
        }
    }
}
