namespace Models
{
    public class Vozac{

        [Key]
       public int ID { get; set; }

        [RegularExpression("^[a-zA-z]+$")]
        [StringLength(30, MinimumLength = 3)]
        public required string Ime { get; set; }
        [RegularExpression("^[a-zA-z]+$")]
        [StringLength(30, MinimumLength = 3)]
        public required string  Prezime { get; set; }

        [RegularExpression("^[0-9]+$")]
        [StringLength(13)]
        public required string JMBG { get; set; }

        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")]
        public required string Email {get; set;}

        [RegularExpression("^[a-zA-Z][a-zA-Z0-9]*$")]
        public required string KorisnickoIme {get; set;}
        public required string Sifra { get; set; }
        [RegularExpression(@"^\+?[0-9][0-9\s.-]{7,11}$")]
        public required string BrojTelefona { get; set;}
        public required string Slika { get; set; }
        public List<Favorizacija>? Favorizacije { get; set; }
        [JsonIgnore]
        public List<Ocena>? Ocene { get; set; }

        [JsonIgnore]
        public List<Vozilo>? Vozila { get; set; }
        [JsonIgnore]

        public List<Prikolica>? Prikolice { get; set; }
        [JsonIgnore]
        public List<PonudjenaTura>? PonudjeneTure { get; set; }
        [JsonIgnore]
        public List<PrihvacenaTura>? PrihvaceneTure {get; set;}
        [JsonIgnore]
        public List<DodeljenaTuraa>? DodeljeneTure{get; set;}
    }
}
