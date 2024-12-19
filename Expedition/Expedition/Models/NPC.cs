using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Models
{
    public class NPC
    {
        public string Name { get; set; }
        public List<Phrase> Phrases { get; set; }

        public NPC(string name) {
            Name = name;
            Phrases = new List<Phrase>();
        }

        public void AddPhrase(string text, string environmentName)
        {
            Phrases.Add(new Phrase(text, environmentName));
        }

        public string GetPhrase(string environmentName)
        {
            var phrase = Phrases.Where(x => x.EnvironmentName == environmentName).FirstOrDefault();
            return phrase != default ? phrase.GetPhrase(Name) : $"Сейчас вы не можете поговорить с {Name}"; 
        }

    }
}
