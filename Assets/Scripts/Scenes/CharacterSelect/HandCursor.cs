using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class HandCursor : MonoBehaviour
{
    public PlayerInput playerInput;
    private float cursorSpeed = 17f;

    private SkinnedMeshRenderer rend;
    private TextMeshPro label;
    private Animator anim;


    private Vector2 currentMovement;
    private bool isMovePressed;
    private PlayerSetting playerSetting;
    private float cursorPadding = .25f;

    public UnityEngine.Object tokenPrefab;
    private CharacterToken heldToken = null;
    private CharacterToken myToken = null;
    private List<GameObject> focusedElements;
    private bool isOverRoster = false;
    private bool isSummoning = false;
    private float timeSinceAccel = 0f;
    private float accelTime = .25f;
    private CharacterSelectManager cs;


    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        anim = transform.Find("Hand").GetComponent<Animator>();
        rend = transform.Find("Hand/Model").GetComponent<SkinnedMeshRenderer>();
        label = transform.Find("Hand/Hand/Label").GetComponent<TextMeshPro>();
        cs = GameObject.Find("CharacterSelectManager").GetComponent<CharacterSelectManager>();

        focusedElements = new List<GameObject>();
    }

    void Start()
    {
        // Look up this cursor's relevant playerSetting from the Game Manager
        GetPlayerSetting();
        label.text = "P" + (playerSetting.playerID + 1);

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
        if (!isMovePressed)
        {
            timeSinceAccel = 0f;
        }
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
                        anim.SetBool("HoldingToken", true);
                    }
                }
            }
            else
            {
                charBox.PlaceToken(heldToken);
                anim.SetBool("HoldingToken", false);
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
            anim.SetBool("OverButton", true);
        }

        if (other.gameObject.tag == "RosterZone")
        {
            isOverRoster = true;
            anim.SetBool("OverRoster", true);
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

            var focusedButtons = focusedElements.Where(e => e.GetComponent<IBasicButton>() != null);
            if (focusedButtons.Count() == 0)
            {
                anim.SetBool("OverButton", false);
            }
        }

        if (other.gameObject.tag == "RosterZone")
        {
            isOverRoster = false;
            anim.SetBool("OverRoster", false);
            if (heldToken != null)
            {
                heldToken.Hide();
            }
        }
    }

    private void onStart(InputAction.CallbackContext context)
    {
        Debug.Log("START");
    }

    private void Summon()
    {
        Vector3 p2 = myToken.transform.position;
        float summonMultiplier = 8f;
        Vector2 pos2D = Vector2.MoveTowards(transform.position, p2, summonMultiplier * cursorSpeed * Time.deltaTime);
        transform.position = new Vector3(pos2D.x, pos2D.y, transform.position.z);

        if (focusedElements.Contains(myToken.gameObject))
        {
            CharacterBox charBox = GetFocusedComponent<CharacterBox>();
            if (charBox != null)
            {
                charBox.RemoveToken(myToken);
                myToken.SetTarget(transform);
                heldToken = myToken;
                anim.SetBool("HoldingToken", true);
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
    }

    private void UpdateCursorColor()
    {
        GetPlayerSetting();
        label.color = playerSetting.team.teamColor;
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
        timeSinceAccel += Time.deltaTime / accelTime;
        timeSinceAccel = Mathf.Clamp(timeSinceAccel, 0.1f, 1f);
        Debug.Log(timeSinceAccel);
        currentMovement = playerInput.actions["Move"].ReadValue<Vector2>();
        currentMovement *= currentMovement.magnitude;
        currentMovement *= timeSinceAccel;
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
