using Kursach.Classes;
using Kursach.Elements;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Kursach.Pages
{
    public partial class Rooms : Page
    {
        private List<RoomContext> allRooms;
        public Rooms()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            parent.Children.Clear();
            parent.Children.Add(tbEmptyMessage);
            MainWindow.init.AllRooms = new RoomContext().AllRooms();
            allRooms = MainWindow.init.AllRooms;
            ApplyFilters();
        }

        private void AddRoomClick(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new AddRooms());
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            tbSearch.Text = string.Empty;
            cbStatus.SelectedIndex = 0;
            LoadData();
        }

        private void ApplyFilters()
        {
            if (allRooms == null)
                return;

            parent.Children.Clear();
            parent.Children.Add(tbEmptyMessage);

            string searchText = tbSearch.Text?.ToLower() ?? string.Empty;

            bool? statusFilter = null;
            if (cbStatus.SelectedIndex == 1)
                statusFilter = true;
            else if (cbStatus.SelectedIndex == 2)
                statusFilter = false;

            var filteredRooms = allRooms.Where(r =>
                (string.IsNullOrEmpty(searchText) ||
                r.Id.ToString().Contains(searchText) ||
                r.Name.ToLower().Contains(searchText) ||
                r.NumSeats.ToString().Contains(searchText) ||
                r.Price.ToString().Contains(searchText)) &&
                (!statusFilter.HasValue || r.Status == statusFilter.Value)
            ).ToList();

            if (filteredRooms.Count > 0)
            {
                tbEmptyMessage.Visibility = Visibility.Collapsed;

                foreach (var room in filteredRooms)
                {
                    RoomItem roomItem = new RoomItem(room);
                    parent.Children.Add(roomItem);
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
