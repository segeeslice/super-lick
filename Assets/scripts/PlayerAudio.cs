using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Sources")] // sources are like speakers
    public AudioSource sfxSource;

    [Header("Clips")] // clips are the actual sfx themselves
    public AudioClip[] footstepClips;
    public AudioClip jumpClip;
    public AudioClip lickClip;
    public AudioClip flipClip;
    public AudioClip pickupClip;
    public AudioClip winClip;

    // This script has to be on the same object as the Animator object in order for the Anim Event to fire this
    public void PlayFootstep()
    {
        if (footstepClips.Length == 0) return;

        var clip = footstepClips[Random.Range(0, footstepClips.Length)];
        sfxSource.PlayOneShot(clip);
    }

    public void PlayJumpSound()
    {
        sfxSource.PlayOneShot(jumpClip);
    }

    public void PlayLickSound()
    {
        sfxSource.PlayOneShot(lickClip);
    }

    public void PlayFlipSound()
    {
        sfxSource.PlayOneShot(flipClip);
    }

    public void PlayPickupSound()
    {
        sfxSource.PlayOneShot(pickupClip);
    }

    public void PlayWinSound()
    {
        sfxSource.PlayOneShot(winClip);
    }

}
