using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamScape.Data.Models
{
    internal class PlayerItem // This is the physical item you will be trading and having in your inventory.
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
