using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogSitesi.Models.ViewModels
{
    public class MakaleVM
    {
        public MakaleEkle? MakaleEkle { get; set; }
        public Makale? Makale { get; set; }
        public List<Makale>? mlist { get; set; }     
        public SelectList? Konu { get; set; }
    }
}
