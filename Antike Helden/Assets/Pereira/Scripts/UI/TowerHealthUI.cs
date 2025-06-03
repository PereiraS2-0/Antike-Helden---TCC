using UnityEngine;
using UnityEngine.UI; // Necess�rio para interagir com componentes de UI como Text
using TMPro; // Use esta linha se voc� estiver usando TextMeshPro, sen�o pode remover

public class TowerHealthUI : MonoBehaviour
{
    // Arraste o componente Text (ou TextMeshProUGUI) do seu Canvas para este campo no Inspector
 
    [SerializeField] private TextMeshProUGUI healthTextMeshPro; // Para TextMeshPro
    public TowerHealth towerHealth;


    // Este m�todo � chamado quando o script � carregado ou um valor � alterado no Inspector.
    // Usamos para nos registrar (subscribe) ao evento OnHealthChanged da torre.
    void OnEnable()
    {
        TowerHealth.OnHealthChanged += UpdateHealthText;
        // Adicionalmente, voc� pode querer se registrar ao evento de destrui��o para, por exemplo, esconder a UI ou mostrar "0"
        TowerHealth.OnTowerDestroyed += HandleTowerDestroyed;
    }

    // Este m�todo � chamado quando o script � desabilitado ou destru�do.
    // � MUITO IMPORTANTE se desregistrar (unsubscribe) dos eventos para evitar erros.
    void OnDisable()
    {
        TowerHealth.OnHealthChanged -= UpdateHealthText;
        TowerHealth.OnTowerDestroyed -= HandleTowerDestroyed;
    }

    // Este m�todo ser� chamado pelo evento OnHealthChanged da TowerHealth
    private void UpdateHealthText(int currentHealth, int maxHealth)
    {
        
        if (healthTextMeshPro != null)
        {
            healthTextMeshPro.text = "Vida: " + currentHealth.ToString(); // Ou "Vida: " + currentHealth + "/" + maxHealth
        }
        

        // Se currentHealth for 0 e voc� n�o tiver um HandleTowerDestroyed separado,
        // poderia atualizar o texto para "Vida: 0" aqui tamb�m.
    }

    private void HandleTowerDestroyed()
    {
        // Quando a torre for destru�da, voc� pode querer que o texto mostre "0"
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
        
        Debug.Log("TowerHealthUI: Torre destru�da, atualizando texto para 0.");
    }
    public void AtualizarTextoVida()
    {
        if (healthTextMeshPro != null && towerHealth != null)
        {
            healthTextMeshPro.text = "Vida: " + towerHealth.currentHealth;
        }
        else
        {
            Debug.LogWarning("TowerHealthUI: Refer�ncia faltando ao atualizar vida.");
        }
    }

    // Opcional: Se voc� quiser que o texto seja atualizado imediatamente ao iniciar o jogo,
    // e n�o apenas na primeira mudan�a de vida, voc� precisaria de uma refer�ncia direta
    // � torre ou fazer o TowerHealth invocar o OnHealthChanged no Start com os valores iniciais.
    // (O TowerHealth.cs j� faz isso no Start, ent�o este script deve pegar o valor inicial).
}