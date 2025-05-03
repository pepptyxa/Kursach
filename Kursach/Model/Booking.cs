using System;

namespace Kursach.Model
{
    public class Booking
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdRoom { get; set; }
        public DateTime DateEntry { get; set; }
        public DateTime DateDeparture { get; set; }
        public int Cost { get; set; }
    }
}
