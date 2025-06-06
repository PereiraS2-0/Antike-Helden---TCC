using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance;

    public int currentXP = 0;
    public int xpToNextLevel = 10;

    public TextMeshProUGUI xpText;
    public GameObject upgradePanel;
    private bool awaitingUpgrade = false;

    [Header("UI de Upgrade")]
    public Button[] powerButtons;
    public Image[] powerIcons;
    public TextMeshProUGUI[] powerNames;
    public TextMeshProUGUI[] powerDescriptions;
    public TextMeshProUGUI[] powerLevels;

    [Header("Lista de Poderes")]
    public List<Power> allPowers = new List<Power>();
    private Power[] currentOfferedPowers = new Power[3];

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Inicializa��o segura do array
        currentOfferedPowers = new Power[3];
    }

    public void AddXP(int amount)
    {
        if (awaitingUpgrade || currentXP >= xpToNextLevel) return;

        currentXP = Mathf.Min(currentXP + amount, xpToNextLevel);
        UpdateUI();

        if (currentXP >= xpToNextLevel)
        {
            TriggerUpgrade();
        }
    }

    void TriggerUpgrade()
    {
        // Verifica��o de seguran�a
        if (upgradePanel == null)
        {
            Debug.LogError("Upgrade Panel n�o atribu�do no inspector");
            return;
        }

        // Seleciona 3 poderes aleat�rios
        SelectRandomPowers();

        // Configura a UI para cada bot�o
        for (int i = 0; i < 3; i++)
        {
            if (currentOfferedPowers[i] != null)
            {
                if (powerIcons[i] != null) powerIcons[i].sprite = currentOfferedPowers[i].icon;
                if (powerNames[i] != null) powerNames[i].text = currentOfferedPowers[i].powerName;
                if (powerDescriptions[i] != null) powerDescriptions[i].text = currentOfferedPowers[i].description;

                // Verifica se o jogador j� tem esse poder
                string powerKey = currentOfferedPowers[i].GetType().Name;
                if (PowerManager.Instance != null && PowerManager.Instance.activePowers.ContainsKey(powerKey))
                {
                    if (powerLevels[i] != null)
                        powerLevels[i].text = "Nv. " + (PowerManager.Instance.activePowers[powerKey].level + 1);
                }
                else
                {
                    if (powerLevels[i] != null)
                        powerLevels[i].text = "Novo!";
                }

                if (powerButtons[i] != null)
                    powerButtons[i].interactable = true;
            }
            else
            {
                if (powerButtons[i] != null)
                    powerButtons[i].interactable = false;
            }
        }

        Time.timeScale = 0f;
        upgradePanel.SetActive(true);
        awaitingUpgrade = true;
    }

    void SelectRandomPowers()
    {
        // Verifica��o de seguran�a
        if (PowerManager.Instance == null)
        {
            Debug.LogError("PowerManager.Instance n�o encontrado!");
            return;
        }

        // Filtra poderes que ainda podem ser evolu�dos
        var availablePowers = allPowers.Where(p =>
            !PowerManager.Instance.activePowers.ContainsKey(p.GetType().Name) ||
            PowerManager.Instance.activePowers[p.GetType().Name].level < p.maxLevel
        ).ToList();

        // Se n�o houver poderes suficientes, usa todos
        if (availablePowers.Count < 3)
        {
            availablePowers = allPowers;
        }

        // Embaralha os poderes
        var shuffledPowers = availablePowers.OrderBy(x => Random.value).ToList();

        // Seleciona 3 poderes �nicos
        for (int i = 0; i < 3 && i < shuffledPowers.Count; i++)
        {
            currentOfferedPowers[i] = shuffledPowers[i];
        }
    }

    public void ApplyUpgrade(int buttonIndex)
    {
        // Verifica��o de seguran�a
        if (buttonIndex < 0 || buttonIndex >= currentOfferedPowers.Length)
        {
            Debug.LogError("�ndice de bot�o inv�lido: " + buttonIndex);
            return;
        }

        // Aplica o upgrade se houver poder selecionado
        if (currentOfferedPowers[buttonIndex] != null)
        {
            Power selectedPower = currentOfferedPowers[buttonIndex];

            if (PowerManager.Instance != null)
            {
                PowerManager.Instance.AddPower(selectedPower);
                Debug.Log("Upgrade aplicado - �ndice: " + buttonIndex + ", Poder: " + selectedPower.powerName);
            }
            else
            {
                Debug.LogError("PowerManager.Instance n�o encontrado!");
            }
        }
        else
        {
            Debug.LogWarning("Nenhum poder selecionado no �ndice " + buttonIndex);
        }

        // Reset do sistema de XP
        currentXP = 0;
        xpToNextLevel = Mathf.CeilToInt(xpToNextLevel * 1.5f);

        // Limpa os poderes oferecidos
        for (int i = 0; i < currentOfferedPowers.Length; i++)
        {
            currentOfferedPowers[i] = null;
        }

        UpdateUI();
        Time.timeScale = 1f;

        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Upgrade Panel n�o atribu�do no inspector");
        }

        awaitingUpgrade = false;
    }

    void UpdateUI()
    {
        if (xpText != null)
        {
            xpText.text = "XP: " + currentXP + " / " + xpToNextLevel;
        }
        else
        {
            Debug.LogWarning("xpText n�o atribu�do no inspector");
        }
    }
}