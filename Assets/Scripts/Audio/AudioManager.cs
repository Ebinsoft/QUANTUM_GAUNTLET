using System.Linq;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // singleton instance
    [HideInInspector]
    public static AudioManager instance = null;

    public MiscAttackSoundElem[] _miscAttackSounds;
    public static Dictionary<MiscAttackSound, Sound> miscAttackSounds;

    public ImpactSoundElem[] _impactSounds;
    public static Dictionary<ImpactSound, Sound> impactSounds;

    public MovementSoundElem[] _movementSounds;
    public static Dictionary<MovementSound, Sound> movementSounds;

    public FireSoundElem[] _fireSounds;
    public static Dictionary<FireSound, Sound> fireSounds;

    public MagicSoundElem[] _magicSounds;
    public static Dictionary<MagicSound, Sound> magicSounds;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        miscAttackSounds = _miscAttackSounds.ToDictionary(s => s.soundType, s => s.sound);
        impactSounds = _impactSounds.ToDictionary(s => s.soundType, s => s.sound);
        movementSounds = _movementSounds.ToDictionary(s => s.soundType, s => s.sound);
        fireSounds = _fireSounds.ToDictionary(s => s.soundType, s => s.sound);
        magicSounds = _magicSounds.ToDictionary(s => s.soundType, s => s.sound);
    }

    // play misc attack sound effect
    public static void PlayAt(MiscAttackSound sound, Vector3 position)
    {
        Sound s = miscAttackSounds[sound];
        AudioSource.PlayClipAtPoint(s.clip, position, s.volume);
    }

    // play an impact sound effect
    public static void PlayAt(ImpactSound sound, Vector3 position)
    {
        Sound s = impactSounds[sound];
        AudioSource.PlayClipAtPoint(s.clip, position, s.volume);
    }

    // play a movement sound effect
    public static void PlayAt(MovementSound sound, Vector3 position)
    {
        Sound s = movementSounds[sound];
        AudioSource.PlayClipAtPoint(s.clip, position, s.volume);
    }

    // play a fire sound effect
    public static void PlayAt(FireSound sound, Vector3 position)
    {
        Sound s = fireSounds[sound];
        AudioSource.PlayClipAtPoint(s.clip, position, s.volume);
    }

    // play a magic sound effect
    public static void PlayAt(MagicSound sound, Vector3 position)
    {
        Sound s = magicSounds[sound];
        AudioSource.PlayClipAtPoint(s.clip, position, s.volume);
    }

    // play a custom sound effect
    public static void PlayAt(Sound sound, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(sound.clip, position, sound.volume);
    }


    // Create a temporary game object that plays a sound that can be interrupted
    public static InterruptableSound CreateInterruptable(Sound sound, Vector3? position = null, Transform parent = null)
    {
        GameObject obj = new GameObject("Interruptable Sound");

        if (parent != null)
        {
            obj.transform.SetParent(parent);
        }

        if (position.HasValue)
        {
            obj.transform.position = position.Value;
        }

        AudioSource source = obj.AddComponent<AudioSource>();
        source.clip = sound.clip;
        source.volume = sound.volume;
        source.playOnAwake = false;

        InterruptableSound interruptableSound = new InterruptableSound();
        interruptableSound.gameObject = obj;
        interruptableSound.audioSource = source;


        return interruptableSound;
    }

}