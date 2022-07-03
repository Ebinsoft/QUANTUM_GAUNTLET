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

// IMPACT SOUNDS
public enum ImpactSound
{
    PunchLight,
    PunchMedium,
    PunchHeavy
}

[System.Serializable]
public class ImpactSoundElem
{
    public ImpactSound soundType;
    public Sound sound;
}

// MOVEMENT SOUNDS
public enum MovementSound
{
    Footstep,
    GroundJump,
    AirJump,
    Land,
    Crash
}

[System.Serializable]
public class MovementSoundElem
{
    public MovementSound soundType;
    public Sound sound;
}