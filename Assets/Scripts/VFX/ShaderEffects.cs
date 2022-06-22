using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderEffects : MonoBehaviour
{
    private Renderer rend;

    private Color intangibleTint = new Color(0.89f, 0.95f, 1.0f);
    private Color armoredTint = new Color(0.6f, 0.47f, 1.0f);
    private Color invulnerableTint = new Color(1.0f, 0.88f, 0.4f);

    void Start()
    {
        rend = transform.Find("Body").GetComponent<Renderer>();
    }

    public void ApplyStatusFlicker(PlayerStats.Status newStatus)
    {
        switch (newStatus)
        {
            case PlayerStats.Status.normal:
                rend.material.SetFloat("_FlickerEnabled", 0);
                break;

            case PlayerStats.Status.intangible:
                rend.material.SetFloat("_FlickerEnabled", 1);
                rend.material.SetColor("_FlickerColor", intangibleTint);
                break;

            case PlayerStats.Status.armored:
                rend.material.SetFloat("_FlickerEnabled", 1);
                rend.material.SetColor("_FlickerColor", armoredTint);
                break;

            case PlayerStats.Status.invulnerable:
                rend.material.SetFloat("_FlickerEnabled", 1);
                rend.material.SetColor("_FlickerColor", invulnerableTint);
                break;
        }
    }
}
