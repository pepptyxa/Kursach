using Kursach.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class Authorization : Page
    {
        public Authorization()
        {
            InitializeComponent();
            tbPhoneNumber.Focus();
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            ResetErrors();
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(tbPhoneNumber.Text))
            {
                ShowError(tbPhoneNumberError, "Введите номер телефона");
                isValid = false;
            }
            else if (!Regex.IsMatch(tbPhoneNumber.Text, @"^\+?[0-9]{10,12}$"))
            {
                ShowError(tbPhoneNumberError, "Номер телефона должен содержать от 10 до 12 цифр");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(pbPassword.Password))
            {
                ShowError(pbPasswordError, "Введите пароль");
                isValid = false;
            }

            if (isValid)
            {
                UserContext userContext = new UserContext();
                var allUsers = userContext.AllUsers();

                var user = allUsers.FirstOrDefault(u => u.PhoneNumber == tbPhoneNumber.Text && u.Password == pbPassword.Password);

                if (user != null)
                {
                    MainWindow.init.CurrentUser = user;
                    if (user.Role) 
                    {
                        MainWindow.init.OpenPage(new MainPageAdmin());
                    }
                    else 
                    {
                        MainWindow.init.OpenPage(new MainPageUser());
                    }
                }
                else
                {
                    MessageBox.Show("Неверный номер телефона или пароль", "Ошибка авторизации",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ShowError(TextBlock errorTextBlock, string errorMessage)
        {
            errorTextBlock.Text = errorMessage;
            errorTextBlock.Visibility = Visibility.Visible;
        }

        private void ResetErrors()
        {
            tbPhoneNumberError.Visibility = Visibility.Collapsed;
            pbPasswordError.Visibility = Visibility.Collapsed;
        }

        private void ToRegisterClick(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new Registration());
        }
    }
}
