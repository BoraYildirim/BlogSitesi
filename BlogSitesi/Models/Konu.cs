namespace BlogSitesi.Models
{
    public class Konu
    {
        public int KonuID { get; set; }
        public string KonuBaslik { get; set; }
        public ICollection<MakaleKonu>? MakaleKonus { get; set; }
        public ICollection<UyeKonu>? UyeKonus { get; set; }
    }
}
