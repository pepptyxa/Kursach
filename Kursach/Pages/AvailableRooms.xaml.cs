using Kursach.Classes;
using Kursach.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kursach.Pages
{
    public partial class AvailableRooms : Page
    {
        private List<RoomContext> allRooms;
        public AvailableRooms()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            roomsContainer.Children.Clear();
            roomsContainer.Children.Add(tbEmptyMessage);
            MainWindow.init.AllRooms = new RoomContext().AllRooms();
            allRooms = MainWindow.init.AllRooms;

            ApplyFilters();
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new MainPageUser());
        }

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void NumSeatsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ClearFiltersClick(object sender, RoutedEventArgs e)
        {
            tbSearch.Text = string.Empty;
            cbNumSeats.SelectedIndex = 0;
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (allRooms == null)
                return;

            roomsContainer.Children.Clear();
            roomsContainer.Children.Add(tbEmptyMessage);

            string searchText = tbSearch.Text?.ToLower() ?? string.Empty;

            int? numSeatsFilter = null;
            if (cbNumSeats.SelectedIndex > 0)
            {
                switch (cbNumSeats.SelectedIndex)
                {
                    case 1: numSeatsFilter = 1; break;     
                    case 2: numSeatsFilter = 2; break;     
                    case 3: numSeatsFilter = 3; break;     
                    case 4: numSeatsFilter = 4; break;     
                }
            }

            var filteredRooms = allRooms.Where(r => r.Status && (string.IsNullOrEmpty(searchText) ||
                r.Id.ToString().Contains(searchText) ||
                r.Name.ToLower().Contains(searchText) ||
                r.NumSeats.ToString().Contains(searchText) ||
                r.Price.ToString().Contains(searchText)) &&
                (!numSeatsFilter.HasValue ||
                 (numSeatsFilter.Value < 4 && r.NumSeats == numSeatsFilter.Value) ||
                 (numSeatsFilter.Value == 4 && r.NumSeats >= 4))
            ).ToList();

            if (filteredRooms.Count > 0)
            {
                tbEmptyMessage.Visibility = Visibility.Collapsed;

                foreach (var room in filteredRooms)
                {
                    AvailableRoomItem roomItem = new AvailableRoomItem(room);
                    roomsContainer.Children.Add(roomItem);
                }
            }
            else
            {
                tbEmptyMessage.Visibility = Visibility.Visible;
            }
        }
    }
}
