using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public Text fpsText;
    public float deltaTime;
    private float fps = 0.0f;
    private float FPS = 5;

    void Start()
    {
        StartCoroutine(updateUI());
    }
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;
    }

    IEnumerator updateUI()
    {
        while (true)
        {
            fpsText.text = Mathf.Ceil(fps).ToString();
            yield return new WaitForSeconds(.2f);
        }

    }
}