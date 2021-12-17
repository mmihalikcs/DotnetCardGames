using CardGames.Common.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGames.War.Models
{
    public sealed class WarHand
    {
        public int PlayerNumber { get; set; }

        public string? Name { get; set; }

        public Card? Card { get; set; }
    }
}
