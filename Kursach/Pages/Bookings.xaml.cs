using Kursach.Classes;
using Kursach.Elements;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Kursach.Pages
{
    public partial class Bookings : Page
    {
        private List<BookingContext> allBookings;
        public Bookings()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            parent.Children.Clear();
            parent.Children.Add(tbEmptyMessage);
            MainWindow.init.AllBookings = new BookingContext().AllBookings();
            allBookings = MainWindow.init.AllBookings;

            if (allBookings != null && allBookings.Count > 0)
            {
                tbEmptyMessage.Visibility = Visibility.Collapsed;

                foreach (var booking in allBookings)
                {
                    BookingItem bookingItem = new BookingItem(booking);
                    parent.Children.Add(bookingItem);
                }
            }
            else
            {
                tbEmptyMessage.Visibility = Visibility.Visible;
            }
        }

        private void AddBookingClick(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new AddBookings());
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilter();
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            tbSearch.Text = string.Empty;
            LoadData();
        }

        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(tbSearch.Text))
            {
                LoadData();
                return;
            }
            parent.Children.Clear();
            parent.Children.Add(tbEmptyMessage);

            string searchText = tbSearch.Text.ToLower();
            var filteredBookings = allBookings.Where(b =>
                b.Id.ToString().Contains(searchText) ||
                b.IdUser.ToString().Contains(searchText) ||
                b.IdRoom.ToString().Contains(searchText) ||
                b.DateEntry.ToString("dd.MM.yyyy").Contains(searchText) ||
                b.DateDeparture.ToString("dd.MM.yyyy").Contains(searchText) ||
                b.Cost.ToString().Contains(searchText)
            ).ToList();

            if (filteredBookings.Count > 0)
            {
                tbEmptyMessage.Visibility = Visibility.Collapsed;
                foreach (var booking in filteredBookings)
                {
                    BookingItem bookingItem = new BookingItem(booking);
                    parent.Children.Add(bookingItem);
                }
            }
            else
            {
                tbEmptyMessage.Visibility = Visibility.Visible;
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new MainPageAdmin());
        }
    }
}
