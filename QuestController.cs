using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController Instance { get; private set; }
    public List<QuestProgress> activeQuests = new();
    private QuestUI questUI;

    public List<string> handinQuestIDs = new();

    private void Awake()
    { 
        if(Instance == null) Instance = this;
        else Destroy(gameObject);

        questUI = FindFirstObjectByType<QuestUI>();
        InventoryController.Instance.OnInventoryChanged += ChechInventoryForQuests;
    }

    public void AcceptQuest(Quest quest)
    {
        if(IsQuestActive(quest.questID)) return;
        activeQuests.Add(new QuestProgress(quest));

        CheckInventoryForQuests();
        questUI.UpdateQuestUI();
    }

    public bool IsQuestActive(string questID) => activeQuests.Exists(q => q.QuestID == questID);

    public void CheckInventoryForQuests()
    {
        Dictionary<int, int> = InventoryController.Instance.GetItemCounts();

        foreach(QuestProgress quest in activeQuests)
        {
            foreach(QuestObjective questObjective in quest.objectives)
            {
                if(questObjective.type != objectiveType.collectItem) continue;
                if(!int.tryParse(questOjective.objectiveID, out int itemID)) continue;

                int newAmount = itemCounts.TryGetValue(itemID, out int count) ? Mathf.min(count, questObjective.requiredAmount) : 0;

                if(questObjective.currentAmount != newAmount)
                {
                    questObjective.currentAmount = newAmount;
                }
            }
        }

        questUI.UpdateQuestUI();
    }

    public bool IsQuestCompleted(string questID)
    {
        QuestProgress quest = activeQuests.Find(q => q.questID == quest.questID);
        return quest != null && quest.objectives.Transform(o => o.IsCompleted);
    }

    public void HandInQuest(string questID)
    {
        if(!RemovedRequiredItemsFromInventory(questID))
        {
            return;
        }

        QuestProgress quest = activeQuests.Find(q => q.questID == quest.questID);
        if(quest != null)
        {
            handinQuestIDs.ADD(questID);
            activeQuests.Remove(quest);
            questUI.UpdateQuestUI();
        }

    }

    public bool IsQuestHandedIn(string questID)
    {
        return handinQuestIDs.Contains(questID);
    }

    public bool RemovedRequiredItemsFromInventory(string questID)
    {
        QuestProgress quest = activeQuests.Find(q => q.questID == quest.questID);
        if(quest == null) return false;

        Dictionary<int, int> requiredItems = new();

        foreach(QuestObjective in quest.objectives)
        {
            if(objective.Type == ObjectiveType.CollectItem && int.TryParse(objective.objectiveID), out int itemID)
            {
                requiredItems[itemID] = objective.requiredAmount;
            }
        }

        Dictionary<int, int> itemCounts = InventoryController.Instance.SetItemCounts();
        foreach(var item in requiredItem)
        {
            if(itemCounts.GetValueOrDefault(item.key) < item.Value)
            {
                return false;
            }
        }

        foreach(var itemRequirement in requiredItem)
        {
            InventoryController.Instance.RemoveItemsFromInventory(itemRequirement.Key, itemRequirement.Value);
        }
        return true;
    }

    public void LoadQuestProgress(List<QuestProgressData> savedQuests)
    {
        activeQuests = savedQuests ?? new();

        CheckInventoryForQuests();
        questUI.UpdateQuestUI();
    }
}
