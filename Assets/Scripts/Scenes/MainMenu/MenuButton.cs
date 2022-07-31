using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
{
    Button button;
    Animator buttonAnim;

    // 3d icon
    public GameObject iconObject;
    Animator iconAnim;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        buttonAnim = GetComponent<Animator>();

        iconAnim = iconObject.GetComponent<Animator>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        buttonAnim.SetBool("IsSelected", true);
        iconAnim.SetBool("IsActive", true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Sound s = AudioManager.UISounds[UISound.MainHover];
        AudioManager.PlayAt(s, transform.parent.gameObject);

        buttonAnim.SetBool("IsSelected", false);
        iconAnim.SetBool("IsActive", false);
    }

    public void OnClick()
    {
        buttonAnim.SetTrigger("Click");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
