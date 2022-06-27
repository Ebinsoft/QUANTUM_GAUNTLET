using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBox : MonoBehaviour
{
    public string characterName;
    // Start is called before the first frame update
    void Start()
    {
        TextMesh characterText = transform.Find("Text").GetComponent<TextMesh>();
        characterName = characterText.text;
    }

    public string GetCharacterName()
    {
        return characterName;
    }
}
