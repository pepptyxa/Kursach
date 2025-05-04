using Kursach.Classes;
using System.Windows.Controls;

namespace Kursach.Elements
{
    public partial class RoomItem : UserControl
    {
        RoomContext Room;
        public RoomItem(RoomContext Room)
        {
            InitializeComponent();
            RoomContext roomContext = new RoomContext();
            lId.Content = $"Код: {Room.Id}";
            lName.Content = $"Номер: {Room.Name}";
            lNumSeats.Content = $"Кол-во мест: {Room.NumSeats}";
            lPrice.Content = $"Цена за ночь: {Room.Price} руб.";
            lStatus.Content = $"Статус: {(Room.Status ? "Доступен" : "Занят")}";
            this.Room = Room;
        }
        private void Edit(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new Pages.AddRooms());
        }

        private void Delete(object sender, System.Windows.RoutedEventArgs e)
        {
            Room.Delete();
            MainWindow.init.AllRooms = new RoomContext().AllRooms();
            MainWindow.init.OpenPage(new Pages.Rooms());
        }
    }
}
