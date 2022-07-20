using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEffects : MonoBehaviour
{
    private PlayerManager player;
    private Animator anim;

    private IEnumerator ShakeRoutine;
    private IEnumerator FreezeRoutine;

    void Start()
    {
        player = transform.root.GetComponent<PlayerManager>();
        anim = GetComponent<Animator>();
    }

    // Freezes the player's animator for a set duration
    public void PlayRecoilLag(float duration)
    {
        StartCoroutine(FreezePlayer(duration));
    }

    // Freezes the player's animator and shakes him around for a set duration
    // If provided, onComplete will be run after the hitlag terminates
    public void PlayHitLag(float duration, Action onComplete = null)
    {
        // stop any currently running coroutines
        if (ShakeRoutine != null) StopCoroutine(ShakeRoutine);
        if (FreezeRoutine != null) StopCoroutine(FreezeRoutine);

        // reinitalize and run new coroutines
        ShakeRoutine = Shake(duration, duration, 75f);
        StartCoroutine(ShakeRoutine);

        FreezeRoutine = FreezePlayer(duration, onComplete: onComplete);
        StartCoroutine(FreezeRoutine);
    }

    private IEnumerator FreezePlayer(float duration, Action onComplete = null)
    {
        // pause animator
        anim.speed = 0;

        // disable gravity
        var yVelocity = player.currentMovement.y;
        player.DisableGravity();

        player.DisableMovement();

        yield return new WaitForSeconds(duration);

        // return to normal speed
        anim.speed = 1;

        // re-enable gravity
        player.EnableGravity();
        player.currentMovement.y = yVelocity;

        player.EnableMovement();

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
        player.EnableGravity();
        player.EnableMovement();
    }
}

