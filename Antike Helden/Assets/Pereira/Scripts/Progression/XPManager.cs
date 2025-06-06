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

        // Inicialização segura do array
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
        // Verificação de segurança
        if (upgradePanel == null)
        {
            Debug.LogError("Upgrade Panel não atribuído no inspector");
            return;
        }

        // Seleciona 3 poderes aleatórios
        SelectRandomPowers();

        // Configura a UI para cada botão
        for (int i = 0; i < 3; i++)
        {
            if (currentOfferedPowers[i] != null)
            {
                if (powerIcons[i] != null) powerIcons[i].sprite = currentOfferedPowers[i].icon;
                if (powerNames[i] != null) powerNames[i].text = currentOfferedPowers[i].powerName;
                if (powerDescriptions[i] != null) powerDescriptions[i].text = currentOfferedPowers[i].description;

                // Verifica se o jogador já tem esse poder
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
        // Verificação de segurança
        if (PowerManager.Instance == null)
        {
            Debug.LogError("PowerManager.Instance não encontrado!");
            return;
        }

        // Filtra poderes que ainda podem ser evoluídos
        var availablePowers = allPowers.Where(p =>
            !PowerManager.Instance.activePowers.ContainsKey(p.GetType().Name) ||
            PowerManager.Instance.activePowers[p.GetType().Name].level < p.maxLevel
        ).ToList();

        // Se não houver poderes suficientes, usa todos
        if (availablePowers.Count < 3)
        {
            availablePowers = allPowers;
        }

        // Embaralha os poderes
        var shuffledPowers = availablePowers.OrderBy(x => Random.value).ToList();

        // Seleciona 3 poderes únicos
        for (int i = 0; i < 3 && i < shuffledPowers.Count; i++)
        {
            currentOfferedPowers[i] = shuffledPowers[i];
        }
    }

    public void ApplyUpgrade(int buttonIndex)
    {
        // Verificação de segurança
        if (buttonIndex < 0 || buttonIndex >= currentOfferedPowers.Length)
        {
            Debug.LogError("Índice de botão inválido: " + buttonIndex);
            return;
        }

        // Aplica o upgrade se houver poder selecionado
        if (currentOfferedPowers[buttonIndex] != null)
        {
            Power selectedPower = currentOfferedPowers[buttonIndex];

            if (PowerManager.Instance != null)
            {
                PowerManager.Instance.AddPower(selectedPower);
                Debug.Log("Upgrade aplicado - Índice: " + buttonIndex + ", Poder: " + selectedPower.powerName);
            }
            else
            {
                Debug.LogError("PowerManager.Instance não encontrado!");
            }
        }
        else
        {
            Debug.LogWarning("Nenhum poder selecionado no índice " + buttonIndex);
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
            Debug.LogError("Upgrade Panel não atribuído no inspector");
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
            Debug.LogWarning("xpText não atribuído no inspector");
        }
    }
}