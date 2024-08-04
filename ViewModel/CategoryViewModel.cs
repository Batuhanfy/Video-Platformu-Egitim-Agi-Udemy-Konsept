using System.Security.Policy;
using UdemyEgitimPlatformu.Models;

namespace UdemyEgitimPlatformu.ViewModel
{
    public class CategoryViewModel
    {
        public IEnumerable<Kategoriler> KategoriListID { get; set; }
        public IEnumerable<Kategoriler> KategoriListGenel { get; set; }
        public IEnumerable<Settings> Ayarlar { get; set; }

        public IEnumerable<Videolar> Videolar { get; set; }
        public string KategoriAdi { get; set; }
        public string VideoBaslik { get; set; }
        public string VideoAciklama { get; set; }
        public string VideoImgSrc { get; set; }

        public double YildizSayisi { get; set; } = 0;
        public int videoIde { get; set; }

        public string videovideoaddress { get; set; }
        public string videoshortadress { get; set; }
        public string videoauthor { get; set; }


    }
}
