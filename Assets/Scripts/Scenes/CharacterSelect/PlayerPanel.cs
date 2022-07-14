using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerPanel : MonoBehaviour
{
    private TextMeshPro text;
    private SpriteRenderer sprite;
    private Animator anim;
    private GameObject addCpuButton;
    private GameObject playerTypeButton;
    private GameObject teamButton;
    private VersusInfo versusInfo;
    private CharacterSelectManager cs;
    private SpriteRenderer characterImage;
    public UnityEngine.Object tokenPrefab;
    private CharacterToken activeToken = null;
    public CharacterBox startingCharacterBox;

    public int playerID;

    void Start()
    {
        text = transform.Find("Prompt Text").GetComponent<TextMeshPro>();
        sprite = GetComponent<SpriteRenderer>();
        anim = transform.Find("Character Mask/Character Image").GetComponent<Animator>();
        addCpuButton = transform.Find("Add CPU Button").gameObject;
        playerTypeButton = transform.Find("Player Type Button").gameObject;
        teamButton = transform.Find("Team").gameObject;
        characterImage = transform.Find("Character Mask/Character Image").GetComponent<SpriteRenderer>();
        cs = GameObject.Find("CharacterSelectManager").GetComponent<CharacterSelectManager>();

        versusInfo = GameManager.instance.versusInfo;
    }

    void Update()
    {
        PlayerSetting ps = versusInfo.playerSettings[playerID];

        if (ps != null && ps.playerType != PlayerType.None)
        {
            text.enabled = false;
            sprite.color = ps.team.teamColor;
            addCpuButton.SetActive(false);
            playerTypeButton.SetActive(true);

            if (activeToken == null)
            {
                activeToken = cs.GetToken(ps.playerID);
            }

            if (versusInfo.gameType == GameMode.Team)
            {
                teamButton.SetActive(true);
            }
            else
            {
                teamButton.SetActive(false);
            }

            UpdateCharacterImage(ps);
        }
        else
        {
            text.enabled = true;
            sprite.color = Color.gray;
            addCpuButton.SetActive(true);
            playerTypeButton.SetActive(false);
            teamButton.SetActive(false);

            characterImage.sprite = null;
        }
    }

    private void UpdateCharacterImage(PlayerSetting ps)
    {
        Sprite prevSprite = characterImage.sprite;
        Color prevColor = characterImage.color;
        Character focusedCharacter = Character.None;
        if (activeToken != null) focusedCharacter = activeToken.GetFocusedCharacter();

        if (ps.character != Character.None)
        {
            Roster roster = GameManager.instance.roster;
            characterImage.sprite = roster.GetCharacter(ps.character).fullBody;
            characterImage.color = Color.white;

            if (characterImage.sprite != prevSprite || characterImage.color != prevColor)
            {
                anim.SetTrigger("Select");
            }
        }
        else if (focusedCharacter != Character.None)
        {
            Roster roster = GameManager.instance.roster;
            characterImage.sprite = roster.GetCharacter(focusedCharacter).fullBody;
            characterImage.color = new Color(1, 1, 1, 0.5f);

            if (characterImage.sprite != prevSprite || characterImage.color != prevColor)
            {
                anim.SetTrigger("Enter");
            }
        }
        else
        {
            characterImage.sprite = null;
        }
    }

    public void SetToCPU()
    {
        PlayerSetting ps = new PlayerSetting
        {
            playerID = playerID,
            playerName = "CPU " + (playerID + 1),
            device = null,
            deviceString = "none",
            // TODO: Fix this - Hard-coding this until we can have players choose AI characters
            character = Character.Edmond,
            playerType = PlayerType.Robot,
            team = new Team((TeamID)playerID)
        };
        versusInfo.AddPlayer(ps);
        // if an active character is on this slot, kill them
        var activeCursor = cs.playerList
            .FirstOrDefault((c => c.GetComponent<PlayerInput>().playerIndex == playerID));
        if (activeCursor != null)
        {
            cs.DestroyCursor(playerID);
        }

        if (activeToken != null)
        {
            cs.DestroyToken(playerID);
            activeToken = null;
        }
        GenerateAIToken();
    }

    private void GenerateAIToken()
    {
        GameObject tokenObj = (GameObject)Instantiate(tokenPrefab);
        CharacterToken aiToken;
        aiToken = tokenObj.GetComponent<CharacterToken>();
        aiToken.Initialize(playerID, transform.position);
        startingCharacterBox.PlaceToken(aiToken);
    }

    public void RemoveCPU()
    {
        if (activeToken != null)
        {
            cs.DestroyToken(playerID);
            activeToken = null;
        }
    }
}
