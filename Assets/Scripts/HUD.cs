using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    private GameManager gameManager;
    private Player player;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI arrowsText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private GameObject loosePanel;
    private Animator anim;
    public static HUD Instance { get; private set; }

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
        anim = GetComponent<Animator>();
        gameManager = GameManager.Instance;
        player = Player.Instance;

        // Subscribe to GameManager events
        gameManager.OnGoldChanged += UpdateGoldText;
        gameManager.OnHealthChanged += UpdateHealthText;
        gameManager.OnArrowsChanged += UpdateArrowsText;
        player.OnDamageChanged += UpdateDamageText;


        // Initialize UI
        UpdateGoldText(gameManager.Gold);
        UpdateHealthText(gameManager.Health);
        UpdateArrowsText(gameManager.Arrows);
        UpdateArrowsText(player.damage);

    }

    void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        if (gameManager != null)
        {
            gameManager.OnGoldChanged -= UpdateGoldText;
            gameManager.OnHealthChanged -= UpdateHealthText;
            gameManager.OnArrowsChanged -= UpdateArrowsText;
            player.OnDamageChanged -= UpdateDamageText;

        }
    }

    public void DisplayLoosePanel()
    {
        // activate some loose menu
        loosePanel.SetActive(true);
        Time.timeScale = 0;
    }

        private void UpdateDamageText(int newDamage)
    {
        damageText.text = $"Damage: {newDamage}";
    }

    private void UpdateGoldText(int newGold)
    {
        goldText.text = $"Gold: {newGold}";
        anim.Play("get_gold");
    }

    private void UpdateHealthText(int newHealth)
    {
        healthText.text = $"Health: {newHealth}";
    }

    private void UpdateArrowsText(int newArrows)
    {
        arrowsText.text = $"Arrows: {newArrows}";
    }
}
