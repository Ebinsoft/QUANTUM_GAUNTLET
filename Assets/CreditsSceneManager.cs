using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class CreditsSceneManager : MonoBehaviour
{
    public GameObject scrollingText;
    private GameObject gameTitleText;
    public TextMeshProUGUI exitText;
    public TextMeshProUGUI leavingSceneText;
    private PlayerInputManager playerInputManager;
    private PlayerInput playerInput;
    private List<GameObject> players;
    public float scrollSpeed;
    // Start is called before the first frame update
    void Awake()
    {
        gameTitleText = scrollingText.transform.Find("Game Title").gameObject;
        players = new List<GameObject>();
        playerInputManager = GetComponent<PlayerInputManager>();
    }
    void Update()
    {
        ScrollText();
        exitText.SetText(DisplayExitText());
    }

    public void ScrollText()
    {
        Vector3 p = scrollingText.transform.position;
        p.y += (Time.deltaTime * scrollSpeed);
        scrollingText.transform.position = p;
    }

    public string DisplayExitText()
    {
        string text = "";
        foreach (var player in players)
        {
            string inputString = player.GetComponent<PlayerInput>().actions.FindAction("Start").GetBindingDisplayString();
            text += "Press " + inputString + " to exit (P" + (players.IndexOf(player) + 1) + ")\n";
        }
        return text;
    }
    public void Outro()
    {
        GameManager.instance.TransitionToScene("MainMenu");
    }

    public void ActivateGameTitle()
    {
        gameTitleText.SetActive(true);
    }

    private void onStart(InputAction.CallbackContext context)
    {
        Outro();
    }
    public string GetPlayerBindingName(string action)
    {
        string binding = playerInput.actions.FindAction(action).GetBindingDisplayString();
        return binding;
    }
    public void OnPlayerJoined(PlayerInput p)
    {
        players.Add(p.gameObject);
        p.actions["Start"].started += onStart;
    }
}
