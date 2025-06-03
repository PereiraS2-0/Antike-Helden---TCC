using UnityEngine;
using TMPro;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance;

    public int currentXP = 0;
    public int xpToNextLevel = 10;

    public TextMeshProUGUI xpText;
    public GameObject upgradePanel; // Painel com as 3 opções
    private bool awaitingUpgrade = false;
    
    public GameObject poderSlot; // arrasta PoderSlot aqui
    public GameObject bracoDeZeusPrefab;
    public GameObject raioDeZeusPrefab;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddXP(int amount)
    {
        if (awaitingUpgrade) return;

        currentXP += amount;
        UpdateUI();

        if (currentXP >= xpToNextLevel)
        {
            TriggerUpgrade();
        }
    }

    void TriggerUpgrade()
    {
        Debug.Log("Subiu de nível! Mostrando opções de upgrade.");
        Time.timeScale = 0f;
        upgradePanel.SetActive(true);
        awaitingUpgrade = true;
    }

    public void ApplyUpgrade()
    {
        Debug.Log("Upgrade escolhido!");
        currentXP = 0;
        xpToNextLevel = Mathf.CeilToInt(xpToNextLevel * 1.5f); // aumenta a dificuldade
        UpdateUI();
        Time.timeScale = 1f;
        upgradePanel.SetActive(false);
        awaitingUpgrade = false;
    }
    public void ApplyUpgradeZeus()
    {
        Instantiate(bracoDeZeusPrefab, poderSlot.transform);
        Instantiate(raioDeZeusPrefab, poderSlot.transform);

        currentXP = 0;
        xpToNextLevel = Mathf.CeilToInt(xpToNextLevel * 1.5f);
        UpdateUI();
        Time.timeScale = 1f;
        upgradePanel.SetActive(false);
        awaitingUpgrade = false;
    }

    void UpdateUI()
    {
        xpText.text = "XP: " + currentXP + " / " + xpToNextLevel;
    }
}
