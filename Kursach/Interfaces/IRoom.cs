using System.Collections.Generic;

namespace Kursach.Interfaces
{
    public interface IRoom
    {
        void Save(bool Update = false);
        List<Classes.RoomContext> AllRooms();
        void Delete();
    }
}
