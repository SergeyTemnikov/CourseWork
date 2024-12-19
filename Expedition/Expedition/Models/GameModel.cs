using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Security.AccessControl;

namespace Expedition.Models
{
    public class GameModel
    {
        public Player Player { get; private set; }
        public List<Quest> Quests { get; private set; }
        public Quest CurrentQuest { get; set; }
        public List<Environment> Environments { get; private set; }
        public Environment CurrentEnvironment { get; set; }
        public List<NPC> NPCs { get; private set; }
        public List<IInventoryItem> AllItems { get; private set; }
        public List<ExitsFromEnvironment> AllTransfers { get; private set; }

        public GameModel()
        {
            Player = new Player();
            Quests = new List<Quest>();
            Environments = new List<Environment>();
            NPCs = new List<NPC>();
            AllItems = new List<IInventoryItem>();
            AllTransfers = new List<ExitsFromEnvironment>();
        }

        public void SetQuests(List<Quest> quests)
        {
            Quests = quests;
        }
        public void SetEnviromnets(List<Environment> environments)
        {
            Environments = environments;
        }
        public void SetNPCS(List<NPC> npcs)
        {
            NPCs = npcs;
        }
        public void SetItems(List<IInventoryItem> items)
        {
            AllItems = items;
        }
        public void SetTransfers(List<ExitsFromEnvironment> transfers)
        {
            AllTransfers = transfers;
        }
        public void SetCurrentQuest(int id)
        {
            CurrentQuest = Quests[id];
        }
        public void SetCurrentEnvironment(int id)
        {
            CurrentEnvironment = Environments[id];
        }



        public string ProcessCommand(string userCommand)
        {
            var response = "";

            var command = userCommand.ToLower();

            if(command.StartsWith("осмотреться"))
            {
                response += CurrentEnvironment.GetDescription();
            }
            if (command.StartsWith("исследовать"))
            {
                response += CurrentEnvironment.GetInteractives();
            }
            if(command.StartsWith("выходы"))
            {
                response += GetExits();
            }
            if (command.StartsWith("посмотреть на"))
            {
                var item = GetItem(command, "посмотреть на ");
                if(item != null)
                {
                    if(CurrentEnvironment.HasItem(item)) response += item.Description;
                }
                else
                {
                    response += "Такого предмета не найдено";
                }
            }
            if (command.StartsWith("взять"))
            {
                var item = GetItem(command, "взять ");

                if (item == null)
                {
                    response += "Такого предмета не найдено.";                  
                }
                else
                {
                    if (!CurrentEnvironment.HasItem(item))
                    {
                        response += "Такого предмета нет в окружении.";
                    }
                    else
                    {
                        if (!Player.AddToInventory(item))
                        {
                            response += "Вы не можете это взять! Максимальное количество уже в инвентаре.";
                        }
                        else
                        {
                            CurrentEnvironment.RemoveItem(item);
                            response += $"Вы взяли {item.Name}.";
                        }
                    }
                }
            }

            if (command.StartsWith("открыть инвентарь"))
            {
                response += Player.GetInventory();
            }
            if(command.StartsWith("использовать "))
            {
                var item = GetItem(command, "использовать");
                if (item != null)
                {
                    if(Player.HasItem(item))
                    {
                        var effect = item.UseItem();
                        if (effect.EffectName == "saturation") 
                        {
                            Player.CurrentHunger = Player.CurrentHunger + effect.EffectValue <= Player.MaximumHunger ? Player.CurrentHunger + effect.EffectValue : Player.MaximumHunger;
                            if(!Player.RemoveFromInventory(item))
                            {
                                response += $"Не удалось использовать {item.Name}";
                            }
                        }
                        if (effect.EffectName == "healing")
                        {
                            Player.CurrentHealthPoint = Player.CurrentHealthPoint + effect.EffectValue <= Player.MaximumHealthPoint ? Player.CurrentHealthPoint + effect.EffectValue : Player.MaximumHealthPoint;
                            if (!Player.RemoveFromInventory(item))
                            {
                                response += $"Не удалось использовать {item.Name}";
                            }
                        }
                        response += effect.UseResponse;
                    }
                }
                else
                {
                    response += "Такого предмета не найдено";
                }
            }
            if (command.StartsWith("перейти в"))
            {
                if (!CurrentQuest.Steps[CurrentQuest.currentStepIndex].IsMovementAllowed) 
                {
                    return "Вы не можете перемещаться сейчас.";
                }
                var envName = command.Replace("перейти в ", String.Empty).Trim();
                var env = Environments.FirstOrDefault(x => x.Name.ToLower() == envName.ToLower() || x.SecondName.ToLower() == envName.ToLower());
                if (env != default)
                {
                    var exits = AllTransfers.Where(x => x.EnvironmentName == CurrentEnvironment.Name).FirstOrDefault();
                    if (exits != default)
                    {
                        var thisEnv = exits.Exits.FirstOrDefault(x => x.Name == env.Name);
                        if (thisEnv == default) response += "Такого перехода не существует";
                        else
                        {
                            CurrentEnvironment = env;
                            response += $"Вы перешли в {env.Name}";
                        }
                    }
                }
                else
                {
                    response += "Подобная локация не найдена";
                }
            }
            if(command.StartsWith("поговорить с"))
            {
                var npcName = command.Replace("поговорить с ", String.Empty).Trim();
                var npc = NPCs.Where(x => x.Name.ToLower() == npcName.ToLower()).FirstOrDefault();
                if (npc != default)
                {
                    response += npc.GetPhrase(CurrentEnvironment.Name);
                }   
            }

            if(command == "загрузить игру")
            {
                LoadGame();
                return CurrentQuest.GetCurrentStepDescription();
            }

            var effectStr = EnvironmentEffect();

            if (effectStr.StartsWith("Вы умерли!"))
            {
                return effectStr;
            }

            if(response == "")
            {
                response += "Неизвестная команда.";
                return response;
            }

            return response + "\n" + CheckQuest(command);
        }

        public IInventoryItem GetItem(string command, string commandStart)
        {
            var objectName = command.Replace(commandStart, string.Empty).Trim();
            var item = AllItems.FirstOrDefault(x => x.Name.ToLower() == objectName.ToLower() || x.SecondName.ToLower() == objectName.ToLower());
            return item; 
        }

        public string GetExits()
        {
            string exitStr = "";
            var exits = AllTransfers.Where(x => x.EnvironmentName == CurrentEnvironment.Name).FirstOrDefault().Exits;
            if (exits != default)
            {
                foreach (var ex in exits)
                {
                    exitStr += $" - {ex.Name}\n";
                }
            }
            if (exitStr == "") return "Нет переходов";

            return exitStr;
        }

        public string EnvironmentEffect()
        {
            if (CurrentEnvironment?.IsDangerous == true)
            {
                int effect = (int)CurrentEnvironment.NegativeEffect;
                Player.CurrentOxygen = Player.CurrentOxygen - effect > 0 ? Player.CurrentOxygen - effect : 0;
                if(Player.CurrentOxygen == 0)
                {
                    Player.CurrentHealthPoint -= effect/2;
                }
            }
            else
            {
                Player.CurrentOxygen = Player.MaximumOxygen;
            }

                int damage = 2;
                Player.CurrentHunger = Player.CurrentHunger - damage > 0 ? Player.CurrentHunger - damage : 0;
                if(Player.CurrentHunger == 0)
                {
                    Player.CurrentHealthPoint -= damage/2;
                }
   

            if (Player.CurrentHealthPoint <= 0)
            {
                return Death();
            }
            return "";
        }

        public string Death()
        {
            string loadMessage = LoadGame();

            if (string.IsNullOrEmpty(loadMessage))
            {
                return "Вы умерли! Игра загружена с последнего сохранения.";
            }
            else
            {
                return $"Вы умерли! {loadMessage}";
            }
        }


        public string CheckQuest(string command)
        {
            var response = CurrentQuest.ProcessCommand(command, Player.Inventory);
            if(response.ResultText == "NonQuest")
            {
                return "";
            }
            if(response.IsEnd == true)
            {
                var questIndex = Quests.FindIndex(a => a.Title == CurrentQuest.Title);
                Quests[questIndex] = CurrentQuest;
                if (CheckAllQuestsCompleted() != "")
                {
                    return response.ResultText + "\n" + CheckAllQuestsCompleted();
                }
                CurrentQuest = Quests[questIndex + 1];
                if (response.ItemsToRemove != null && response.ItemsToRemove.Count > 0)
                {
                    foreach (var item in response.ItemsToRemove)
                    {
                        Player.RemoveFromInventory(item);
                    }
                }
                return CurrentQuest.GetStartText() + "\n" + CurrentQuest.Steps[CurrentQuest.currentStepIndex].Description;
            }
            return response.ResultText;
        }

        public string CheckAllQuestsCompleted()
        {
            bool allQuestsCompleted = Quests.All(quest => quest.State == QuestState.Completed);

            if (allQuestsCompleted)
            {
                return "Поздравляем! Все квесты завершены. Игра окончена!";
            }
            return "";
        }


        public void SaveGame(string textHistory)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "savegame.json");

            GameSaveData saveData = new GameSaveData
            {
                Player = Player,
                Quests = Quests,
                CurrentQuestIndex = Quests.IndexOf(CurrentQuest),
                Environments = Environments,
                CurrentEnvironmentIndex = Environments.IndexOf(CurrentEnvironment),
                TextHistory = textHistory,
                NPCs = NPCs,
                AllItems = AllItems,
                AllTransfers = AllTransfers
            };

            string json = JsonConvert.SerializeObject(saveData, Formatting.Indented, new JsonSerializerSettings
            {
                Converters = { new InventoryItemConverter() } 
            });

            File.WriteAllText(filePath, json);
        }

        public string LoadGame()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "savegame.json");

            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    GameSaveData saveData = JsonConvert.DeserializeObject<GameSaveData>(json, new JsonSerializerSettings
                    {
                        Converters = { new InventoryItemConverter() } 
                    });

                    if (saveData == null)
                    {
                        return "Ошибка загрузки: данные сохранения пусты.";
                    }

                    Player = saveData.Player ?? new Player(); 
                    Quests = saveData.Quests ?? new List<Quest>();

                    var currentQuestIndex = saveData.CurrentQuestIndex;
                    if (currentQuestIndex >= 0 && currentQuestIndex < Quests.Count)
                    {
                        CurrentQuest = Quests[currentQuestIndex];
                    }
                    else
                    {
                        return "Ошибка загрузки: некорректный индекс текущего квеста.";
                    }

                    Environments = saveData.Environments ?? new List<Environment>();

                    var currentEnvironmentIndex = saveData.CurrentEnvironmentIndex;
                    if (currentEnvironmentIndex >= 0 && currentEnvironmentIndex < Environments.Count)
                    {
                        CurrentEnvironment = Environments[currentEnvironmentIndex];
                    }
                    else
                    {
                        return "Ошибка загрузки: некорректный индекс текущей локации.";
                    }

                    NPCs = saveData.NPCs ?? new List<NPC>();
                    AllItems = saveData.AllItems ?? new List<IInventoryItem>();
                    AllTransfers = saveData.AllTransfers ?? new List<ExitsFromEnvironment>();

                    return saveData.TextHistory ?? "История текста отсутствует."; 
                }
                catch (JsonReaderException ex)
                {
                    return $"Ошибка чтения сохраненной игры: {ex.Message}";
                }
                catch (Exception ex)
                {
                    return $"Не удалось загрузить игру: {ex.Message}";
                }
            }

            return "Не получилось загрузить игру! Файл не найден.";
        }



    }

}
