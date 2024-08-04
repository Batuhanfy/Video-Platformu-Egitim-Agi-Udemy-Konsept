namespace UdemyEgitimPlatformu.Models
{
    public class AlinanVideolar
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int VideoId { get; set; }
 
        public string? VideoName { get; set; }

        public DateTime? Tarih { get; set; } = DateTime.Now;
        public DateTime? SonTarih { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
   
    }
}
