using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Models
{
    public class Phrase
    {
        public string Text { get; set; }
        public string EnvironmentName { get; set; }
        public bool IsCheck { get; set; }

        public Phrase(string text, string environmentName) 
        {
            Text = text;
            EnvironmentName = environmentName;
            IsCheck = false;
        }

        public string GetPhrase(string characterName)
        {
            if(!IsCheck)
            {
                return Text;
            }
            else
            {
                return $"Вы уже разговаривали {characterName}.";
            }
        }
    }
}
