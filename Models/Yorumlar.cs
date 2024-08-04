namespace UdemyEgitimPlatformu.Models
{
    public class Yorumlar
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public string? Value { get; set; }
        public DateTime DateTime { get; set; }= DateTime.Now;

        public bool IsDelete { get; set; } = false;


    }
}
