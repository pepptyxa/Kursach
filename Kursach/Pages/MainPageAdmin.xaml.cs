using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class MainPageAdmin : Page
    {
        public MainPageAdmin()
        {
            InitializeComponent();
            LoadData(); 
        }

        private void LoadData()
        {
            try
            {
                if (MainWindow.init.CurrentUser != null)
                {
                    tbCurrentUserName.Text = $"{MainWindow.init.CurrentUser.Surname} {MainWindow.init.CurrentUser.Name}";
                }

                int userCount = MainWindow.init.AllUsers.Count;
                int roomCount = MainWindow.init.AllRooms.Count;
                int bookingCount = MainWindow.init.AllBookings.Count;

                tbUserCount.Text = $"Всего: {userCount}";
                tbRoomCount.Text = $"Всего: {roomCount}";
                tbBookingCount.Text = $"Всего: {bookingCount}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UsersClick(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new Users());
        }

        private void RoomsClick(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new Rooms());
        }

        private void BookingsClick(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new Bookings());
        }

        private void ExportExcelClick(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel файлы (*.csv)|*.csv",
                    FileName = "Отчет_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm")
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    ExportToCSV(filePath);
                    MessageBox.Show("Отчет успешно создан!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportToCSV(string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                // Заголовок отчета
                sw.WriteLine("ОТЧЕТ ПО СИСТЕМЕ УПРАВЛЕНИЯ ОТЕЛЕМ");
                sw.WriteLine($"Дата создания: {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}");
                sw.WriteLine("-----------------------------------------------------");

                // Экспорт информации о пользователях
                sw.WriteLine("\nСПИСОК ПОЛЬЗОВАТЕЛЕЙ:");
                sw.WriteLine("ID;Фамилия;Имя;Отчество;Телефон;Роль");

                foreach (var user in MainWindow.init.AllUsers)
                {
                    string role = user.Role ? "Администратор" : "Пользователь";
                    sw.WriteLine($"{user.Id};{user.Surname};{user.Name};{user.Patronomyc};{user.PhoneNumber};{role}");
                }

                // Экспорт информации о номерах
                sw.WriteLine("\nСПИСОК НОМЕРОВ:");
                sw.WriteLine("ID;Название;Кол-во мест;Цена за ночь;Статус");

                foreach (var room in MainWindow.init.AllRooms)
                {
                    string status = room.Status ? "Доступен" : "Занят";
                    sw.WriteLine($"{room.Id};{room.Name};{room.NumSeats};{room.Price};{status}");
                }

                // Экспорт информации о бронированиях
                sw.WriteLine("\nСПИСОК БРОНИРОВАНИЙ:");
                sw.WriteLine("ID;ID Пользователя;ID Комнаты;Дата заезда;Дата выезда;Стоимость");

                foreach (var booking in MainWindow.init.AllBookings)
                {
                    sw.WriteLine($"{booking.Id};{booking.IdUser};{booking.IdRoom};{booking.DateEntry.ToShortDateString()};{booking.DateDeparture.ToShortDateString()};{booking.Cost}");
                }

                // Статистика
                sw.WriteLine("\nСТАТИСТИКА:");
                sw.WriteLine($"Всего пользователей: {MainWindow.init.AllUsers.Count}");
                sw.WriteLine($"Всего номеров: {MainWindow.init.AllRooms.Count}");
                sw.WriteLine($"Всего бронирований: {MainWindow.init.AllBookings.Count}");

                int totalIncome = MainWindow.init.AllBookings.Sum(b => b.Cost);
                sw.WriteLine($"Общая сумма бронирований: {totalIncome} руб.");
            }
        }

        private void LogoutClick(object sender, RoutedEventArgs e)
        {
            MainWindow.init.CurrentUser = null;
            MainWindow.init.OpenPage(new Authorization());
        }
    }
}
