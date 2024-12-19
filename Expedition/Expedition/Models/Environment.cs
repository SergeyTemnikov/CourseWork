using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Models
{
    public class Environment
    {
        public string Name { get; private set; }
        public string SecondName { get; private set; }
        public string Description { get; private set; }
        public List<IInventoryItem> ItemsInEnvironment { get; private set; }
        public List<NPC> CharactersInEnvironment { get; private set; }
        public bool IsDangerous { get; set; }
        public int? NegativeEffect { get; set; }

        public Environment(string name, string description, string secondName = "")
        {
            Name = name;
            Description = description;
            ItemsInEnvironment = new List<IInventoryItem>();
            CharactersInEnvironment = new List<NPC>();
            IsDangerous = false;

            if(secondName == "")
            {
                SecondName = name;
            }
            else
            {
                SecondName = secondName;
            }

        }

        public void AddItem(IInventoryItem item)
        {
            ItemsInEnvironment.Add(item);
        }
        public void RemoveItem(IInventoryItem item)
        {
            ItemsInEnvironment.Remove(item);
        }

        public void AddCharacter(NPC character)
        {
            CharactersInEnvironment.Add(character);
        }

        public void SetDangerous(int effect)
        {
            IsDangerous = true;
            NegativeEffect = effect;
        }

        public string GetDescription() 
        {
            return Description; 
        }

        public string GetInteractives()
        {
            string interactives = "";
            interactives += GetCharacters();
            interactives += GetItems();

            if (interactives == "") return "Рядом нет активностей";

            return interactives;
        }

        public string GetCharacters()
        {
            var characters = "";
            if (CharactersInEnvironment.Count > 0)
            {
                characters += "Люди рядом:\n";
                foreach (var character in CharactersInEnvironment)
                {
                    characters += $"- {character.Name}\n";
                }
            }
            return characters;
        }

        public string GetItems()
        {
            var items = "";
            if (ItemsInEnvironment.Count > 0)
            {
                items += "Предметы рядом:\n";

                foreach (var item in ItemsInEnvironment.GroupBy(x => x.Name))
                {
                    items += $"- {item.Key}\n";
                }
            }

            if (items == "\n")
                return "В комнате пусто";

            return items;
        }

        public bool HasItem(IInventoryItem item)
        {
            var thisItem = ItemsInEnvironment.FirstOrDefault(x => x.Name == item.Name);
            if(thisItem == default) return false;
            return true;
        }
    }
}
