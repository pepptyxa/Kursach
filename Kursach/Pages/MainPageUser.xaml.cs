﻿using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Kursach.Pages
{
    public partial class MainPageUser : Page
    {
        public MainPageUser()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            if (MainWindow.init.CurrentUser != null)
            {
                tbCurrentUserName.Text = $"{MainWindow.init.CurrentUser.Surname} {MainWindow.init.CurrentUser.Name}";

                int userBookingsCount = MainWindow.init.AllBookings
                    .Count(b => b.IdUser == MainWindow.init.CurrentUser.Id);

                if (userBookingsCount > 0)
                {
                    tbUserBookingCount.Text = $"У вас {userBookingsCount} активных бронирований";
                }
                else
                {
                    tbUserBookingCount.Text = "У вас нет активных бронирований";
                }

                tbWelcomeMessage.Text = $"Добро пожаловать в систему управления отелем, {MainWindow.init.CurrentUser.Name}! " +
                    "Здесь вы можете просматривать доступные номера и бронировать их.";
            }
        }

        private void ProfileClick(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new Profile());
        }

        private void AvailableRoomsClick(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new AvailableRooms());
        }

        private void RentRoomClick(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new RentRoom());
        }

        private void LogoutClick(object sender, RoutedEventArgs e)
        {
            MainWindow.init.CurrentUser = null;
            MainWindow.init.OpenPage(new Authorization());
        }
    }
}
