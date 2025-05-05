using Kursach.Classes;
using System.Windows;
using System;
using System.Windows.Controls;

namespace Kursach.Pages
{
    public partial class RentRoom : Page
    {
        private RoomContext _selectedRoom;
        private int _totalCost = 0;
        public RentRoom()
        {
            InitializeComponent();
            InitializeControls();

            ShowRoomNotSelectedMessage();
        }
        public RentRoom(RoomContext selectedRoom)
        {
            InitializeComponent();
            InitializeControls();

            if (selectedRoom != null)
            {
                _selectedRoom = selectedRoom;
                ShowRoomInfo();
            }
            else
            {
                ShowRoomNotSelectedMessage();
            }
        }
        private void InitializeControls()
        {
            dpDateEntry.SelectedDate = DateTime.Today;
            dpDateDeparture.SelectedDate = DateTime.Today.AddDays(1);
            UpdateCostCalculation();
        }

        private void ShowRoomInfo()
        {
            if (_selectedRoom != null)
            {
                tbRoomName.Text = _selectedRoom.Name;
                tbRoomNumSeats.Text = _selectedRoom.NumSeats.ToString();
                tbRoomPrice.Text = $"{_selectedRoom.Price} руб.";
                tbRoomStatus.Text = _selectedRoom.Status ? "Доступен" : "Занят";
                tbNightPrice.Text = $"{_selectedRoom.Price} руб.";
                UpdateCostCalculation();
                roomInfoPanel.Visibility = Visibility.Visible;
                tbNoRoomSelected.Visibility = Visibility.Collapsed;
            }
            else
            {
                ShowRoomNotSelectedMessage();
            }
        }

        private void ShowRoomNotSelectedMessage()
        {
            roomInfoPanel.Visibility = Visibility.Collapsed;
            tbNoRoomSelected.Visibility = Visibility.Visible;
            tbNightPrice.Text = "0 руб.";
            UpdateCostCalculation();
        }

        private void UpdateCostCalculation()
        {
            if (dpDateEntry.SelectedDate.HasValue && dpDateDeparture.SelectedDate.HasValue)
            {
                if (dpDateDeparture.SelectedDate.Value <= dpDateEntry.SelectedDate.Value)
                {
                    tbDaysCount.Text = "1";

                    if (_selectedRoom != null)
                    {
                        _totalCost = _selectedRoom.Price;
                        tbTotalCost.Text = $"{_totalCost} руб.";
                    }
                    else
                    {
                        _totalCost = 0;
                        tbTotalCost.Text = "0 руб.";
                    }
                }
                else
                {
                    int daysCount = (dpDateDeparture.SelectedDate.Value - dpDateEntry.SelectedDate.Value).Days;
                    tbDaysCount.Text = daysCount.ToString();

                    if (_selectedRoom != null)
                    {
                        _totalCost = daysCount * _selectedRoom.Price;
                        tbTotalCost.Text = $"{_totalCost} руб.";
                    }
                    else
                    {
                        _totalCost = 0;
                        tbTotalCost.Text = "0 руб.";
                    }
                }
            }
            else
            {
                tbDaysCount.Text = "0";
                _totalCost = 0;
                tbTotalCost.Text = "0 руб.";
            }
        }

        private void DateEntryChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpDateEntry.SelectedDate.HasValue &&
                (!dpDateDeparture.SelectedDate.HasValue || dpDateDeparture.SelectedDate.Value <= dpDateEntry.SelectedDate.Value))
            {
                dpDateDeparture.SelectedDate = dpDateEntry.SelectedDate.Value.AddDays(1);
            }

            UpdateCostCalculation();
        }

        private void DateDepartureChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCostCalculation();
        }

        private void ChooseRoomClick(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new AvailableRooms());
        }

        private void BookRoomClick(object sender, RoutedEventArgs e)
        {
            tbError.Visibility = Visibility.Collapsed;
            tbSuccess.Visibility = Visibility.Collapsed;

            if (_selectedRoom == null)
            {
                tbError.Text = "Пожалуйста, выберите номер для бронирования";
                tbError.Visibility = Visibility.Visible;
                return;
            }

            if (!_selectedRoom.Status)
            {
                tbError.Text = "Выбранный номер недоступен для бронирования";
                tbError.Visibility = Visibility.Visible;
                return;
            }

            if (!dpDateEntry.SelectedDate.HasValue || !dpDateDeparture.SelectedDate.HasValue)
            {
                tbError.Text = "Пожалуйста, выберите даты заезда и выезда";
                tbError.Visibility = Visibility.Visible;
                return;
            }

            if (dpDateEntry.SelectedDate.Value < DateTime.Today)
            {
                tbError.Text = "Дата заезда не может быть раньше текущей даты";
                tbError.Visibility = Visibility.Visible;
                return;
            }

            if (dpDateDeparture.SelectedDate.Value <= dpDateEntry.SelectedDate.Value)
            {
                tbError.Text = "Дата выезда должна быть позже даты заезда";
                tbError.Visibility = Visibility.Visible;
                return;
            }

            BookingContext booking = new BookingContext
            {
                IdUser = MainWindow.init.CurrentUser.Id,
                IdRoom = _selectedRoom.Id,
                DateEntry = dpDateEntry.SelectedDate.Value,
                DateDeparture = dpDateDeparture.SelectedDate.Value,
                Cost = _totalCost
            };
            booking.Save();
            _selectedRoom.Status = false;
            _selectedRoom.Save(true);

            MainWindow.init.AllBookings = new BookingContext().AllBookings();
            MainWindow.init.AllRooms = new RoomContext().AllRooms();

            tbSuccess.Visibility = Visibility.Visible;
            btnBook.IsEnabled = false;
            tbRoomStatus.Text = "Занят";
            tbRoomStatus.Foreground = System.Windows.Media.Brushes.Red;
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new MainPageUser());
        }
    }
}
