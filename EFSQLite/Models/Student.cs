namespace EFSQLite.Models
{
    public class Student
    {
        public int Id { get; set; } // PK pro databázovou tabulku

        public string Name { get; set; }
        public string Email { get; set; }
        public decimal Number { get; set; }
        public string Address { get; set; }
        public decimal IC { get; set; }
        public decimal DIC { get; set; }


        public override string ToString()
        {
            return $"{Name} {Email} {IC} {DIC} {Number} {Address} ";   
        }
    }
}
