using System.Collections.Generic;

namespace Kursach.Interfaces
{
    public interface IUser
    {
        void Save(bool Update = false);
        List<Classes.UserContext> AllUsers();
        void Delete();
    }
}
