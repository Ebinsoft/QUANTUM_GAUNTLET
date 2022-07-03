using System.Linq;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public ImpactSoundElem[] impactSounds;
    Dictionary<ImpactSound, Sound> _impactSounds;

    public MovementSoundElem[] movementSounds;
    Dictionary<MovementSound, Sound> _movementSounds;

    void Start()
    {
        _impactSounds = impactSounds.ToDictionary(s => s.soundType, s => s.sound);
        _movementSounds = movementSounds.ToDictionary(s => s.soundType, s => s.sound);
    }

    // play an impact sound effect
    public void PlayAt(ImpactSound sound, Vector3 position)
    {
        Sound s = _impactSounds[sound];
        AudioSource.PlayClipAtPoint(s.clip, position, s.volume);
    }

    // play a movement sound effect
    public void PlayAt(MovementSound sound, Vector3 position)
    {
        Sound s = _movementSounds[sound];
        AudioSource.PlayClipAtPoint(s.clip, position, s.volume);
    }

    // play a custom sound effect
    public void PlayAt(Sound sound, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(sound.clip, position, sound.volume);
    }

}