using Kursach.Classes;
using Kursach.Elements;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Kursach.Pages
{
    public partial class Users : Page
    {
        private List<UserContext> allUsers;
        public Users()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            parent.Children.Clear();
            parent.Children.Add(tbEmptyMessage);
            MainWindow.init.AllUsers = new UserContext().AllUsers();
            allUsers = MainWindow.init.AllUsers;
            ApplyFilters();
        }

        private void AddUserClick(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new AddUsers());
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void Role_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            tbSearch.Text = string.Empty;
            cbRole.SelectedIndex = 0;
            LoadData();
        }

        private void ApplyFilters()
        {
            if (allUsers == null)
                return;

            parent.Children.Clear();
            parent.Children.Add(tbEmptyMessage);

            string searchText = tbSearch.Text?.ToLower() ?? string.Empty;

            bool? roleFilter = null;
            if (cbRole.SelectedIndex == 1)
                roleFilter = true;
            else if (cbRole.SelectedIndex == 2)
                roleFilter = false;

            var filteredUsers = allUsers.Where(u =>
                (string.IsNullOrEmpty(searchText) ||
                u.Id.ToString().Contains(searchText) ||
                u.Name.ToLower().Contains(searchText) ||
                u.Surname.ToLower().Contains(searchText) ||
                u.Patronomyc.ToLower().Contains(searchText) ||
                u.PhoneNumber.ToLower().Contains(searchText)) &&
                (!roleFilter.HasValue || u.Role == roleFilter.Value)
            ).ToList();

            if (filteredUsers.Count > 0)
            {
                tbEmptyMessage.Visibility = Visibility.Collapsed;

                foreach (var user in filteredUsers)
                {
                    UserItem userItem = new UserItem(user);
                    parent.Children.Add(userItem);
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
