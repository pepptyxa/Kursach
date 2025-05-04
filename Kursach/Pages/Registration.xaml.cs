using Kursach.Classes;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Kursach.Pages
{
    public partial class Registration : Page
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void RegisterClick(object sender, RoutedEventArgs e)
        {
            ResetErrors();
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(tbSurname.Text))
            {
                ShowError(tbSurnameError, "Фамилия обязательна для заполнения");
                isValid = false;
            }
            else if (!Regex.IsMatch(tbSurname.Text, @"^[А-Яа-яA-Za-z]+$"))
            {
                ShowError(tbSurnameError, "Фамилия должна содержать только буквы");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                ShowError(tbNameError, "Имя обязательно для заполнения");
                isValid = false;
            }
            else if (!Regex.IsMatch(tbName.Text, @"^[А-Яа-яA-Za-z]+$"))
            {
                ShowError(tbNameError, "Имя должно содержать только буквы");
                isValid = false;
            }

            if (!string.IsNullOrWhiteSpace(tbPatronomyc.Text) && !Regex.IsMatch(tbPatronomyc.Text, @"^[А-Яа-яA-Za-z]+$"))
            {
                ShowError(tbPatronomycError, "Отчество должно содержать только буквы");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(tbPhoneNumber.Text))
            {
                ShowError(tbPhoneNumberError, "Номер телефона обязателен для заполнения");
                isValid = false;
            }
            else if (!Regex.IsMatch(tbPhoneNumber.Text, @"^\+?[0-9]{10,12}$"))
            {
                ShowError(tbPhoneNumberError, "Номер телефона должен содержать от 10 до 12 цифр");
                isValid = false;
            }

            UserContext userContext = new UserContext();
            var allUsers = userContext.AllUsers();
            if (allUsers.Exists(u => u.PhoneNumber == tbPhoneNumber.Text))
            {
                ShowError(tbPhoneNumberError, "Пользователь с таким номером телефона уже существует");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(pbPassword.Password))
            {
                ShowError(pbPasswordError, "Пароль обязателен для заполнения");
                isValid = false;
            }
            else if (pbPassword.Password.Length < 6)
            {
                ShowError(pbPasswordError, "Пароль должен содержать не менее 6 символов");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(pbConfirmPassword.Password))
            {
                ShowError(pbConfirmPasswordError, "Подтверждение пароля обязательно");
                isValid = false;
            }
            else if (pbPassword.Password != pbConfirmPassword.Password)
            {
                ShowError(pbConfirmPasswordError, "Пароли не совпадают");
                isValid = false;
            }

            if (isValid)
            {
                UserContext newUser = new UserContext
                {
                    Surname = tbSurname.Text,
                    Name = tbName.Text,
                    Patronomyc = string.IsNullOrWhiteSpace(tbPatronomyc.Text) ? "" : tbPatronomyc.Text,
                    Password = pbPassword.Password,
                    PhoneNumber = tbPhoneNumber.Text,
                    Role = false 
                };

                newUser.Save();
                MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindow.init.OpenPage(new Authorization());
            }
        }

        private void ShowError(TextBlock errorTextBlock, string errorMessage)
        {
            errorTextBlock.Text = errorMessage;
            errorTextBlock.Visibility = Visibility.Visible;
        }

        private void ResetErrors()
        {
            tbSurnameError.Visibility = Visibility.Collapsed;
            tbNameError.Visibility = Visibility.Collapsed;
            tbPatronomycError.Visibility = Visibility.Collapsed;
            tbPhoneNumberError.Visibility = Visibility.Collapsed;
            pbPasswordError.Visibility = Visibility.Collapsed;
            pbConfirmPasswordError.Visibility = Visibility.Collapsed;
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new Authorization());
        }

        private void ToLoginClick(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new Authorization());
        }
    }
}
