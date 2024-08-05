namespace UdemyEgitimPlatformu.Models
{
    public class Menuler
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsEnable { get; set; } = true;

        public Kategoriler Category { get; set; }
    }
}
