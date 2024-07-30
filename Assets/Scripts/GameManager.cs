using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int gold;
    [SerializeField] private int health;
    [SerializeField] private int arrows;
    [SerializeField] private GameObject gatePosition;
    [SerializeField] private GameObject towerPosition;
    private bool isAtGate = true;



    public static GameManager Instance { get; private set; }

    public event Action<int> OnGoldChanged;
    public event Action<int> OnHealthChanged;
    public event Action<int> OnArrowsChanged;
    public CameraShake cameraShake;

    private void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        PauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void BuyArrow()
    {
        if (gold > 0) // Example cost
        {
            Gold--; // Deduct cost
            Arrows += 5; // Add arrow
            // SoundManager.Instance.PlayGetCoin();
            SoundManager.Instance.PlayUpgradeSound();

            // Update UI if needed
        }
        else
        {
            SoundManager.Instance.PlayError();
        }
    }

    public void MoveToGate()
    {
        if (gold >= 5 && !isAtGate) // Example cost
        {
            Gold -= 5; // Deduct cost
            Player.Instance.Damage++;
            SoundManager.Instance.PlayGetCoin();
            Player.Instance.transform.position = gatePosition.transform.position;
            isAtGate = true;
        }
        else
        {
            SoundManager.Instance.PlayError();
        }
    }

        public void MoveToTower()
    {
        if (gold >= 20 && isAtGate) // Example cost
        {
            Gold -= 20; // Deduct cost
            Player.Instance.Damage++;
            SoundManager.Instance.PlayGetCoin();
            Player.Instance.transform.position = towerPosition.transform.position;
            isAtGate = false;
        }
        else
        {
            SoundManager.Instance.PlayError();
        }
    }

            public void RebuildCastle()
    {
        if (gold >= 30) // Example cost
        {
            Gold -= 30; // Deduct cost
            Health = 15;
            SoundManager.Instance.PlayGetCoin();
        }
        else
        {
            SoundManager.Instance.PlayError();
        }
    }




    public void UpgradeDamage()
    {
        if (gold >= 20) // Example cost
        {
            Gold -= 20; // Deduct cost
            Player.Instance.Damage++;
            SoundManager.Instance.PlayUpgradeSound();
        }
        else
        {
            SoundManager.Instance.PlayError();
        }
    }

    public void ReloadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public int Gold
    {
        get => gold;
        set
        {
            if (gold != value)
            {
                gold = value;
                OnGoldChanged?.Invoke(gold);  // Trigger the event
            }
        }
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        SoundManager.Instance.PlayGetCoin();
    }

    public void LooseHealth(int amount)
    {
        if (health > amount)
        {
            Health -= amount;
        }
        else
        {
            Health = 0;
            OnHealthChanged?.Invoke(health);  // Trigger the event
            LostGame();
            //Play lost sound

        }
    }

    public void LostGame()
    {
            HUD.Instance.DisplayLoosePanel();

    }

    public int Health
    {
        get => health;
        set
        {
            if (health != value)
            {
                cameraShake.ScreenShake();
                health = value;
                OnHealthChanged?.Invoke(health);  // Trigger the event
            }
        }
    }

    public int Arrows
    {
        get => arrows;
        set
        {
            if (arrows != value)
            {
                arrows = value;
                OnArrowsChanged?.Invoke(arrows);  // Trigger the event
            }
        }
    }
}
