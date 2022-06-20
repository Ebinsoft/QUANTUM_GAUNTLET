using System;
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

    public void PlayRecoilLag(float duration)
    {
        StartCoroutine(AnimatorLag(duration, false));
    }

    public void PlayHitLag(float duration, Action onComplete = null)
    {
        transform.root.GetComponent<PlayerManager>().isHitLagging = true;
        StartCoroutine(Shake(duration, duration, 75f));
        StartCoroutine(AnimatorLag(duration, true, onComplete: onComplete));
    }

    private IEnumerator AnimatorLag(float duration, bool resetHitlagFlag, Action onComplete = null)
    {
        // pause animator
        anim.speed = 0;
        yield return new WaitForSeconds(duration);

        // return to normal speed
        anim.speed = 1;

        if (resetHitlagFlag)
        {
            transform.root.GetComponent<PlayerManager>().isHitLagging = false;
        }

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
}
