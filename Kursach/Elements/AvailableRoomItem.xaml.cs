using Kursach.Classes;
using System.Windows;
using System.Windows.Controls;

namespace Kursach.Elements
{
    public partial class AvailableRoomItem : UserControl
    {
        private RoomContext Room;
        public AvailableRoomItem(RoomContext room)
        {
            InitializeComponent();
            Room = room;

            lId.Content = $"Код: {Room.Id}";
            lName.Content = $"Номер: {Room.Name}";
            lNumSeats.Content = $"Кол-во мест: {Room.NumSeats}";
            lPrice.Content = $"Цена за ночь: {Room.Price} руб.";
        }
        private void BookRoomClick(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new Pages.RentRoom(Room));
        }
    }
}
