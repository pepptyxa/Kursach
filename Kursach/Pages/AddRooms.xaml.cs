using Kursach.Classes;
using System.Windows;
using System.Windows.Controls;

namespace Kursach.Pages
{
    public partial class AddRooms : Page
    {
        private bool isEditMode = false;
        private RoomContext currentRoom = null;
        public AddRooms()
        {
            InitializeComponent();
            tbPageTitle.Text = "Добавление номера";
        }

        public AddRooms(RoomContext room)
        {
            InitializeComponent();
            tbPageTitle.Text = "Редактирование номера";
            isEditMode = true;
            currentRoom = room;

            tbName.Text = room.Name;
            tbNumSeats.Text = room.NumSeats.ToString();
            tbPrice.Text = room.Price.ToString();
            chbStatus.IsChecked = room.Status;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            ResetErrors();
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                ShowError(tbNameError, "Название номера обязательно для заполнения");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(tbNumSeats.Text))
            {
                ShowError(tbNumSeatsError, "Количество мест обязательно для заполнения");
                isValid = false;
            }
            else if (!int.TryParse(tbNumSeats.Text, out int numSeats) || numSeats <= 0)
            {
                ShowError(tbNumSeatsError, "Количество мест должно быть положительным числом");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(tbPrice.Text))
            {
                ShowError(tbPriceError, "Цена за ночь обязательна для заполнения");
                isValid = false;
            }
            else if (!int.TryParse(tbPrice.Text, out int price) || price <= 0)
            {
                ShowError(tbPriceError, "Цена должна быть положительным числом");
                isValid = false;
            }

            if (isValid)
            {
                RoomContext room;
                int.TryParse(tbNumSeats.Text, out int numSeats);
                int.TryParse(tbPrice.Text, out int price);

                if (isEditMode)
                {
                    room = currentRoom;
                    room.Name = tbName.Text;
                    room.NumSeats = numSeats;
                    room.Price = price;
                    room.Status = chbStatus.IsChecked ?? false;

                    room.Save(true);
                    MessageBox.Show("Номер успешно отредактирован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    room = new RoomContext
                    {
                        Name = tbName.Text,
                        NumSeats = numSeats,
                        Price = price,
                        Status = chbStatus.IsChecked ?? false
                    };

                    room.Save();
                    MessageBox.Show("Номер успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                MainWindow.init.AllRooms = new RoomContext().AllRooms();
                MainWindow.init.OpenPage(new Rooms());
            }
        }

        private void ShowError(TextBlock errorTextBlock, string errorMessage)
        {
            errorTextBlock.Text = errorMessage;
            errorTextBlock.Visibility = Visibility.Visible;
        }

        private void ResetErrors()
        {
            tbNameError.Visibility = Visibility.Collapsed;
            tbNumSeatsError.Visibility = Visibility.Collapsed;
            tbPriceError.Visibility = Visibility.Collapsed;
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
            MainWindow.init.OpenPage(new Rooms());
        }
    }
}
