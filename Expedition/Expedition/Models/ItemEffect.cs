using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Models
{
    public class ItemEffect
    {
        public string EffectName { get; set; }
        public int EffectValue { get; set; }
        public string UseResponse {  get; set; }

        public ItemEffect(string effectName, int effectValue, string useResponse) 
        {
            EffectName = effectName;
            EffectValue = effectValue;
            UseResponse = useResponse;
        }

    }
}
