using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Models
{
    public class HealthKit : IInventoryItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int RecoverableHealth {  get; set; }
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
            return new ItemEffect("healing", RecoverableHealth, $"Вы использовали: {Name}");
        }
    }
}
