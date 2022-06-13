using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEffects : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayHitlag(float duration)
    {
        StartCoroutine(Hitlag(duration));
    }

    private IEnumerator Hitlag(float duration)
    {
        // pause animator
        anim.speed = 0;
        yield return new WaitForSeconds(duration);

        // play animator at 2x speed for same duration to catch up
        anim.speed = 2;
        yield return new WaitForSeconds(duration);

        // return to normal speed
        anim.speed = 1;
    }
}
