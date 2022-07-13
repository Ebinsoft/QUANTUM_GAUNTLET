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
    private Transform transformTarget;
    private Vector3? positionTarget;
    public CharacterBox lastCharacterBox;
    private Vector3 target
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

        transform.position = Vector3.MoveTowards(transform.position, target, movementSpeed * Time.deltaTime);
    }

    public void SetPlayer(int playerID)
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

        Hide();
    }

    public void SetTarget(Vector3 targetPos)
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

    private void OnDestroy()
    {
        if (lastCharacterBox != null)
        {
            lastCharacterBox.RemoveToken(this);
        }
    }
}
