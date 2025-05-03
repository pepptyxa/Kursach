namespace Kursach.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronomyc { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public bool Role { get; set; }
    }
}
