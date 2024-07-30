using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*This script can be used on pretty much any gameObject. It provides several functions that can be called with 
animation events in the animation window.*/

public class AnimatorFunctions : MonoBehaviour
{
    // [SerializeField] private AudioSource audioSource;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private Animator setBoolInAnimator;

    // If we don't specify what audio source to play sounds through, just use the one on player.
    void Start()
    {
        // if (!audioSource) audioSource = NewPlayer.Instance.audioSource;
    }

    //Play a sound through the specified audioSource
    void PlaySound(AudioClip whichSound)
    {
        // SoundManager.Instance.PlaySound(SoundManager.Instance.lavaSound, 0.3f);
    }

    void PlaylavaSound()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.lavaSound, 0.2f);
    }

        void PlayGateSound()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.openGate, 0.1f);
    }


    public void EmitParticles(int amount)
    {
        particleSystem.Emit(amount);
    }

    public void ScreenShake()
    {
        // NewPlayer.Instance.cameraEffects.Shake(power, 1f);
        GameManager.Instance.cameraShake.ScreenShake();
    }

    public void SetTimeScale(float time)
    {
        Time.timeScale = time;
    }

    public void SetAnimBoolToFalse(string boolName)
    {
        setBoolInAnimator.SetBool(boolName, false);
    }

    public void SetAnimBoolToTrue(string boolName)
    {
        setBoolInAnimator.SetBool(boolName, true);
    }

    public void LoadScene(string whichLevel)
    {
        SceneManager.LoadScene(whichLevel);
    }

    //Slow down or speed up the game's time scale!
    public void SetTimeScaleTo(float timeScale)
    {
        Time.timeScale = timeScale;
    }
}
    