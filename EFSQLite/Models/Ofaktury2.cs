namespace EFSQLite.Models
{
    public class Ofaktury2
    {
        public int Id { get; set; } // PK pro databázovou tabulku

        public string Name { get; set; }
        public string SurName { get; set; }
        public string Address { get; set; }
        public string PSC { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }
        public string Atrribute { get; set; }
        public string Price { get; set; }
        public string Zpusob { get; set; }
        public string AccountNumber { get; set; }
        public string PocetKusu { get; set; }


        public override string ToString()
        {
            return $"{Name} {SurName} {Address} {PSC} {Email} {Number} {Atrribute} {Price} {Zpusob} {AccountNumber} {PocetKusu}";
        }
    }
}
