using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBox : MonoBehaviour
{
    public Character character;
    // Start is called before the first frame update
    void Start()
    {
        TextMesh characterText = transform.Find("Text").GetComponent<TextMesh>();
        characterText.text = character.ToString();
    }

    public Character GetCharacterName()
    {
        return character;
    }
}
