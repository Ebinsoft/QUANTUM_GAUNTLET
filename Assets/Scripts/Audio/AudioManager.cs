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

    public UISoundElem[] _UISounds;
    public static Dictionary<UISound, Sound> UISounds;
    public AudioMixer masterMixer;
    public AudioMixerGroup sfxMixerGroup;
    public AudioMixerGroup musicMixerGroup;

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        miscAttackSounds = _miscAttackSounds.ToDictionary(s => s.soundType, s => s.sound);
        impactSounds = _impactSounds.ToDictionary(s => s.soundType, s => s.sound);
        movementSounds = _movementSounds.ToDictionary(s => s.soundType, s => s.sound);
        fireSounds = _fireSounds.ToDictionary(s => s.soundType, s => s.sound);
        magicSounds = _magicSounds.ToDictionary(s => s.soundType, s => s.sound);
        UISounds = _UISounds.ToDictionary(s => s.soundType, s => s.sound);
    }

    // play misc attack sound effect
    public static void PlayAt(MiscAttackSound sound, GameObject obj)
    {
        Sound s = miscAttackSounds[sound];
        AudioSource source = GetAudioSource(obj);
        source.clip = s.clip;
        source.Play();
    }

    // play an impact sound effect
    public static void PlayAt(ImpactSound sound, GameObject obj)
    {
        Sound s = impactSounds[sound];
        AudioSource source = GetAudioSource(obj);
        source.clip = s.clip;
        source.Play();
    }

    // play a movement sound effect
    public static void PlayAt(MovementSound sound, GameObject obj)
    {
        Sound s = movementSounds[sound];
        AudioSource source = GetAudioSource(obj);
        source.clip = s.clip;
        source.Play();
    }

    // play a fire sound effect
    public static void PlayAt(FireSound sound, GameObject obj)
    {
        Sound s = fireSounds[sound];
        AudioSource source = GetAudioSource(obj);
        source.clip = s.clip;
        source.Play();
    }

    // play a magic sound effect
    public static void PlayAt(MagicSound sound, GameObject obj)
    {
        Sound s = magicSounds[sound];
        AudioSource source = GetAudioSource(obj);
        source.clip = s.clip;
        source.Play();
    }

    // play a UI sound effect
    public static void PlayAt(UISound sound, GameObject obj)
    {
        Sound s = UISounds[sound];
        AudioSource source = GetAudioSource(obj);
        source.clip = s.clip;
        source.Play();
    }

    // play 2D sounds via the AudioManager's source, uses PlayOneShot so sounds dont interrupt each other
    public static void Play2D(Sound s)
    {
        instance.audioSource.PlayOneShot(s.clip, s.volume);
    }

    // play a custom sound effect
    public static void PlayAt(Sound sound, GameObject obj)
    {
        AudioSource source = GetAudioSource(obj);
        source.clip = sound.clip;
        source.Play();
    }

    public static AudioSource GetAudioSource(GameObject obj)
    {
        return obj.GetComponent<AudioSource>();
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
        source.outputAudioMixerGroup = AudioManager.instance.sfxMixerGroup;
        source.playOnAwake = false;

        InterruptableSound interruptableSound = new InterruptableSound();
        interruptableSound.gameObject = obj;
        interruptableSound.audioSource = source;


        return interruptableSound;
    }

}