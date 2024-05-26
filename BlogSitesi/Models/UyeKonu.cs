namespace BlogSitesi.Models
{
    public class UyeKonu
    {
        public int UyeKonuID { get; set; }
        public int UyeID { get; set; }
        public int KonuID { get; set; }
        public Uye? Uye { get; set; }
        public Konu? Konu { get; set; }
    }
}
