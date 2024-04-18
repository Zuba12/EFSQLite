namespace EFSQLite.Models
{
    public class Faktury
    {
        public int Id { get; set; } // PK pro databázovou tabulku

        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }
        public string Address { get; set; }
        public string Product { get; set; }
        public string Price { get; set; }
        public string Zpusob { get; set; }
        public string AccountNumber { get; set; }


        public override string ToString()
        {
            return $"{Name} {LastName} {Email} {Number} {Address} {Product} {Price} {Zpusob} {AccountNumber}";
        }
    }
}
