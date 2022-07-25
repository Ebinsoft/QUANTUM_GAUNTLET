using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public Roster roster;
    public VersusInfo versusInfo;

    // scene transition stuff
    Animator screenWipeAnim;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        screenWipeAnim = transform.Find("Canvas/ScreenWipes").GetComponent<Animator>();
    }

    public void TransitionToScene(string sceneName)
    {
        // disable the event system so that you can't button mash and break the game
        EventSystem.current.enabled = false;

        StartCoroutine(PlaySceneTransition(sceneName));
    }

    IEnumerator PlaySceneTransition(string sceneName)
    {
        // wait until screenwipe is finished animating
        screenWipeAnim.SetBool("IsActive", true);
        while (!screenWipeAnim.GetCurrentAnimatorStateInfo(0).IsName("Active"))
        {
            yield return null;
        }

        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(sceneName);

        // wait until new scene is done loading
        while (sceneLoad.progress < 1f)
        {
            yield return null;
        }

        screenWipeAnim.SetBool("IsActive", false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
