using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TutorialDummy : MonoBehaviour
{
    public GameObject collectible;
    public TextMeshProUGUI UIText;
    private IQuest currentQuest;
    private List<IQuest> questList = new List<IQuest>();
    private int questIndex = 0;
    void Start()
    {
        GenerateQuests();
        currentQuest = questList[questIndex];
        currentQuest.OnQuestEnter();
        UIText.SetText(currentQuest.GetQuestText());
    }

    void Update()
    {
        UIText.SetText(currentQuest.GetQuestText());
        if (currentQuest.IsComplete())
        {
            currentQuest.OnQuestExit();

            if (questIndex == questList.Count)
            {
                Debug.Log("Oh shit you win");
            }
            currentQuest = questList[++questIndex];

            currentQuest.OnQuestEnter();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Le trigger");
        PlayerManager pm = other.attachedRigidbody.gameObject.GetComponent<PlayerManager>();
        if (pm != null)
        {
            Debug.Log(pm.currentState.ToString());
            if (currentQuest.GetQuestType() == QuestType.HitDummy)
            {
                HitDummyQuest hdq = (HitDummyQuest)currentQuest;
                if (pm.currentState.ToString() == hdq.state)
                {
                    hdq.dummyWasHitByAttack = true;
                }
            }
        }
        else
        {
            ProjectileManager proj = other.gameObject.GetComponent<ProjectileManager>();
            if (proj != null)
            {
                Debug.Log(proj.attack.attackName);
            }
        }
    }
    public void GenerateQuests()
    {
        questList.Add(new CollectibleQuest
        {
            questType = QuestType.RandomCollectibles,
            numCollectibles = 5,
            questText = "Hello, bitch. I am here to teach how to play this amazing game, use Left Stick to collect the collectibles"
        });
        Vector3 p = transform.position;
        p.y += 2;
        questList.Add(new CollectibleQuest
        {
            questType = QuestType.SpecificCollectible,
            point = p,
            questText = "Haha, yes, good job you can move, now you must jump with Space Bar(keyboard) or Button South(Gamepad), collect this shit on my head."
        });
        questList.Add(new HitDummyQuest
        {
            questType = QuestType.HitDummy,
            questText = "Now, you must fight me! Press " + TutorialSceneManager.instance.GetPlayerBindingName("LightAttack")
             + " to light attack me, THE GIANT GREEN CUBE",
            state = "PlayerLightAttackState",
        });
        questList.Add(new HitDummyQuest
        {
            questType = QuestType.HitDummy,
            questText = "Now try HEAVY attacks! Press" + TutorialSceneManager.instance.GetPlayerBindingName("HeavyAttack")
            + " to HEAVY ATTACK",
            state = "PlayerHeavyAttackState",
        });
    }
}


public interface IQuest
{
    public QuestType GetQuestType();
    public bool IsComplete();
    public string GetQuestText();
    public void OnQuestEnter();
    public void OnQuestExit();

    public void OnQuestUpdate();


}

public class CollectibleQuest : IQuest
{
    public QuestType questType;
    public string questText;
    public int numCollectibles;
    public Vector3 point;

    public QuestType GetQuestType()
    {
        return questType;
    }

    public string GetQuestText()
    {
        return questText + "\n Remaining: " + TutorialSceneManager.instance.collectiblesRemaining.ToString();
    }
    public bool IsComplete()
    {
        return TutorialSceneManager.instance.collectiblesRemaining == 0;
    }
    public void OnQuestEnter()
    {
        if (questType == QuestType.RandomCollectibles)
        {
            TutorialSceneManager.instance.SpawnRandomCollectibles(numCollectibles);
        }
        else
        {
            TutorialSceneManager.instance.SpawnCollectibleAtPoint(point);
        }
    }
    public void OnQuestExit()
    {

    }

    public void OnQuestUpdate()
    {

    }
}

public class HitDummyQuest : IQuest
{
    public QuestType questType;
    public string questText;
    public string state;
    public bool dummyWasHitByAttack;
    public QuestType GetQuestType()
    {
        return questType;
    }

    public string GetQuestText()
    {
        return questText;
    }
    public bool IsComplete()
    {
        return dummyWasHitByAttack;
    }
    public void OnQuestEnter()
    {

    }
    public void OnQuestExit()
    {

    }

    public void OnQuestUpdate()
    {

    }
}


public enum QuestType
{
    RandomCollectibles,
    SpecificCollectible,
    HitDummy,
    FightEdmond,
    Listen
}