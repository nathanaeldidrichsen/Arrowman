using UnityEngine;

public class Castle : MonoBehaviour
{
    public static Castle Instance { get; private set; }

    private GameManager gameManager;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        // Implement Singleton pattern
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
        gameManager = GameManager.Instance;
    }

    // Example method to handle health reduction
    public void TakeDamage(int damage)
    {
        SoundManager.Instance.PlayCastleHurt();
        gameManager.Health -= damage;
        anim.SetTrigger("hurt");

        if (gameManager.Health <= 0)
        {
            gameManager.Health = 0;
            Die();
        }
    }

    public void PourLava()
    {
        if (GameManager.Instance.Gold >= 40)
        {
            anim.Play("pour_lava 1");
            GameManager.Instance.Gold -= 40;
        }
        else
        {
            SoundManager.Instance.PlayError();
        }

    }

    public void OpenGate()
    {
        if (GameManager.Instance.Gold >= 20)
        {
            anim.Play("open_gate");
            GameManager.Instance.Gold -= 20;
        }
        else
        {
            SoundManager.Instance.PlayError();
        }
    }

    public void CloseGate()
    {
        if (GameManager.Instance.Gold >= 10)
        {
            anim.Play("close_gate");
            GameManager.Instance.Gold -= 10;
        }
        else
        {
            SoundManager.Instance.PlayError();
        }
    }

    private void Die()
    {
        // Handle castle death logic here
        Debug.Log("Castle has been destroyed!");
        gameManager.LostGame();
        // Destroy(gameObject);
    }
}
