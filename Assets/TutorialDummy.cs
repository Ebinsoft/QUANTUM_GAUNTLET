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

            if (currentQuest.GetQuestType() == QuestType.HitDummy)
            {
                HitDummyQuest hdq = (HitDummyQuest)currentQuest;
                // raise the block for an air challenge
                if (hdq.state == "PlayerAirLightAttackState")
                {
                    Vector3 p = transform.position;
                    p.y += 5;
                    transform.position = p;
                }
            }

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
            ProjectileManager proj = other.transform.root.GetComponent<ProjectileManager>();
            if (proj != null)
            {
                PlayerManager p = TutorialSceneManager.instance.player;
                Debug.Log(p.currentState.ToString());
                HitDummyQuest hdq = (HitDummyQuest)currentQuest;
                if (p.currentState.ToString() == hdq.state)
                {
                    hdq.dummyWasHitByAttack = true;
                }
            }
        }
    }
    public void GenerateQuests()
    {
        // questList.Add(new CollectibleQuest
        // {
        //     questType = QuestType.RandomCollectibles,
        //     numCollectibles = 5,
        //     questText = "Hello, bitch. I am here to teach how to play this amazing game, use Left Stick to collect the collectibles"
        // });
        // Vector3 p = transform.position;
        // p.y += 2;
        // questList.Add(new CollectibleQuest
        // {
        //     questType = QuestType.SpecificCollectible,
        //     point = p,
        //     questText = "Haha, yes, good job you can move, now you must jump with Space Bar(keyboard) or Button South(Gamepad), collect this shit on my head."
        // });
        // questList.Add(new HitDummyQuest
        // {
        //     questType = QuestType.HitDummy,
        //     questText = "Now, you must fight me! Press " + TutorialSceneManager.instance.GetPlayerBindingName("LightAttack")
        //      + " to light attack me, THE GIANT GREEN CUBE",
        //     state = "PlayerLightAttackState",
        // });
        // questList.Add(new HitDummyQuest
        // {
        //     questType = QuestType.HitDummy,
        //     questText = "Now try HEAVY attacks! Press" + TutorialSceneManager.instance.GetPlayerBindingName("HeavyAttack")
        //     + " to HEAVY ATTACK",
        //     state = "PlayerHeavyAttackState",
        // });
        questList.Add(new HitDummyQuest
        {
            questType = QuestType.HitDummy,
            questText = "Next is Special attacks! These use mana! Try holding " + TutorialSceneManager.instance.GetPlayerBindingName("PowerToggle")
            + " while pressing " + TutorialSceneManager.instance.GetPlayerBindingName("LightAttack")
            + " to use your first special",
            state = "PlayerSpecial1State",
        });
        questList.Add(new HitDummyQuest
        {
            questType = QuestType.HitDummy,
            questText = "Haha great job, now try Special 2! " + TutorialSceneManager.instance.GetPlayerBindingName("PowerToggle")
    + " + " + TutorialSceneManager.instance.GetPlayerBindingName("HeavyAttack")
    + " you can even hold this one!",
            state = "PlayerSpecial2State",
        });
        questList.Add(new HitDummyQuest
        {
            questType = QuestType.HitDummy,
            questText = "Special 3 next you dumb bitch\n" + TutorialSceneManager.instance.GetPlayerBindingName("PowerToggle")
+ " +  " + TutorialSceneManager.instance.GetPlayerBindingName("UtilityAttack")
+ " to use your third special",
            state = "PlayerSpecial3State",
        });
        questList.Add(new HitDummyQuest
        {
            questType = QuestType.HitDummy,
            questText = "Now try " + TutorialSceneManager.instance.GetPlayerBindingName("LightAttack")
+ " after jumping for an aerial attack! Otherwise you'll never reach me CUNT",
            state = "PlayerAirLightAttackState",
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