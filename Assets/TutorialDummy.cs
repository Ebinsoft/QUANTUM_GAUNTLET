using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TutorialDummy : MonoBehaviour
{
    public GameObject collectible;
    public TextMeshProUGUI UIText;
    private IQuest currentQuest;
    private List<IQuest> questList = new List<IQuest>();
    private int numLivingEdmonds;
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
        currentQuest.OnQuestUpdate();
        if (currentQuest.IsComplete())
        {
            currentQuest.OnQuestExit();

            currentQuest = questList[questIndex++];

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

            // Final test - instantly kill the tutorial player
            if (currentQuest.GetQuestType() == QuestType.FightEdmond)
            {
                CharacterData c = GameManager.instance.roster.GetCharacter(Character.Edmond);
                c.characterPrefab.GetComponent<PlayerManager>().playerID = 1;
                FightEdmondQuest feq = (FightEdmondQuest)currentQuest;
                numLivingEdmonds = feq.numEdmond;
                for (int i = 0; i < feq.numEdmond; i++)
                {
                    GameObject g = Instantiate(c.characterPrefab, TutorialSceneManager.instance.spawnPoints.GetSpawnPoint(), c.characterPrefab.transform.rotation);
                    PlayerManager pm = g.GetComponent<PlayerManager>();
                    pm.name = "GIGACUBE";
                    pm.stats.lives = 1;
                    pm.stats.onPlayerLose += OnEdmondLose;
                    TutorialSceneManager.instance.CreateTutorialPlayerHUD(pm, GameManager.instance.versusInfo.GetPlayer(1));

                }
                c.characterPrefab.GetComponent<PlayerManager>().playerID = 0;
            }

            currentQuest.OnQuestEnter();
        }
    }

    public void OnEdmondLose(GameObject player)
    {
        numLivingEdmonds--;
        if (numLivingEdmonds == 0)
        {
            if (currentQuest.GetQuestType() == QuestType.FightEdmond)
            {
                FightEdmondQuest feq = (FightEdmondQuest)currentQuest;
                feq.isComplete = true;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        PlayerManager pm = other.attachedRigidbody.gameObject.GetComponent<PlayerManager>();
        if (pm != null)
        {
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
            if (proj != null && currentQuest.GetQuestType() == QuestType.HitDummy)
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
        questList.Add(new ListenQuest
        {
            questType = QuestType.Listen,
            questText = "Hello, you must be new here, my name is <color=#00FF00>GIGACUBE</color>. "
            + "To make things simple, I will refer to you as <color=#30D5C8>IDIOT</color>.",
            timer = 5f
        });
        questList.Add(new CollectibleQuest
        {
            questType = QuestType.RandomCollectibles,
            numCollectibles = 5,
            questText = "Use " + TutorialSceneManager.instance.GetPlayerBindingName("Move")
            + " to collect the cubes.\n" + "This is basic movement and is very easy!"
        });
        Vector3 p = transform.position;
        p.y += 2;
        questList.Add(new CollectibleQuest
        {
            questType = QuestType.SpecificCollectible,
            point = p,
            questText = "Haha, great job, <color=#30D5C8>IDIOT</color>. Now you must jump with " + TutorialSceneManager.instance.GetPlayerBindingName("Jump")
            + ". Collect this shit on my head."
        });
        questList.Add(new HitDummyQuest
        {
            questType = QuestType.HitDummy,
            questText = "Now, <color=#30D5C8>IDIOT</color>, it's time to fight! Press " + TutorialSceneManager.instance.GetPlayerBindingName("LightAttack")
             + " to light attack me, <color=#00FF00>GIGACUBE</color>. Don't be shy I am only a box.",
            state = "PlayerLightAttackState",
        });
        questList.Add(new ListenQuest
        {
            questType = QuestType.Listen,
            questText = "Ouch, why would you do that, <color=#30D5C8>IDIOT</color> ?",
            timer = 2f
        });
        questList.Add(new HitDummyQuest
        {
            questType = QuestType.HitDummy,
            questText = "I'm just kidding, you are weak and pathletic. Now try HEAVY attacks! Press " + TutorialSceneManager.instance.GetPlayerBindingName("HeavyAttack")
            + " to HEAVY ATTACK",
            state = "PlayerHeavyAttackState",
        });
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
            questText = "Haha great job <color=#30D5C8>IDIOT</color>, now try Special 2! " + TutorialSceneManager.instance.GetPlayerBindingName("PowerToggle")
    + " + " + TutorialSceneManager.instance.GetPlayerBindingName("HeavyAttack")
    + " you can even <color=#FF0000>hold</color> this one!",
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
            questText = "Now try double jumping with " + TutorialSceneManager.instance.GetPlayerBindingName("Jump") +
            " Mid-air and then hitting " + TutorialSceneManager.instance.GetPlayerBindingName("LightAttack")
+ " to aerial attack! Otherwise you'll never reach me!",
            state = "PlayerAirLightAttackState",
        });
        questList.Add(new ListenQuest
        {
            questType = QuestType.Listen,
            questText = "WOW you actually reached that,  <color=#30D5C8>IDIOT</color>. You've really come a long way.",
            timer = 5f
        });
        questList.Add(new ListenQuest
        {
            questType = QuestType.Listen,
            questText = "Here is your final test, <color=#30D5C8>IDIOT</color>. You must fight a robot powered by me, "
            + "<color=#00FF00>GIGACUBE</color>.",
            timer = 5f
        });
        questList.Add(new FightEdmondQuest
        {
            questType = QuestType.FightEdmond,
            questText = "Good luck, loser!",
            numEdmond = 1
        });
        questList.Add(new ListenQuest
        {
            questType = QuestType.Listen,
            questText = "Wow, you actually won, huh? You've gotten strong.",
            timer = 5f
        });
        questList.Add(new ListenQuest
        {
            questType = QuestType.Listen,
            questText = "So strong, in fact, that I'm not really comfortable with letting you live any longer, teehee.",
            timer = 5f
        });
        questList.Add(new ListenQuest
        {
            questType = QuestType.Listen,
            questText = "TIME TO <color=#FF0000>DIEEEEEEEEEEEEEEEEEEEEEEE</color>",
            timer = 1f
        });
        questList.Add(new FightEdmondQuest
        {
            questType = QuestType.FightEdmond,
            questText = "<color=#FF0000>REEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE</color>",
            numEdmond = 10
        });
        questList.Add(new ListenQuest
        {
            questType = QuestType.Listen,
            questText = "WOW JEEZ, OKAY YOU WIN. There's no main menu yet so you can really just hit pause and restart "
            + " or go to character select and play the real game.",
            timer = 1000f
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
    { return questType; }

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
    public void OnQuestExit() { }

    public void OnQuestUpdate() { }
}

public class HitDummyQuest : IQuest
{
    public QuestType questType;
    public string questText;
    public string state;
    public bool dummyWasHitByAttack;
    public QuestType GetQuestType() { return questType; }

    public string GetQuestText() { return questText; }
    public bool IsComplete() { return dummyWasHitByAttack; }
    public void OnQuestEnter() { }
    public void OnQuestExit() { }

    public void OnQuestUpdate() { }
}

public class ListenQuest : IQuest
{
    public QuestType questType;
    public string questText;
    public float timer;
    public QuestType GetQuestType() { return questType; }

    public string GetQuestText() { return questText; }
    public bool IsComplete() { return timer <= 0; }
    public void OnQuestEnter() { }
    public void OnQuestExit() { }
    public void OnQuestUpdate() { timer -= Time.deltaTime; }
}
public class FightEdmondQuest : IQuest
{
    public QuestType questType;
    public string questText;
    public float timer;
    public int numEdmond;
    public bool isComplete = false;
    public QuestType GetQuestType() { return questType; }

    public string GetQuestText() { return questText; }
    public bool IsComplete() { return isComplete; }
    public void OnQuestEnter() { }
    public void OnQuestExit() { }
    public void OnQuestUpdate() { timer -= Time.deltaTime; }
}

public enum QuestType
{
    RandomCollectibles,
    SpecificCollectible,
    HitDummy,
    FightEdmond,
    Listen
}