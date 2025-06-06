using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    private bool collected = false; // Para evitar coleta múltipla

    private void OnMouseEnter()
    {
        if (!collected)
        {
            collected = true;
            Debug.Log("Coletando moeda: " + gameObject.name);
            CoinManager.Instance.AddCoins(coinValue);
            gameObject.SetActive(false); // Desativa ao invés de destruir
        }
    }

    // Alternativa para permitir coleta por clique também
    private void OnMouseDown()
    {
        if (!collected)
        {
            collected = true;
            Debug.Log("Coletando moeda (clique): " + gameObject.name);
            CoinManager.Instance.AddCoins(coinValue);
            gameObject.SetActive(false);
        }
    }
}