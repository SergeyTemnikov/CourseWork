using System;
using System.Collections.Generic;

namespace Expedition.Models
{
    [Serializable]
    public class GameSaveData
    {
        public Player Player { get; set; }

        public List<Quest> Quests { get; set; }

        public int CurrentQuestIndex { get; set; }
        public List<Environment> Environments { get; set; }
        public int CurrentEnvironmentIndex { get; set; }
        public string TextHistory { get; set; }
        public List<NPC> NPCs { get; set; }
        public List<IInventoryItem> AllItems { get; set; }
        public List<ExitsFromEnvironment> AllTransfers { get; set; }

        public GameSaveData()
        {
            Player = new Player();
            Quests = new List<Quest>();
            Environments = new List<Environment>();
            NPCs = new List<NPC>();
            AllItems = new List<IInventoryItem>();
            TextHistory = string.Empty;
            AllTransfers = new List<ExitsFromEnvironment>();
        }
    }
}
