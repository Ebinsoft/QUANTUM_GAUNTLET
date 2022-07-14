using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandCursor : MonoBehaviour
{
    public PlayerInput playerInput;
    public float cursorSpeed = 1000f;

    public Sprite neutralSprite;
    public Sprite buttonHoverSprite;
    public Sprite holdingTokenSprite;


    private Vector2 currentMovement;
    private bool isMovePressed;
    private PlayerSetting playerSetting;
    private SpriteRenderer sprite;
    private float cursorPadding = .25f;

    public UnityEngine.Object tokenPrefab;
    private CharacterToken heldToken = null;
    private CharacterToken myToken = null;
    private List<GameObject> focusedElements;
    private bool isOverRoster = false;
    private bool isSummoning = false;
    private CharacterSelectManager cs;


    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        cs = GameObject.Find("CharacterSelectManager").GetComponent<CharacterSelectManager>();

        focusedElements = new List<GameObject>();
    }

    void Start()
    {
        // Look up this cursor's relevant playerSetting from the Game Manager
        GetPlayerSetting();
        GeneratePlayerToken();
    }

    private void GeneratePlayerToken()
    {
        cs.DestroyToken(playerInput.playerIndex);
        GameObject tokenObj = (GameObject)Instantiate(tokenPrefab);
        heldToken = tokenObj.GetComponent<CharacterToken>();
        myToken = heldToken;
        heldToken.Initialize(playerInput.playerIndex, transform.position, hide: true);
        heldToken.SetTarget(transform);
    }

    private void onMove(InputAction.CallbackContext context)
    {
        isMovePressed = playerInput.actions["Move"].ReadValue<Vector2>().magnitude > 0;
    }

    private void onClick(InputAction.CallbackContext context)
    {
        if (focusedElements.Count == 0) return;

        IBasicButton button = GetFocusedComponent<IBasicButton>();
        if (button != null)
        {
            button.Click();
        }

        CharacterBox charBox = GetFocusedComponent<CharacterBox>();
        if (charBox != null)
        {
            if (heldToken == null)
            {
                CharacterToken token = GetFocusedComponent<CharacterToken>();
                if (token != null)
                {
                    bool canPickUp = token.playerType == PlayerType.Robot
                                  || token.playerID == playerInput.playerIndex;

                    if (canPickUp)
                    {
                        charBox.RemoveToken(token);
                        token.SetTarget(transform);
                        heldToken = token;
                    }
                }
            }
            else
            {
                charBox.PlaceToken(heldToken);
                heldToken = null;
            }
        }
    }

    private void onBack(InputAction.CallbackContext context)
    {
        if (heldToken != null) return;

        isSummoning = true;
    }

    private T GetFocusedComponent<T>()
    {
        return focusedElements
            .Select(e => e.GetComponent<T>())
            .Where(c => c != null)
            .FirstOrDefault();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        focusedElements.Add(other.gameObject);

        IBasicButton button = other.gameObject.GetComponent<IBasicButton>();
        if (button != null)
        {
            button.HoverEnter();
        }

        if (other.gameObject.tag == "RosterZone")
        {
            isOverRoster = true;
            if (heldToken != null)
            {
                heldToken.Show();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        focusedElements.Remove(other.gameObject);

        IBasicButton button = other.gameObject.GetComponent<IBasicButton>();
        if (button != null)
        {
            button.HoverExit();
        }

        sprite.sprite = neutralSprite;

        if (other.gameObject.tag == "RosterZone")
        {
            isOverRoster = false;
            if (heldToken != null)
            {
                heldToken.Hide();
            }
        }
    }

    private void ChangeState(GameObject go)
    {
        // have to re-grab this or sometimes the reference dies
        GetPlayerSetting();
        CharacterBox characterBox = go.GetComponent<CharacterBox>();
        if (characterBox != null)
        {
            // update our chosen character string
            playerSetting.character = characterBox.GetCharacterName();
        }

        IBasicButton button = go.GetComponent<IBasicButton>();
        if (button != null)
        {
            button.Click();
        }
    }

    private void onStart(InputAction.CallbackContext context)
    {
        Debug.Log("START");
    }

    private void Summon()
    {
        Vector3 p2 = myToken.transform.position;
        Vector2 pos2D = Vector2.MoveTowards(transform.position, p2, cursorSpeed * Time.deltaTime);
        transform.position = new Vector3(pos2D.x, pos2D.y, transform.position.z);

        if (focusedElements.Contains(myToken.gameObject))
        {
            CharacterBox charBox = GetFocusedComponent<CharacterBox>();
            if (charBox != null)
            {
                charBox.RemoveToken(myToken);
                myToken.SetTarget(transform);
                heldToken = myToken;
                isSummoning = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCursorColor();

        if (isSummoning)
        {
            Summon();
        }
        else if (isMovePressed)
        {
            updateCursor();
        }

        if (isOverRoster)
        {
            if (heldToken != null)
            {
                sprite.sprite = holdingTokenSprite;
            }
            else
            {
                sprite.sprite = neutralSprite;
            }
        }
        else
        {
            if (GetFocusedComponent<IBasicButton>() != null)
            {
                sprite.sprite = buttonHoverSprite;
            }
            else
            {
                sprite.sprite = neutralSprite;
            }
        }
    }

    private void UpdateCursorColor()
    {
        GetPlayerSetting();
        sprite.color = playerSetting.team.teamColor;
    }

    private void GetPlayerSetting()
    {
        foreach (PlayerSetting ps in GameManager.instance.versusInfo.playerSettings)
        {
            if (ps.playerID == playerInput.playerIndex)
            {
                playerSetting = ps;
                break;
            }
        }
        if (playerSetting == null)
        {
            Debug.LogError("No playerSetting found with playerIndex of " + playerInput.playerIndex);
        }
    }

    void updateCursor()
    {
        currentMovement = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector2 moveVector = new Vector2();
        moveVector.x = transform.position.x + currentMovement.x * cursorSpeed * Time.deltaTime;
        moveVector.y = transform.position.y + currentMovement.y * cursorSpeed * Time.deltaTime;
        moveVector = LimitCursorToCamera(moveVector.x, moveVector.y);

        transform.position = new Vector3(moveVector.x, moveVector.y, transform.position.z);
    }

    private Vector2 LimitCursorToCamera(float x, float y)
    {
        var bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        var topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight));
        // keep on screen
        x = Mathf.Clamp(x, bottomLeft.x + cursorPadding, topRight.x - cursorPadding);
        y = Mathf.Clamp(y, bottomLeft.y + cursorPadding, topRight.y - cursorPadding);
        return new Vector2(x, y);
    }

    private void OnEnable()
    {
        playerInput.actions["Move"].started += onMove;
        playerInput.actions["Move"].performed += onMove;
        playerInput.actions["Move"].canceled += onMove;

        playerInput.actions["Click"].started += onClick;

        playerInput.actions["Back"].started += onBack;

        playerInput.actions["Start"].started += onStart;
        playerInput.actions["Start"].canceled += onStart;
    }

    private void OnDisable()
    {
        playerInput.actions["Move"].started -= onMove;
        playerInput.actions["Move"].performed -= onMove;
        playerInput.actions["Move"].canceled -= onMove;

        playerInput.actions["Click"].started -= onClick;

        playerInput.actions["Back"].started -= onBack;

        playerInput.actions["Start"].started -= onStart;
        playerInput.actions["Start"].canceled -= onStart;
    }
}
