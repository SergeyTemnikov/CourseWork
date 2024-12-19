using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Models
{
    public class QuestItem : IInventoryItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Action { get; set; }
        public int MaximumCount { get; set; }
        public bool IsVisible { get; set; } = true;
        private string? _secondName;
        public string? SecondName
        {
            get
            {
                return _secondName ?? Name;
            }
            set
            {
                _secondName = value;
            }
        }

        public string InspectItem()
        {
            return Description;
        }

        public ItemEffect UseItem()
        {
            return new ItemEffect("quest", 1, $"Вы использовали {Name}.");
        }
    }
}
