namespace UdemyEgitimPlatformu.Models
{
    public class SmtpSettings
    {
        public int Id { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }

        public string? Konu { get; set; }

        public string? Mesaj { get; set; }
    }
}
