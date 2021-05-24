using System;
using System.Collections.Generic;

namespace Scrbll.Api.Models
{
    public class Game
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; init; } = DateTime.Now;
        public DateTime? StartedAt { get; set; }
        public string Token { get; init; }
        public Player Owner { get; init; }
        public IList<Player> Players { get; init; } = new List<Player>();
        public Player ActivePlayer { get; set; } = null;

        public Game()
        {

        }

        public Game(string token, Player owner)
        {
            Token = token;
            Owner = owner;
            Players.Add(Owner);
        }
    }
}
