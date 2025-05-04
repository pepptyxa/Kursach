using Kursach.Classes;
using System.Windows.Controls;

namespace Kursach.Elements
{
    public partial class UserItem : UserControl
    {
        UserContext User;
        public UserItem(UserContext User)
        {
            InitializeComponent();
            UserContext userContext = new UserContext();
            lId.Content = $"Код: {User.Id}";
            lSurname.Content = $"Фамилия: {User.Surname}";
            lName.Content = $"Имя: {User.Name}";
            lPatronomyc.Content = $"Отчество (при наличии): {User.Patronomyc}";
            lPhoneNumber.Content = $"Номер телефона: {User.PhoneNumber}";
            lRole.Content = $"Роль: {(User.Role ? "Администратор" : "Пользователь")}";
            this.User = User;
        }
        private void Edit(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new Pages.AddUsers());
        }

        private void Delete(object sender, System.Windows.RoutedEventArgs e)
        {
            User.Delete();
            MainWindow.init.AllUsers = new UserContext().AllUsers();
            MainWindow.init.OpenPage(new Pages.Users());
        }
    }
}
