using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicPlayer;
    public AudioSource effectPlayer;

    public AudioClip buttonClickClip;
    public AudioClip dragAndDropClip;
    public AudioClip deleteBlockClip;
    public AudioClip levelCompleteClip;
    public AudioClip levelFailedClip;

    public AudioClip movementClip;
    public AudioClip takeTreasureClip;
    public AudioClip coinCollectedClip;
    public AudioClip explosionClip;

    public bool audioEnabled;

    static AudioManager instance;

    public static AudioManager Instance { get { return instance; } }

	void Start ()
    {
        instance = this;

        audioEnabled = true;
        if (musicPlayer)
        {
            musicPlayer.Play();
        }
	}

	void Update ()
    {
	
	}

    public void ChangeAudioState()
    {
        if (audioEnabled)
        {
            audioEnabled = false;

            if (musicPlayer)
            {
                musicPlayer.Pause();
            }
            if (effectPlayer)
            {
                effectPlayer.Stop();
            }
        }
        else
        {
            audioEnabled = true;

            if (musicPlayer)
            {
                musicPlayer.Play();
            }
        }
    }

    public void StopMusicBackground()
    {
        if (musicPlayer)
        {
            musicPlayer.Stop();
        }
    }

    public void PlayMusicBackground()
    {
        if (musicPlayer)
        {
            musicPlayer.Play();
        }
    }

    public void PlayButtonClick()
    {
        if (buttonClickClip && audioEnabled)
        {
            effectPlayer.clip = buttonClickClip;
            effectPlayer.Play();
        }
    }

    public void PlayDragDrop()
    {
        if (dragAndDropClip && audioEnabled)
        {
            effectPlayer.clip = dragAndDropClip;
            effectPlayer.Play();
        }
    }

    public void PlayDeleteBlock()
    {
        if (deleteBlockClip && audioEnabled)
        {
            effectPlayer.clip = deleteBlockClip;
            effectPlayer.Play();
        }
    }

    public void PlayLevelComplete()
    {
        if (levelCompleteClip && audioEnabled)
        {
            StopMusicBackground();
            effectPlayer.clip = levelCompleteClip;
            effectPlayer.Play();
        }
    }

    public void PlayLevelFailed()
    {
        if (levelFailedClip && audioEnabled)
        {
            StopMusicBackground();
            effectPlayer.clip = levelFailedClip;
            effectPlayer.Play();
        }
    }

    public void PlayMovement()
    {
        if (movementClip && audioEnabled)
        {
            effectPlayer.clip = movementClip;
            effectPlayer.Play();
        }
    }

    public void PlayTakeTreasure()
    {
        if (takeTreasureClip && audioEnabled)
        {
            effectPlayer.clip = takeTreasureClip;
            effectPlayer.Play();
        }
    }

    public void PlayCoinCollected()
    {
        if (coinCollectedClip && audioEnabled)
        {
            effectPlayer.clip = coinCollectedClip;
            effectPlayer.Play();
        }
    }

    public void PlayExplosion()
    {
        if (explosionClip && audioEnabled)
        {
            effectPlayer.clip = explosionClip;
            effectPlayer.Play();
        }
    }
}
