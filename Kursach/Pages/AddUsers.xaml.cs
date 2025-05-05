using Kursach.Classes;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Kursach.Pages
{
    public partial class AddUsers : Page
    {
        private bool isEditMode = false;
        private UserContext currentUser = null;
        public AddUsers()
        {
            InitializeComponent();
            tbPageTitle.Text = "Добавление пользователя";
        }
        public AddUsers(UserContext user)
        {
            InitializeComponent();
            tbPageTitle.Text = "Редактирование пользователя";
            isEditMode = true;
            currentUser = user;

            tbSurname.Text = user.Surname;
            tbName.Text = user.Name;
            tbPatronomyc.Text = user.Patronomyc;
            tbPhoneNumber.Text = user.PhoneNumber;
            chbIsAdmin.IsChecked = user.Role;

            pbPassword.Password = string.Empty;
            pbConfirmPassword.Password = string.Empty;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
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
            if (!isEditMode && allUsers.Exists(u => u.PhoneNumber == tbPhoneNumber.Text))
            {
                ShowError(tbPhoneNumberError, "Пользователь с таким номером телефона уже существует");
                isValid = false;
            }
            else if (isEditMode && allUsers.Exists(u => u.PhoneNumber == tbPhoneNumber.Text && u.Id != currentUser.Id))
            {
                ShowError(tbPhoneNumberError, "Пользователь с таким номером телефона уже существует");
                isValid = false;
            }

            if (!isEditMode || !string.IsNullOrWhiteSpace(pbPassword.Password))
            {
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
            }

            if (isValid)
            {
                UserContext user;
                if (isEditMode)
                {
                    user = currentUser;
                    user.Surname = tbSurname.Text;
                    user.Name = tbName.Text;
                    user.Patronomyc = string.IsNullOrWhiteSpace(tbPatronomyc.Text) ? "" : tbPatronomyc.Text;
                    user.PhoneNumber = tbPhoneNumber.Text;
                    user.Role = chbIsAdmin.IsChecked ?? false;

                    if (!string.IsNullOrWhiteSpace(pbPassword.Password))
                    {
                        user.Password = pbPassword.Password;
                    }

                    user.Save(true);
                    MessageBox.Show("Пользователь успешно отредактирован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    user = new UserContext
                    {
                        Surname = tbSurname.Text,
                        Name = tbName.Text,
                        Patronomyc = string.IsNullOrWhiteSpace(tbPatronomyc.Text) ? "" : tbPatronomyc.Text,
                        Password = pbPassword.Password,
                        PhoneNumber = tbPhoneNumber.Text,
                        Role = chbIsAdmin.IsChecked ?? false
                    };

                    user.Save();
                    MessageBox.Show("Пользователь успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                MainWindow.init.AllUsers = new UserContext().AllUsers();
                MainWindow.init.OpenPage(new Users());
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
            MainWindow.init.OpenPage(new Users());
        }
    }
}
