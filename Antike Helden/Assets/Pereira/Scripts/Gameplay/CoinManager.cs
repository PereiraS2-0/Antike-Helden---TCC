using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public int totalCoins = 0;
    public TextMeshProUGUI coinText; // ou TextMeshProUGUI se preferir

    void Awake()
    {
        Debug.Log("CoinManager Awake chamado");
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("CoinManager Instance definido");
        }
        else
        {
            Debug.LogWarning("CoinManager duplicado - destruindo");
            Destroy(gameObject);
        }
    }

    public void AddCoins(int amount)
    {
        totalCoins += amount;
        Debug.Log($"Moedas adicionadas: {amount}. Total: {totalCoins}");
        UpdateUI();
    }

    public void UpdateUI()
    {
        coinText.text = "Moedas: " + totalCoins;
    }
}
