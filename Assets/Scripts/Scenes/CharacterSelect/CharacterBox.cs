using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterBox : MonoBehaviour
{
    public Character character;
    private SpriteRenderer portrait;
    
    void Start()
    {
        TextMeshPro characterText = transform.Find("Name").GetComponent<TextMeshPro>();
        portrait = transform.Find("Portrait Mask/Portrait").GetComponent<SpriteRenderer>();
        characterText.text = character.ToString();
        portrait.sprite = GameManager.instance.roster.GetCharacter(character).portrait;
    }

    public Character GetCharacterName()
    {
        return character;
    }
}
