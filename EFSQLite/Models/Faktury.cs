using System.ComponentModel.DataAnnotations;

namespace EFSQLite.Models
{
    public class Faktury
    {
        public int Id { get; set; } // PK pro databázovou tabulku
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Address { get; set; }
        public string PSC { get; set; }
        private string _email;

        public string Email
        {
            get { return _email; }
            set
            {
                // Pokud je e-mailová adresa prázdná nebo neobsahuje zavináč, vyvoláme výjimku
                if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                {
                    throw new ArgumentException("Invalid or missing email address.");
                }
                _email = value;
            }
        }
        public string Number { get; set; }
        public string Atrribute { get; set; }
        public string Price { get; set; }
        public string Zpusob { get; set; }
        public string AccountNumber { get; set; }
        public string PocetKusu { get; set; }
        public string DatumVystaveni { get; set; }
        public string DatumSplatnosti { get; set; }

        public override string ToString()
        {
            return $"{Name} {SurName} {Address} {PSC} {Email} {Number} {Atrribute} {Price} {Zpusob} {AccountNumber} {PocetKusu} {DatumVystaveni} {DatumSplatnosti}";
        }
    }
}
