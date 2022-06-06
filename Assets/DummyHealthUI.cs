using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DummyHealthUI : MonoBehaviour
{
    public Text healthText;
    public DummyHealth dummy;
    // Start is called before the first frame update
    void Start()
    {
        healthText.text = "NULL";
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = dummy.currentHealth.ToString();
    }
}
