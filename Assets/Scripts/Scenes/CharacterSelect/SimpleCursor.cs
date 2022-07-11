using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SimpleCursor : MonoBehaviour
{
    public PlayerInput playerInput;
    public float cursorSpeed = 1000f;
    private Vector2 currentMovement;
    private bool isMovePressed;
    private PlayerSetting playerSetting;
    private SpriteRenderer sprite;
    private float cursorPadding = .25f;


    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        sprite = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        // Look up this cursor's relevant playerSetting from the Game Manager
        GetPlayerSetting();
    }

    private void onMove(InputAction.CallbackContext context)
    {
        isMovePressed = playerInput.actions["Move"].ReadValue<Vector2>().magnitude > 0;

    }

    private void onClick(InputAction.CallbackContext context)
    {
        Debug.Log("CLICK");

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit))
        {
            Debug.Log("Collider Hit");
            ChangeState(hit.collider.gameObject);
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
            playerSetting.characterName = characterBox.GetCharacterName();
        }

        GameTypeToggle gtt = go.transform.parent.gameObject.GetComponent<GameTypeToggle>();
        if (gtt != null)
        {
            Debug.Log("Hit Toggle");
            gtt.ToggleMode();
        }
        SetTeamButton stb = go.GetComponent<SetTeamButton>();
        if (stb != null)
        {
            Debug.Log("Team Button");
            playerSetting.team.teamName = stb.teamName;
            sprite.color = playerSetting.team.teamColor;
        }

        VersusStart vs = go.GetComponent<VersusStart>();
        if (vs != null)
        {
            Debug.Log("VERSUS START");
            vs.StartVersusMatch();
        }

        AIToggle ai = go.transform.parent.gameObject.GetComponent<AIToggle>();
        if (ai != null)
        {
            Debug.Log("AI BUTTON");
            ai.ToggleAI();
        }
    }

    private void onStart(InputAction.CallbackContext context)
    {
        Debug.Log("START");
    }
    // Update is called once per frame
    void Update()
    {
        UpdateCursorColor();
        if (isMovePressed)
        {
            updateCursor();
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
        // playerInput.actions["Click"].canceled += onClick;

        playerInput.actions["Start"].started += onStart;
        playerInput.actions["Start"].canceled += onStart;
    }

    private void OnDisable()
    {
        playerInput.actions["Move"].started -= onMove;
        playerInput.actions["Move"].performed -= onMove;
        playerInput.actions["Move"].canceled -= onMove;

        playerInput.actions["Click"].started -= onClick;
        // playerInput.actions["Click"].canceled -= onClick;

        playerInput.actions["Start"].started -= onStart;
        playerInput.actions["Start"].canceled -= onStart;
    }
}
