using Kursach.Classes;
using Kursach.Model;
using System.Windows.Controls;

namespace Kursach.Elements
{
    public partial class BookingItem : UserControl
    {
        BookingContext Booking;
        public BookingItem(BookingContext Booking)
        {
            InitializeComponent();
            BookingContext bookingContext = new BookingContext();
            lId.Content = $"Код: {Booking.Id}";
            lIdUser.Content = $"Код пользователя: {Booking.IdUser}";
            lIdRoom.Content = $"Код комнаты: {Booking.IdRoom}";
            lDateEntry.Content = $"Дата въезда: {Booking.DateEntry}";
            lDateDeparture.Content = $"Дата выезда: {Booking.DateDeparture}";
            lCost.Content = $"Стоимость: {Booking.Cost} руб.";
            this.Booking = Booking;
        }
        private void Edit(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new Pages.AddBookings(Booking));
        }

        private void Delete(object sender, System.Windows.RoutedEventArgs e)
        {
            Booking.Delete();
            MainWindow.init.AllBookings = new BookingContext().AllBookings();
            MainWindow.init.OpenPage(new Pages.Bookings());
        }
    }
}
