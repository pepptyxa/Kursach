using Kursach.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Kursach.Pages
{
    public partial class AddBookings : Page
    {
        private bool isEditMode = false;
        private BookingContext currentBooking = null;
        private List<UserContext> users;
        private List<RoomContext> rooms;
        private int totalDays = 0;
        private int pricePerDay = 0;
        private class ComboBoxItem<T>
        {
            public T Item { get; set; }
            public string DisplayName { get; set; }
        }
        public AddBookings()
        {
            InitializeComponent();
            tbPageTitle.Text = "Добавление бронирования";
            LoadData();
        }

        public AddBookings(BookingContext booking)
        {
            InitializeComponent();
            tbPageTitle.Text = "Редактирование бронирования";
            isEditMode = true;
            currentBooking = booking;
            LoadData();
            SetValuesFromBooking();
        }

        private void LoadData()
        {
            UserContext userContext = new UserContext();
            users = userContext.AllUsers();

            RoomContext roomContext = new RoomContext();
            rooms = roomContext.AllRooms();

            List<ComboBoxItem<UserContext>> userItems = new List<ComboBoxItem<UserContext>>();
            foreach (var user in users)
            {
                userItems.Add(new ComboBoxItem<UserContext>
                {
                    Item = user,
                    DisplayName = $"{user.Id} - {user.Surname} {user.Name} {user.Patronomyc}"
                });
            }
            cbUsers.ItemsSource = userItems;

            List<ComboBoxItem<RoomContext>> roomItems = new List<ComboBoxItem<RoomContext>>();
            foreach (var room in rooms.Where(r => r.Status))
            {
                roomItems.Add(new ComboBoxItem<RoomContext>
                {
                    Item = room,
                    DisplayName = $"{room.Id} - {room.Name} ({room.NumSeats} мест, {room.Price} руб./ночь)"
                });
            }
            cbRooms.ItemsSource = roomItems;

            dpDateEntry.SelectedDate = DateTime.Today;
            dpDateDeparture.SelectedDate = DateTime.Today.AddDays(1);
            CalculateCost();
        }

        private void SetValuesFromBooking()
        {
            var selectedUser = users.FirstOrDefault(u => u.Id == currentBooking.IdUser);
            if (selectedUser != null)
            {
                var userItems = cbUsers.ItemsSource as List<ComboBoxItem<UserContext>>;
                var selectedUserItem = userItems.FirstOrDefault(ui => ((ComboBoxItem<UserContext>)ui).Item.Id == selectedUser.Id);
                cbUsers.SelectedItem = selectedUserItem;
            }

            var selectedRoom = rooms.FirstOrDefault(r => r.Id == currentBooking.IdRoom);
            if (selectedRoom != null)
            {
                var roomItems = cbRooms.ItemsSource as List<ComboBoxItem<RoomContext>>;
                var selectedRoomItem = roomItems.FirstOrDefault(ri => ((ComboBoxItem<RoomContext>)ri).Item.Id == selectedRoom.Id);

                if (selectedRoomItem == null)
                {
                    var newRoomItems = new List<ComboBoxItem<RoomContext>>(roomItems);
                    selectedRoomItem = new ComboBoxItem<RoomContext>
                    {
                        Item = selectedRoom,
                        DisplayName = $"{selectedRoom.Id} - {selectedRoom.Name} ({selectedRoom.NumSeats} мест, {selectedRoom.Price} руб./ночь)"
                    };
                    newRoomItems.Add(selectedRoomItem);
                    cbRooms.ItemsSource = newRoomItems;
                }

                cbRooms.SelectedItem = selectedRoomItem;
            }
            dpDateEntry.SelectedDate = currentBooking.DateEntry;
            dpDateDeparture.SelectedDate = currentBooking.DateDeparture;
        }

        private void CbUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbUsersError.Visibility = Visibility.Collapsed;
        }

        private void CbRooms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbRoomsError.Visibility = Visibility.Collapsed;
            CalculateCost();
        }

        private void DpDateEntry_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            tbDateEntryError.Visibility = Visibility.Collapsed;
            if (dpDateEntry.SelectedDate.HasValue)
            {
                DateTime minDeparture = dpDateEntry.SelectedDate.Value.AddDays(1);
                if (dpDateDeparture.SelectedDate.HasValue && dpDateDeparture.SelectedDate < minDeparture)
                {
                    dpDateDeparture.SelectedDate = minDeparture;
                }
            }
            CalculateCost();
        }

        private void DpDateDeparture_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            tbDateDepartureError.Visibility = Visibility.Collapsed;
            CalculateCost();
        }

        private void CalculateCost()
        {
            pricePerDay = 0;
            totalDays = 0;

            if (cbRooms.SelectedItem is ComboBoxItem<RoomContext> selectedRoomItem)
            {
                pricePerDay = selectedRoomItem.Item.Price;
            }

            if (dpDateEntry.SelectedDate.HasValue && dpDateDeparture.SelectedDate.HasValue)
            {
                totalDays = (dpDateDeparture.SelectedDate.Value - dpDateEntry.SelectedDate.Value).Days;
            }

            int cost = pricePerDay * totalDays;
            tbCost.Text = cost.ToString();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            ResetErrors();
            bool isValid = true;

            if (cbUsers.SelectedItem == null)
            {
                ShowError(tbUsersError, "Необходимо выбрать пользователя");
                isValid = false;
            }

            if (cbRooms.SelectedItem == null)
            {
                ShowError(tbRoomsError, "Необходимо выбрать номер");
                isValid = false;
            }

            if (!dpDateEntry.SelectedDate.HasValue)
            {
                ShowError(tbDateEntryError, "Необходимо выбрать дату заезда");
                isValid = false;
            }
            else if (dpDateEntry.SelectedDate < DateTime.Today)
            {
                ShowError(tbDateEntryError, "Дата заезда не может быть в прошлом");
                isValid = false;
            }

            if (!dpDateDeparture.SelectedDate.HasValue)
            {
                ShowError(tbDateDepartureError, "Необходимо выбрать дату выезда");
                isValid = false;
            }
            else if (dpDateEntry.SelectedDate.HasValue && dpDateDeparture.SelectedDate <= dpDateEntry.SelectedDate)
            {
                ShowError(tbDateDepartureError, "Дата выезда должна быть позже даты заезда");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(tbCost.Text))
            {
                ShowError(tbCostError, "Не удалось рассчитать стоимость");
                isValid = false;
            }
            else if (!int.TryParse(tbCost.Text, out int cost) || cost <= 0)
            {
                ShowError(tbCostError, "Стоимость должна быть положительным числом");
                isValid = false;
            }

            if (isValid && cbRooms.SelectedItem is ComboBoxItem<RoomContext> selectedRoomItem)
            {
                var selectedRoom = selectedRoomItem.Item;

                if (!isEditMode || (isEditMode && currentBooking.IdRoom != selectedRoom.Id))
                {
                    if (!selectedRoom.Status)
                    {
                        ShowError(tbRoomsError, "Выбранный номер недоступен для бронирования");
                        isValid = false;
                    }
                }

                if (isValid)
                {
                    BookingContext bookingContext = new BookingContext();
                    var allBookings = bookingContext.AllBookings();

                    var overlappingBookings = allBookings.Where(b =>
                        b.IdRoom == selectedRoom.Id &&
                        (isEditMode ? b.Id != currentBooking.Id : true) &&  
                        ((dpDateEntry.SelectedDate >= b.DateEntry && dpDateEntry.SelectedDate < b.DateDeparture) ||
                         (dpDateDeparture.SelectedDate > b.DateEntry && dpDateDeparture.SelectedDate <= b.DateDeparture) ||
                         (dpDateEntry.SelectedDate <= b.DateEntry && dpDateDeparture.SelectedDate >= b.DateDeparture))
                    ).ToList();

                    if (overlappingBookings.Count > 0)
                    {
                        ShowError(tbRoomsError, "Номер уже забронирован на выбранные даты");
                        isValid = false;
                    }
                }
            }

            if (isValid)
            {
                var selectedUser = ((ComboBoxItem<UserContext>)cbUsers.SelectedItem).Item;
                var selectedRoom = ((ComboBoxItem<RoomContext>)cbRooms.SelectedItem).Item;
                var dateEntry = dpDateEntry.SelectedDate.Value;
                var dateDeparture = dpDateDeparture.SelectedDate.Value;
                var cost = int.Parse(tbCost.Text);

                BookingContext booking;
                if (isEditMode)
                {
                    booking = currentBooking;
                    booking.IdUser = selectedUser.Id;
                    booking.IdRoom = selectedRoom.Id;
                    booking.DateEntry = dateEntry;
                    booking.DateDeparture = dateDeparture;
                    booking.Cost = cost;

                    booking.Save(true);
                    MessageBox.Show("Бронирование успешно отредактировано!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    booking = new BookingContext
                    {
                        IdUser = selectedUser.Id,
                        IdRoom = selectedRoom.Id,
                        DateEntry = dateEntry,
                        DateDeparture = dateDeparture,
                        Cost = cost
                    };

                    booking.Save();
                    MessageBox.Show("Бронирование успешно добавлено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                selectedRoom.Status = false;
                selectedRoom.Save(true);

                MainWindow.init.AllBookings = new BookingContext().AllBookings();
                MainWindow.init.OpenPage(new Bookings());
            }
        }

        private void ShowError(TextBlock errorTextBlock, string errorMessage)
        {
            errorTextBlock.Text = errorMessage;
            errorTextBlock.Visibility = Visibility.Visible;
        }

        private void ResetErrors()
        {
            tbUsersError.Visibility = Visibility.Collapsed;
            tbRoomsError.Visibility = Visibility.Collapsed;
            tbDateEntryError.Visibility = Visibility.Collapsed;
            tbDateDepartureError.Visibility = Visibility.Collapsed;
            tbCostError.Visibility = Visibility.Collapsed;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigateBack();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigateBack();
        }

        private void NavigateBack()
        {
            MainWindow.init.OpenPage(new Bookings());
        }
    }
}
