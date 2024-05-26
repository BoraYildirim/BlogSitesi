namespace BlogSitesi.Models
{
    public class Makale
    {
        public int MakaleID { get; set; }
        public string Baslık { get; set; }
        public string Metin { get; set; }
        public DateTime Tarih { get; set; }
        public double OrtalamaSure { get; set; }
        public int OkunmaSayisi { get; set; }
        public ICollection<MakaleKonu>? MakaleKonus { get; set; }
        public int UyeID { get; set; }
        public Uye? Uye { get; set; }
        
    }
}
