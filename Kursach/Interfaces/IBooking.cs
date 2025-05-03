using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursach.Interfaces
{
    public interface IBooking
    {
        void Save(bool Update = false);
        List<Classes.BookingContext> AllBookings();
        void Delete();
    }
}
