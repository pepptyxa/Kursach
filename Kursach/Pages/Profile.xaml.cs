using Kursach.Classes;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Kursach.Pages
{
    public partial class Profile : Page
    {
        private UserContext _currentUser;
        public Profile()
        {
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            if (MainWindow.init.CurrentUser != null)
            {
                _currentUser = MainWindow.init.CurrentUser;
                tbFullName.Text = $"{_currentUser.Surname} {_currentUser.Name} {_currentUser.Patronomyc}";
                tbPhoneNumber.Text = _currentUser.PhoneNumber;
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            ResetErrors();
            bool isValid = true;

            if (!Regex.IsMatch(tbPhoneNumber.Text, @"^\+?[0-9]{10,12}$"))
            {
                ShowError(tbPhoneNumberError, "Номер телефона должен содержать от 10 до 12 цифр");
                isValid = false;
            }

            if (!string.IsNullOrWhiteSpace(pbPassword.Password))
            {
                if (pbPassword.Password.Length < 6)
                {
                    ShowError(pbPasswordError, "Пароль должен содержать минимум 6 символа");
                    isValid = false;
                }
                else if (pbPassword.Password != pbConfirmPassword.Password)
                {
                    ShowError(pbConfirmPasswordError, "Пароли не совпадают");
                    isValid = false;
                }
            }

            if (isValid)
            {
                _currentUser.PhoneNumber = tbPhoneNumber.Text;
                if (!string.IsNullOrWhiteSpace(pbPassword.Password))
                {
                    _currentUser.Password = pbPassword.Password;
                }
                _currentUser.Save(true);
                int userIndex = MainWindow.init.AllUsers.FindIndex(u => u.Id == _currentUser.Id);
                if (userIndex >= 0)
                {
                    MainWindow.init.AllUsers[userIndex] = _currentUser;
                }
                tbSuccessMessage.Visibility = Visibility.Visible;
                pbPassword.Password = string.Empty;
                pbConfirmPassword.Password = string.Empty;
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new MainPageUser());
        }

        private void ShowError(TextBlock errorTextBlock, string message)
        {
            errorTextBlock.Text = message;
            errorTextBlock.Visibility = Visibility.Visible;
        }

        private void ResetErrors()
        {
            tbPhoneNumberError.Visibility = Visibility.Collapsed;
            pbPasswordError.Visibility = Visibility.Collapsed;
            pbConfirmPasswordError.Visibility = Visibility.Collapsed;
            tbSuccessMessage.Visibility = Visibility.Collapsed;
        }
    }
}
