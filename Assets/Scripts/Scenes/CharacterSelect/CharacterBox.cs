using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterBox : MonoBehaviour
{
    public Character character;
    // Start is called before the first frame update
    void Start()
    {
        TextMeshPro characterText = transform.Find("Name").GetComponent<TextMeshPro>();
        characterText.text = character.ToString();
    }

    public Character GetCharacterName()
    {
        return character;
    }
}
