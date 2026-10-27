using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    public Transform questListContent;
    public GameObject questEntryPrefab;
    public GameObject objectiveTextPrefab;

    void Start()
    {
        UpdateQuestUI(); 
    }

    public void UpdateQuestUI()
    {
        foreach(Transform child in questListContent)
        {
            Destroy(child.gameObject);
        }

        foreach(var quest in QuestController.Instance.ActiveQuests)
        {
            GameObject entry = Instantiate(questEntryPrefab, questListContent);
            TMP_Text questNameText = entry.Transform.Find("QuestNameText").GetComponent<TMP_Text>();
            Transform objectiveList = entry.Transform.Find("ObjectiveList");

            questNameText.text = quest.quest.name;

            foreach(var objective in quest.objectives)
            {
                GameObject objTextGO = Instantiate(objectiveTextPrefab, objectiveList);
                TMP_Text objText = objTextGO.GetComponent<TMP_Text>();
                objText.text = $"{objective.description} ({objective.currentAmount}/{objective.requiredAmount})";
            }
        }
    }
}
