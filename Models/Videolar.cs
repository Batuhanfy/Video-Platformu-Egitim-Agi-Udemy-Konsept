namespace UdemyEgitimPlatformu.Models
{
    public class Videolar

    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public int CategoryId { get; set; }
        public string? img { get; set; }

        public double yildizortalamasi { get; set; } = 0;

        public DateTime songuncelleme { get; set; }=DateTime.Now;

        public string? videoaddress { get; set; }
        public string? videoshortadress { get; set; }

        public Kategoriler? Category { get; set; }
    }
}
