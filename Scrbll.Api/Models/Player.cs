namespace Scrbll.Api.Models
{
    public class Player
    {
        public long Id { get; set; }
        public string Alias { get; set; }
        public int Scrore { get; private set; } = 0;
        public Avatar Avatar { get; set; }
    }
}