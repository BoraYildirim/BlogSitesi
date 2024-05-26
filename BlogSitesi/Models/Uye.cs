using Microsoft.AspNetCore.Identity;

namespace BlogSitesi.Models
{
    public class Uye:IdentityUser<int>
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string? Adres { get; set; }
        public string? Resim { get; set; }
        public string? Acıklama { get; set; }
        public int ConfirmCode { get; set; }
        public ICollection<Makale> Makales { get; set; }
        public ICollection<UyeKonu> UyeKonus { get; set; }
    }
}
