using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Models
{
    public interface IInventoryItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaximumCount { get; set; }
        public bool IsVisible { get; set; }
        public string SecondName { get; set; }

        public string InspectItem();

        public ItemEffect UseItem();


    }
}
