using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterToken : MonoBehaviour
{
    public int playerID { get; private set; }
    public PlayerType playerType { get; private set; }

    public SpriteRenderer sprite;
    private TextMeshPro label;
    private VersusInfo versusInfo;

    private float movementSpeed = 30f;
    public CharacterBox lastCharacterBox;
    private Transform transformTarget;
    private Vector2? positionTarget;
    private Vector2 target
    {
        get
        {
            if (transformTarget != null)
            {
                return transformTarget.position;
            }
            else if (positionTarget != null)
            {
                return positionTarget.Value;
            }
            else
            {
                return transform.position;
            }
        }
    }

    void Awake()
    {
        sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        label = transform.Find("Label").GetComponent<TextMeshPro>();
        versusInfo = GameManager.instance.versusInfo;
    }

    void Update()
    {
        sprite.color = versusInfo.GetPlayer(playerID).team.teamColor;

        // only move on the XY plane so we don't mess up the Z-layering
        Vector2 pos2D = Vector2.MoveTowards(transform.position, target, movementSpeed * Time.deltaTime);
        transform.position = new Vector3(pos2D.x, pos2D.y, transform.position.z);
    }

    public void Initialize(int playerID, Vector2 position, bool hide = false)
    {
        this.playerID = playerID;

        this.playerType = versusInfo.GetPlayer(playerID).playerType;

        if (playerType == PlayerType.Robot)
        {
            label.text = "CPU" + (playerID + 1);
        }
        else
        {
            label.text = "P" + (playerID + 1);
        }

        transform.position = new Vector3(position.x, position.y, transform.position.z);

        CharacterSelectManager csm = GameObject.Find("CharacterSelectManager").GetComponent<CharacterSelectManager>();
        csm.AddToken(gameObject);

        if (hide)
        {
            Hide();
        }
    }

    public void SetTarget(Vector2 targetPos)
    {
        transformTarget = null;
        positionTarget = targetPos;
    }

    public void SetTarget(Transform target)
    {
        positionTarget = null;
        transformTarget = target;
    }

    public void Hide()
    {
        sprite.enabled = false;
        label.enabled = false;
    }

    public void Show()
    {
        sprite.enabled = true;
        label.enabled = true;
    }

    public void CleanUp()
    {
        if (lastCharacterBox != null)
        {
            lastCharacterBox.RemoveToken(this);
        }
    }
}
