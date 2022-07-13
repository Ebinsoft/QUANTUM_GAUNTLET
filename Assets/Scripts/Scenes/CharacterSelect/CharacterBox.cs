using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterBox : MonoBehaviour
{
    public Character character;
    private SpriteRenderer portrait;
    private List<CharacterToken> placedTokens;
    private SpriteRenderer panelSprite;

    void Start()
    {
        TextMeshPro characterText = transform.Find("Name").GetComponent<TextMeshPro>();
        portrait = transform.Find("Portrait Mask/Portrait").GetComponent<SpriteRenderer>();
        panelSprite = GetComponent<SpriteRenderer>();
        characterText.text = character.ToString();
        portrait.sprite = GameManager.instance.roster.GetCharacter(character).portrait;

        placedTokens = new List<CharacterToken>();
    }

    public Character GetCharacterName()
    {
        return character;
    }

    public void PlaceToken(CharacterToken token)
    {
        token.lastCharacterBox = this;
        placedTokens.Add(token);
        DistributeTokens();
    }

    public void RemoveToken(CharacterToken token)
    {
        placedTokens.Remove(token);
        DistributeTokens();
    }

    private void DistributeTokens()
    {
        if (placedTokens.Count == 0) return;

        if (placedTokens.Count == 1)
        {
            placedTokens[0].SetTarget(transform.position);
            return;
        }

        float angleBetween = 360 / placedTokens.Count;
        float tokenRadius = placedTokens[0].sprite.bounds.size.x / 2;
        float distance = (panelSprite.bounds.size.x / 2) - tokenRadius;

        for (int i = 0; i < placedTokens.Count; i++)
        {
            float rads = Mathf.Deg2Rad * (angleBetween * i);
            Vector2 pos = new Vector2(Mathf.Cos(rads), Mathf.Sin(rads)) * distance;

            Vector3 pos3 = new Vector3(pos.x, pos.y, placedTokens[i].transform.position.z);
            placedTokens[i].SetTarget(transform.position + pos3);
        }
    }
}
