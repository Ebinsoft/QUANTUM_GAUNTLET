using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public AudioClip clip;

    [Range(0, 1)]
    public float volume;
}


public class InterruptableSound
{
    public GameObject gameObject;
    public AudioSource audioSource;

    ~InterruptableSound()
    {
        StopAndDestroy();
    }

    public void Play()
    {
        audioSource.Play();
    }

    public void StopAndDestroy()
    {
        if (gameObject == null) return;

        audioSource.Stop();
        GameObject.Destroy(gameObject);
    }
}


// IMPACT SOUNDS
public enum ImpactSound
{
    PunchLight,
    PunchMedium,
    PunchHeavy,
    KickLight,
    KickMedium,
    KickHeavy
}

[System.Serializable]
public class ImpactSoundElem
{
    public ImpactSound soundType;
    public Sound sound;
}


// MISC SOUNDS
public enum MiscAttackSound
{
    WhooshLight,
    WhooshHeavy
}
[System.Serializable]
public class MiscAttackSoundElem
{
    public MiscAttackSound soundType;
    public Sound sound;
}

// MOVEMENT SOUNDS
public enum MovementSound
{
    Footstep,
    Dash,
    Land,
    Crash
}

[System.Serializable]
public class MovementSoundElem
{
    public MovementSound soundType;
    public Sound sound;
}


// FIRE SOUNDS
public enum FireSound
{
    FlameBurst,
    ExplosionSmall,
    ExplosionMedium,
    ExplosionBig,
    DeathExplosion
}

[System.Serializable]
public class FireSoundElem
{
    public FireSound soundType;
    public Sound sound;
}


// MAGIC SOUNDS
public enum MagicSound
{
    ChargeUp,
    Zoom,
    Powerup
}

[System.Serializable]
public class MagicSoundElem
{
    public MagicSound soundType;
    public Sound sound;
}

// UI SOUNDS
public enum UISound
{
    MainHover,
    MainClick,
    CSHover,
    CSClick,
    CSFightReady,
    CSFightClick,
    PowerToggle
}

[System.Serializable]
public class UISoundElem
{
    public UISound soundType;
    public Sound sound;
}