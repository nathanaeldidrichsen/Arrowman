using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Clips")]
    [SerializeField] private AudioClip arrowHitEnemy;
    [SerializeField] private AudioClip arrowRelease;
    [SerializeField] private AudioClip arrowDraw;
    [SerializeField] private AudioClip arrowHitGround;
    [SerializeField] private AudioClip arrowHitCastle;
    [SerializeField] private AudioClip arrowShoot;
    [SerializeField] private AudioClip castleHurt;
    [SerializeField] private AudioClip error;
    [SerializeField] private AudioClip getCoin;
    [SerializeField] private AudioClip spawnWave;
    [SerializeField] private AudioClip upgradeSound;
    public AudioClip lavaSound;
    public AudioClip openGate;
    public AudioClip closeGate;




    [Header("Volume Settings")]
    [SerializeField, Range(0, 1)] private float arrowHitEnemyVolume = 1.0f;
    [SerializeField, Range(0, 1)] private float arrowReleaseVolume = 1.0f;
    [SerializeField, Range(0, 1)] private float arrowDrawVolume = 1.0f;
    [SerializeField, Range(0, 1)] private float arrowHitGroundVolume = 1.0f;
    [SerializeField, Range(0, 1)] private float arrowHitCastleVolume = 1.0f;
    [SerializeField, Range(0, 1)] private float arrowShootVolume = 1.0f;
    [SerializeField, Range(0, 1)] private float castleHurtVolume = 1.0f;
    [SerializeField, Range(0, 1)] private float errorVolume = 1.0f;
    [SerializeField, Range(0, 1)] private float getCoinVolume = 1.0f;
    [SerializeField, Range(0, 1)] private float spawnWaveVolume = 1.0f;
    [SerializeField, Range(0, 1)] private float upgradeSoundVolume = 1.0f;

    [Header("Settings")]
    [SerializeField, Range(0, 1)] private float globalVolume = 1f; // Default global volume
    [SerializeField] private int maxSimultaneousSounds = 5; // Limit of simultaneous sounds

    private List<AudioSource> audioSourcePool;
    private int currentAudioSourceIndex = 0;

    private void Awake()
    {
        // Implement Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: Keep the sound manager across scenes

        // Initialize AudioSource pool
        audioSourcePool = new List<AudioSource>();
        for (int i = 0; i < maxSimultaneousSounds; i++)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSourcePool.Add(audioSource);
        }
    }

    /// <summary>
    /// Plays a specified audio clip.
    /// </summary>
    /// <param name="clip">The AudioClip to play.</param>
    /// <param name="volume">Volume level for this specific clip (0 to 1).</param>
    public void PlaySound(AudioClip clip, float volume = 1.0f)
    {
        if (clip == null)
        {
            Debug.LogWarning("SoundManager: Attempted to play a null AudioClip.");
            return;
        }

        // Calculate final volume based on global and local settings
        float finalVolume = Mathf.Clamp01(globalVolume * volume);

        // Play the sound using the next available audio source
        AudioSource audioSource = audioSourcePool[currentAudioSourceIndex];
        audioSource.clip = clip;
        audioSource.volume = finalVolume;
        audioSource.Play();

        // Cycle through the audio source pool
        currentAudioSourceIndex = (currentAudioSourceIndex + 1) % maxSimultaneousSounds;
    }

    /// <summary>
    /// Sets the global volume for all sounds.
    /// </summary>
    /// <param name="newVolume">New global volume (0 to 1).</param>
    public void SetGlobalVolume(float newVolume)
    {
        globalVolume = Mathf.Clamp01(newVolume); // Ensure the volume is between 0 and 1
    }

    /// <summary>
    /// Gets the current global volume.
    /// </summary>
    /// <returns>The current global volume (0 to 1).</returns>
    public float GetGlobalVolume()
    {
        return globalVolume;
    }

    // Methods to play specific sounds using their clips with predefined volumes
    public void PlayArrowHitEnemy() => PlaySound(arrowHitEnemy, arrowHitEnemyVolume);
    public void PlayArrowRelease() => PlaySound(arrowRelease, arrowReleaseVolume);
    public void PlayArrowDraw() => PlaySound(arrowDraw, arrowDrawVolume);
    public void PlayArrowHitGround() => PlaySound(arrowHitGround, arrowHitGroundVolume);
    public void PlayArrowHitWall() => PlaySound(arrowHitCastle, arrowHitCastleVolume);
    public void PlayArrowShoot() => PlaySound(arrowShoot, arrowShootVolume);
    public void PlayCastleHurt() => PlaySound(castleHurt, castleHurtVolume);
    public void PlayError() => PlaySound(error, errorVolume);
    public void PlayGetCoin() => PlaySound(getCoin, getCoinVolume);
    public void PlaySpawnWave() => PlaySound(spawnWave, spawnWaveVolume);
    public void PlayUpgradeSound() => PlaySound(upgradeSound, upgradeSoundVolume);
}
