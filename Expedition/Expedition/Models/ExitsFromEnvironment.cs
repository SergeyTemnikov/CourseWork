using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Models
{
    public class ExitsFromEnvironment
    {

        public string EnvironmentName { get; set; }
        public List<Environment> Exits { get; set; }
        public ExitsFromEnvironment(string environmentName)
        {
            EnvironmentName = environmentName;
            Exits = new List<Environment>();
        }

        public void AddExit(Environment environment)
        {
            Exits.Add(environment);
        }

        static public Environment Transfer(List<ExitsFromEnvironment> allExits, string currentEnvironment, string nextEnvironment)
        {
            var exits = allExits.Where(x => x.EnvironmentName == currentEnvironment).FirstOrDefault();
            if (exits != default)
            {
                var exit = exits.Exits.Where(x => x.Name == nextEnvironment).FirstOrDefault();
                if (exit != default) 
                {
                    return exit;
                }
            }
            return null;
        }
    }
}
