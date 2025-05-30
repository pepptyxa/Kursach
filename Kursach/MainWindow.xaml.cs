﻿using Kursach.Classes;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Kursach
{
    public partial class MainWindow : Window
    {
        public static MainWindow init;
        public List<UserContext> AllUsers = new UserContext().AllUsers();
        public List<RoomContext> AllRooms = new RoomContext().AllRooms();
        public List<BookingContext> AllBookings = new BookingContext().AllBookings();
        public UserContext CurrentUser { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            OpenPage(new Pages.Authorization());
            init = this;
        }
        public void OpenPage(Page Page)
        {
            frame.Navigate(Page);
        }
    }
}
