using Kursach.Classes.Common;
using Kursach.Interfaces;
using Kursach.Model;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Kursach.Classes
{
    public class BookingContext : Booking, IBooking
    {
        public List<BookingContext> AllBookings()
        {
            List<BookingContext> allBookings = new List<BookingContext>();
            MySqlConnection connection = DBConnection.Connection();
            MySqlDataReader dataBookings = DBConnection.Query("SELECT * FROM \"bookings\"", connection);
            while (dataBookings.Read())
            {
                BookingContext booking = new BookingContext
                {
                    Id = dataBookings.GetInt32(0),
                    IdUser = dataBookings.GetInt32(1),
                    IdRoom = dataBookings.GetInt32(2),
                    DateEntry = dataBookings.GetDateTime(3),
                    DateDeparture = dataBookings.GetDateTime(4),
                    Cost = dataBookings.GetInt32(5),
                };
                allBookings.Add(booking);
            }
            return allBookings;
        }

        public void Save(bool Update = false) 
        {
            MySqlConnection connection = DBConnection.Connection();
            if (Update)
            {
                DBConnection.Query("UPDATE \"bookings\" " +
                                              "SET " +
                                                "\"idUser\" = @idUser, " +
                                                "\"idRoom\" = @idRoom, " +
                                                "\"dateEntry\" = @dateEntry, " +
                                                "\"dateDeparture\" = @dateDeparture, " +
                                                "\"cost\" = @cost " +
                                              "WHERE \"id\" = @id", connection,
                        new MySqlParameter("@idUser", this.IdUser),
                        new MySqlParameter("@idRoom", this.IdRoom),
                        new MySqlParameter("@dateEntry", this.DateEntry),
                        new MySqlParameter("@dateDeparture", this.DateDeparture),
                        new MySqlParameter("@cost", this.Cost),
                        new MySqlParameter("@id", this.Id));
            }
            else
            {
                DBConnection.Query("INSERT INTO \"users\" " +
                          "(\"idUser\", \"idRoom\", \"dateEntry\", \"dateDeparture\", \"cost\") " +
                          "VALUES (@idUser, @idRoom, @dateEntry, @dateDeparture, @cost)", connection,
                                    new MySqlParameter("@idUser", this.IdUser),
                                    new MySqlParameter("@idRoom", this.IdRoom),
                                    new MySqlParameter("@dateEntry", this.DateEntry),
                                    new MySqlParameter("@dateDeparture", this.DateDeparture),
                                    new MySqlParameter("@cost", this.Cost));
            }
        }

        public void Delete()
        {
            MySqlConnection connection = DBConnection.Connection();
            DBConnection.Query("DELETE FROM \"bookings\" WHERE \"id\" = @id", connection, new MySqlParameter("@id", this.Id));
        }
    }
}
