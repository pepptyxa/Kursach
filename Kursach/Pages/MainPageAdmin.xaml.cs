using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;

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
                    Filter = "Excel файлы (*.xlsx)|*.xlsx",
                    FileName = "Отчет_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm")
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    ExportToExcel(filePath);
                    MessageBox.Show("Отчет успешно создан!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportToExcel(string filePath)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;

            excelApp = new Excel.Application();
            excelApp.Visible = false;
            workbook = excelApp.Workbooks.Add();

            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets[1];
            worksheet.Name = "Отчет по системе";

            worksheet.Cells[1, 1] = "ОТЧЕТ ПО СИСТЕМЕ УПРАВЛЕНИЯ ОТЕЛЕМ";
            Excel.Range headerRange = worksheet.Range["A1:F1"];
            headerRange.Merge();
            headerRange.Font.Bold = true;
            headerRange.Font.Size = 14;
            headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            headerRange.Interior.Color = Excel.XlRgbColor.rgbLavender;

            worksheet.Cells[2, 1] = $"Дата создания: {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}";
            Excel.Range dateRange = worksheet.Range["A2:F2"];
            dateRange.Merge();
            dateRange.Font.Italic = true;
            dateRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            worksheet.Cells[3, 1] = "-----------------------------------------------------";
            Excel.Range separatorRange = worksheet.Range["A3:F3"];
            separatorRange.Merge();
            separatorRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            int currentRow = 5;
            worksheet.Cells[currentRow, 1] = "СПИСОК ПОЛЬЗОВАТЕЛЕЙ:";
            Excel.Range usersTitleRange = worksheet.Range[$"A{currentRow}:F{currentRow}"];
            usersTitleRange.Merge();
            usersTitleRange.Font.Bold = true;
            usersTitleRange.Interior.Color = Excel.XlRgbColor.rgbPurple;

            currentRow++;
            worksheet.Cells[currentRow, 1] = "ID";
            worksheet.Cells[currentRow, 2] = "Фамилия";
            worksheet.Cells[currentRow, 3] = "Имя";
            worksheet.Cells[currentRow, 4] = "Отчество";
            worksheet.Cells[currentRow, 5] = "Телефон";
            worksheet.Cells[currentRow, 6] = "Роль";

            Excel.Range usersHeaderRange = worksheet.Range[$"A{currentRow}:F{currentRow}"];
            usersHeaderRange.Font.Bold = true;
            usersHeaderRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            usersHeaderRange.Interior.Color = Excel.XlRgbColor.rgbLightGray;

            foreach (var user in MainWindow.init.AllUsers)
            {
                currentRow++;
                worksheet.Cells[currentRow, 1] = user.Id;
                worksheet.Cells[currentRow, 2] = user.Surname;
                worksheet.Cells[currentRow, 3] = user.Name;
                worksheet.Cells[currentRow, 4] = user.Patronomyc;
                worksheet.Cells[currentRow, 5] = user.PhoneNumber;
                worksheet.Cells[currentRow, 6] = user.Role ? "Администратор" : "Пользователь";

                Excel.Range userDataRange = worksheet.Range[$"A{currentRow}:F{currentRow}"];
                userDataRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            }

            currentRow += 2;
            worksheet.Cells[currentRow, 1] = "СПИСОК НОМЕРОВ:";
            Excel.Range roomsTitleRange = worksheet.Range[$"A{currentRow}:F{currentRow}"];
            roomsTitleRange.Merge();
            roomsTitleRange.Font.Bold = true;
            roomsTitleRange.Interior.Color = Excel.XlRgbColor.rgbLightGreen;

            currentRow++;
            worksheet.Cells[currentRow, 1] = "ID";
            worksheet.Cells[currentRow, 2] = "Название";
            worksheet.Cells[currentRow, 3] = "Кол-во мест";
            worksheet.Cells[currentRow, 4] = "Цена за ночь";
            worksheet.Cells[currentRow, 5] = "Статус";

            Excel.Range roomsHeaderRange = worksheet.Range[$"A{currentRow}:E{currentRow}"];
            roomsHeaderRange.Font.Bold = true;
            roomsHeaderRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            roomsHeaderRange.Interior.Color = Excel.XlRgbColor.rgbLightGray;

            foreach (var room in MainWindow.init.AllRooms)
            {
                currentRow++;
                worksheet.Cells[currentRow, 1] = room.Id;
                worksheet.Cells[currentRow, 2] = room.Name;
                worksheet.Cells[currentRow, 3] = room.NumSeats;
                worksheet.Cells[currentRow, 4] = room.Price;
                worksheet.Cells[currentRow, 5] = room.Status ? "Доступен" : "Занят";

                Excel.Range roomDataRange = worksheet.Range[$"A{currentRow}:E{currentRow}"];
                roomDataRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            }

            currentRow += 2;
            worksheet.Cells[currentRow, 1] = "СПИСОК БРОНИРОВАНИЙ:";
            Excel.Range bookingsTitleRange = worksheet.Range[$"A{currentRow}:F{currentRow}"];
            bookingsTitleRange.Merge();
            bookingsTitleRange.Font.Bold = true;
            bookingsTitleRange.Interior.Color = Excel.XlRgbColor.rgbLightBlue;

            currentRow++;
            worksheet.Cells[currentRow, 1] = "ID";
            worksheet.Cells[currentRow, 2] = "ID Пользователя";
            worksheet.Cells[currentRow, 3] = "ID Комнаты";
            worksheet.Cells[currentRow, 4] = "Дата заезда";
            worksheet.Cells[currentRow, 5] = "Дата выезда";
            worksheet.Cells[currentRow, 6] = "Стоимость";

            Excel.Range bookingsHeaderRange = worksheet.Range[$"A{currentRow}:F{currentRow}"];
            bookingsHeaderRange.Font.Bold = true;
            bookingsHeaderRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            bookingsHeaderRange.Interior.Color = Excel.XlRgbColor.rgbLightGray;

            foreach (var booking in MainWindow.init.AllBookings)
            {
                currentRow++;
                worksheet.Cells[currentRow, 1] = booking.Id;
                worksheet.Cells[currentRow, 2] = booking.IdUser;
                worksheet.Cells[currentRow, 3] = booking.IdRoom;
                worksheet.Cells[currentRow, 4] = booking.DateEntry.ToShortDateString();
                worksheet.Cells[currentRow, 5] = booking.DateDeparture.ToShortDateString();
                worksheet.Cells[currentRow, 6] = booking.Cost;

                Excel.Range bookingDataRange = worksheet.Range[$"A{currentRow}:F{currentRow}"];
                bookingDataRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            }

            currentRow += 2;
            worksheet.Cells[currentRow, 1] = "СТАТИСТИКА:";
            Excel.Range statsTitleRange = worksheet.Range[$"A{currentRow}:F{currentRow}"];
            statsTitleRange.Merge();
            statsTitleRange.Font.Bold = true;
            statsTitleRange.Interior.Color = Excel.XlRgbColor.rgbOrange;

            currentRow++;
            worksheet.Cells[currentRow, 1] = "Всего пользователей:";
            worksheet.Cells[currentRow, 2] = MainWindow.init.AllUsers.Count;

            currentRow++;
            worksheet.Cells[currentRow, 1] = "Всего номеров:";
            worksheet.Cells[currentRow, 2] = MainWindow.init.AllRooms.Count;

            currentRow++;
            worksheet.Cells[currentRow, 1] = "Всего бронирований:";
            worksheet.Cells[currentRow, 2] = MainWindow.init.AllBookings.Count;

            currentRow++;
            int totalIncome = MainWindow.init.AllBookings.Sum(b => b.Cost);
            worksheet.Cells[currentRow, 1] = "Общая сумма бронирований:";
            worksheet.Cells[currentRow, 2] = $"{totalIncome} руб.";

            Excel.Range statsDataRange = worksheet.Range[$"A{currentRow - 3}:B{currentRow}"];
            statsDataRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            Excel.Range moneyRange = worksheet.Range[$"B{currentRow}"];
            moneyRange.NumberFormat = "# ##0 ₽";

            worksheet.Columns.AutoFit();

            workbook.SaveAs(filePath);
            if (workbook != null)
            {
                workbook.Close(false);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            }

            if (excelApp != null)
            {
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        private void LogoutClick(object sender, RoutedEventArgs e)
        {
            MainWindow.init.CurrentUser = null;
            MainWindow.init.OpenPage(new Authorization());
        }
    }
}
