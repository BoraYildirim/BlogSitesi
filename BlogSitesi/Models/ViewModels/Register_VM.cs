namespace BlogSitesi.Models.ViewModels
{
    public class Register_VM
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad {get; set; }
        public string EMail { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
