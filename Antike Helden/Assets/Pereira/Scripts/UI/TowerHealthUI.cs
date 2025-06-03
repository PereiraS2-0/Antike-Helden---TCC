using UnityEngine;
using UnityEngine.UI; // Necessário para interagir com componentes de UI como Text
using TMPro; // Use esta linha se você estiver usando TextMeshPro, senão pode remover

public class TowerHealthUI : MonoBehaviour
{
    // Arraste o componente Text (ou TextMeshProUGUI) do seu Canvas para este campo no Inspector
 
    [SerializeField] private TextMeshProUGUI healthTextMeshPro; // Para TextMeshPro
    public TowerHealth towerHealth;


    // Este método é chamado quando o script é carregado ou um valor é alterado no Inspector.
    // Usamos para nos registrar (subscribe) ao evento OnHealthChanged da torre.
    void OnEnable()
    {
        TowerHealth.OnHealthChanged += UpdateHealthText;
        // Adicionalmente, você pode querer se registrar ao evento de destruição para, por exemplo, esconder a UI ou mostrar "0"
        TowerHealth.OnTowerDestroyed += HandleTowerDestroyed;
    }

    // Este método é chamado quando o script é desabilitado ou destruído.
    // É MUITO IMPORTANTE se desregistrar (unsubscribe) dos eventos para evitar erros.
    void OnDisable()
    {
        TowerHealth.OnHealthChanged -= UpdateHealthText;
        TowerHealth.OnTowerDestroyed -= HandleTowerDestroyed;
    }

    // Este método será chamado pelo evento OnHealthChanged da TowerHealth
    private void UpdateHealthText(int currentHealth, int maxHealth)
    {
        
        if (healthTextMeshPro != null)
        {
            healthTextMeshPro.text = "Vida: " + currentHealth.ToString(); // Ou "Vida: " + currentHealth + "/" + maxHealth
        }
        

        // Se currentHealth for 0 e você não tiver um HandleTowerDestroyed separado,
        // poderia atualizar o texto para "Vida: 0" aqui também.
    }

    private void HandleTowerDestroyed()
    {
        // Quando a torre for destruída, você pode querer que o texto mostre "0"
        // ou talvez esconder o texto completamente.
        if (healthTextMeshPro != null)
        {
            healthTextMeshPro.text = "Vida: 0";
        }
        // OU, se estiver usando TextMeshPro:
        
        if (healthTextMeshPro != null)
        {
            healthTextMeshPro.text = "Vida: 0";
        }
        
        Debug.Log("TowerHealthUI: Torre destruída, atualizando texto para 0.");
    }
    public void AtualizarTextoVida()
    {
        if (healthTextMeshPro != null && towerHealth != null)
        {
            healthTextMeshPro.text = "Vida: " + towerHealth.currentHealth;
        }
        else
        {
            Debug.LogWarning("TowerHealthUI: Referência faltando ao atualizar vida.");
        }
    }

    // Opcional: Se você quiser que o texto seja atualizado imediatamente ao iniciar o jogo,
    // e não apenas na primeira mudança de vida, você precisaria de uma referência direta
    // à torre ou fazer o TowerHealth invocar o OnHealthChanged no Start com os valores iniciais.
    // (O TowerHealth.cs já faz isso no Start, então este script deve pegar o valor inicial).
}