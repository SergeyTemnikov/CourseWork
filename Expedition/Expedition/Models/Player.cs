using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Models
{
    public class Player
    {
        public string Name { get; set; }
        public List<IInventoryItem> Inventory { get; set; }
        public int CurrentHealthPoint { get; set; }
        public int MaximumHealthPoint { get; set; }
        public int CurrentOxygen { get; set; }
        public int MaximumOxygen { get; set; }
        public int CurrentHunger { get; set; }
        public int MaximumHunger { get; set; }

        public string CurrentStats
        {
            get
            {
                return $"Здровье: {CurrentHealthPoint}/{MaximumHealthPoint} Кислород: {CurrentOxygen}/{MaximumOxygen} Голод: {CurrentHunger}/{MaximumHunger}";
            }
        }

        public Player()
        {
            Inventory = new List<IInventoryItem>();
            Name = "Исследователь";
            MaximumHealthPoint = 20;
            MaximumOxygen = 100;
            MaximumHunger = 50;
            CurrentHealthPoint = MaximumHealthPoint;
            CurrentOxygen = MaximumOxygen;
            CurrentHunger = MaximumHunger;
        }

        public bool AddToInventory(IInventoryItem item)
        {
            if (Inventory.Count(x => x.Name == item.Name) < item.MaximumCount)
            {
                Inventory.Add(item); 
                return true;
            }
            return false; 
        }

        public bool RemoveFromInventory(IInventoryItem item)
        {
            var existingItem = Inventory.FirstOrDefault(x => x.Name == item.Name);

            if (existingItem != null)
            {
                Inventory.Remove(existingItem); 
                return true; 
            }

            return false; 
        }

        public bool HasItem(IInventoryItem item)
        {
            var thisItem = Inventory.FirstOrDefault(x => x.Name == item.Name);
            if (thisItem == default) return false;
            return true;
        }

        public string GetInventory()
        {
            var inventoryString = "\n";

            foreach (var item in Inventory.GroupBy(x => x.Name))
            {
                inventoryString += $"- {item.Key} (Количество: {item.Count()})\n"; 
            }

            if (inventoryString == "\n")
                return "В инвентаре пусто!";

            return inventoryString;
        }
    }
}
