using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Models
{
    public class Quest
    {
        public string Title { get; private set; }
        public string StartText { get; private set; }
        public List<QuestStep> Steps { get; private set; }
        public QuestState State { get; set; }
        public int currentStepIndex;
        public List<IInventoryItem> ItemsToRemove { get; private set; }
        public List<QuestEnding> QuestEndings { get; set; }

        public Quest(string title, string startText, List<IInventoryItem> items = null)
        {
            Title = title;
            StartText = startText;
            Steps = new List<QuestStep>();
            State = QuestState.NotStarted;
            currentStepIndex = 0;
            ItemsToRemove = new List<IInventoryItem>();
            QuestEndings = new List<QuestEnding>();

            if(items != null)
            {
                ItemsToRemove = items;
            }
        }

        public string StartQuest()
        {
            State = QuestState.InProgress;
            return StartText + "\n" + Steps[currentStepIndex].Description;
        }

        public string GetStartText()
        {
            return StartText;
        }

        public void SetSteps(List<QuestStep> steps)
        {
            Steps = steps;
        }

        public void AddStep(QuestStep step)
        {
            Steps.Add(step);
        }
        public void AddItemToRemove(QuestItem item)
        {
            ItemsToRemove.Add(item);
        }
        public void SetEndings(List<QuestEnding> endings)
        {
            QuestEndings = endings;
        }
        public void AddEnding(string text, List<QuestStep> steps)
        {
            QuestEndings.Add(new QuestEnding()
            {
                EndingText = text,
                NecessarySteps = steps
            });
        }
        public string GetCurrentStepDescription()
        {
            if (currentStepIndex < Steps.Count)
            {
                return Steps[currentStepIndex].Description;
            }
            return "Квест завершен!";
        }

        public QuestResults ProcessCommand(string command, List<IInventoryItem> playerItems)
        {
            if (currentStepIndex < Steps.Count)
            {
                var currentStep = Steps[currentStepIndex];

                currentStep.CheckRequiredItems(playerItems);

                var check = currentStep.Complete(command);

                if (currentStep.MoralChoice != null)
                {
                    return HandleMoralChoice(currentStep.MoralChoice, command);
                }

                if (check)
                {
                    return CompleteCurrentStep();
                }
            }

            return new QuestResults("NonQuest", null);
        }

        private QuestResults HandleMoralChoice(MoralChoice moralChoice, string playerDecision)
        {
            if (playerDecision.ToLower() == moralChoice.ChoicePositiveText)
            {
                State = QuestState.Completed;
                return new QuestResults(moralChoice.PositiveOutcome, null, true);
            }
            else if (playerDecision.ToLower() == moralChoice.ChoiceNegativeText)
            {
                State = QuestState.Completed;
                return new QuestResults(moralChoice.NegativeOutcome, null, true);
            }

            return new QuestResults("Неизвестный выбор.", null);
        }

        private QuestResults CompleteCurrentStep()
        {
            currentStepIndex++;

            if (currentStepIndex >= Steps.Count)
            {
                State = QuestState.Completed;
                return new QuestResults(GetEnding(), ItemsToRemove, true);
            }

            return new QuestResults(Steps[currentStepIndex].Description, null);
        }
        public string GetEnding()
        {
            foreach (var ending in QuestEndings)
            {
                bool allStepsCompleted = true;

                foreach (var step in ending.NecessarySteps)
                {
                    var questStep = Steps.FirstOrDefault(x => x.Description == step.Description);
                    if(questStep != default)
                    {
                        if (!questStep.IsCompleted)
                        {
                            allStepsCompleted = false;
                            break;
                        }
                    }
                }

                if (allStepsCompleted)
                {
                    return ending.EndingText;
                }
            }

            return "К сожалению, вы не смогли выполнить все задачи квеста.";
        }

    }

    public enum QuestState
    {
        NotStarted,
        InProgress,
        Completed
    }

    public class QuestStep
    {
        public List<string> NecessaryActions { get; private set; }
        public string Description { get; private set; }
        public bool IsCompleted { get; set; }
        public bool IsMovementAllowed { get; set; }
        public List<IInventoryItem> RequiredItems { get; private set; }
        public MoralChoice MoralChoice { get; set; }

        public QuestStep(List<string> necessaryActions, string description, List<IInventoryItem> requiredItems = null, bool isMovement = true)
        {
            NecessaryActions = necessaryActions;
            Description = description;
            IsCompleted = false;
            RequiredItems = requiredItems ?? new List<IInventoryItem>();
            IsMovementAllowed = isMovement;
        }

        public void SetMovementTrue()
        {
            IsMovementAllowed = true;
        }

        public void SetMovementFalse()
        {
            IsMovementAllowed = false;
        }

        public void SetMoralChoice(MoralChoice moralChoice)
        {
            MoralChoice = moralChoice;
        }

        public bool Complete(string action)
        {
            if (NecessaryActions.Contains(action))
            {
                NecessaryActions.Remove(action);
                if (NecessaryActions.Count == 0 && CanComplete()) 
                {
                    IsCompleted = true;
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool CanComplete()
        {
            return IsCompleted || RequiredItems.Count == 0; 
        }

        public void CheckRequiredItems(List<IInventoryItem> playerItems)
        {
            foreach (var item in RequiredItems.ToList()) 
            {
                var thisItem = playerItems.FirstOrDefault(x => x.Name == item.Name);
                if (thisItem != default) 
                {
                    NecessaryActions.Remove($"взять {item.Name.ToLower()}");
                    RequiredItems.Remove(item);
                }
            }
        }
    }

    public class QuestResults
    {
        public string ResultText { get; set; }
        public List<IInventoryItem> ItemsToRemove { get; set; }
        public bool IsEnd {  get; set; }

        public QuestResults(string result, List<IInventoryItem> items, bool isEnd = false) 
        {
            ResultText = result;
            ItemsToRemove = items;
            IsEnd = isEnd;
        }

        public void SetItemsToRemove(List<IInventoryItem> items)
        {
            ItemsToRemove = items;
        }
        public void AddItem(IInventoryItem item)
        {
            ItemsToRemove.Add(item);
        }
    }

    public class QuestEnding
    { 
        public string EndingText { get; set; }
        public List<QuestStep> NecessarySteps { get; set; }
    }

    public class MoralChoice
    {
        public string ChoicePositiveText { get; set; }
        public string ChoiceNegativeText { get; set; }
        public string PositiveOutcome { get; set; }
        public string NegativeOutcome { get; set; }

        public MoralChoice(string choicePositiveText, string choiceNegativeText, string positiveOutcome, string negativeOutcome)
        {
            ChoicePositiveText = choicePositiveText;
            ChoiceNegativeText = choiceNegativeText;
            PositiveOutcome = positiveOutcome;
            NegativeOutcome = negativeOutcome;
        }
    }


}
