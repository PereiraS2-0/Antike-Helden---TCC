using UnityEngine;
using TMPro;

public class TowerUpgradePanel : MonoBehaviour
{
    [Header("Referências")]
    public TowerHealth towerHealth;
    public TowerController towerController;
    public CoinManager coinManager;

    [Header("UI Vida")]
    public TextMeshProUGUI vidaTexto;
    public TextMeshProUGUI vidaCustoTexto;
    public int vidaCusto = 2;
    public TowerHealthUI towerHealthUI;

    [Header("UI Dano")]
    public TextMeshProUGUI danoTexto;
    public TextMeshProUGUI danoCustoTexto;
    public int danoCusto = 2;


    void Start()
    {
        AtualizarUI();
    }

    public void AumentarVida()
    {
        if (coinManager.totalCoins >= vidaCusto)
        {
            coinManager.totalCoins -= vidaCusto;
            towerHealth.maxHealth += 10;
            towerHealth.currentHealth += 10;

            vidaCusto += 2; // custo cresce só dessa opção
            AtualizarUI();
            coinManager.UpdateUI();
            towerHealthUI.AtualizarTextoVida();
        }
    }

    public void AumentarDano()
    {
        if (coinManager.totalCoins >= danoCusto)
        {
            coinManager.totalCoins -= danoCusto;
            towerController.projectileDamage += 2;

            danoCusto += 2;
            AtualizarUI();
            coinManager.UpdateUI();
        }
    }

    void AtualizarUI()
    {
        vidaTexto.text = "Vida da Torre: " + towerHealth.maxHealth;
        vidaCustoTexto.text = "Custo: " + vidaCusto;

        danoTexto.text = "Dano do Projétil: " + towerController.projectileDamage;
        danoCustoTexto.text = "Custo: " + danoCusto;
    }
}
