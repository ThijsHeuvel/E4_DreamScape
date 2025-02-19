using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamScape.Data.Models
{
    internal class Item // This is just a blueprint, so you can pull data from an item that a player has. See PlayerItem for the actual item.
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Type { get; set; }
        public required Rarity Rarity { get; set; }
        public string? AdditionalEffects { get; set; }
    }

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythic
    }
}
