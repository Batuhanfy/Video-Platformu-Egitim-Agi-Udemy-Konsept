
namespace UdemyEgitimPlatformu.Models
{
    public class Kategoriler
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsDeleted { get; set; } = false;


        public ICollection<Videolar>? Videolar { get; set; }


        public ICollection<Menuler>? Menuler { get; set; }

    }
}
