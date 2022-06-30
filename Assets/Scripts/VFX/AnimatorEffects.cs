using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEffects : MonoBehaviour
{
    private Animator anim;

    private IEnumerator ShakeRoutine;
    private IEnumerator AnimatorLagRoutine;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Freezes the player's animator for a set duration
    public void PlayRecoilLag(float duration)
    {
        StartCoroutine(AnimatorLag(duration));
    }

    // Freezes the player's animator and shakes him around for a set duration
    // If provided, onComplete will be run after the hitlag terminates
    public void PlayHitLag(float duration, Action onComplete = null)
    {
        // stop any currently running coroutines
        if (ShakeRoutine != null) StopCoroutine(ShakeRoutine);
        if (AnimatorLagRoutine != null) StopCoroutine(AnimatorLagRoutine);

        // reinitalize and run new coroutines
        ShakeRoutine = Shake(duration, duration, 75f);
        StartCoroutine(ShakeRoutine);

        AnimatorLagRoutine = AnimatorLag(duration, onComplete: onComplete);
        StartCoroutine(AnimatorLagRoutine);
    }

    private IEnumerator AnimatorLag(float duration, Action onComplete = null)
    {
        // pause animator
        anim.speed = 0;
        yield return new WaitForSeconds(duration);

        // return to normal speed
        anim.speed = 1;

        if (onComplete != null)
        {
            onComplete();
        }
    }

    private IEnumerator Shake(float duration, float magnitude, float speed)
    {
        Vector3 originalPos = Vector3.zero;
        float timeStart = Time.time;

        while (Time.time < timeStart + duration)
        {
            Vector3 newPos = originalPos;
            newPos.z += Mathf.Sin(Time.time * speed) * magnitude;
            anim.gameObject.transform.localPosition = newPos;

            yield return 0;
        }
        anim.gameObject.transform.localPosition = originalPos;
    }

    public void CancelHit()
    {
        StopAllCoroutines();
        anim.speed = 1;
    }
}

