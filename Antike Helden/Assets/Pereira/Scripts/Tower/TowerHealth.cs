using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] public int maxHealth = 10;
    public int currentHealth;

    [Header("Effects (Opcional)")]
    [SerializeField] private GameObject hitEffectPrefab; // Efeito visual ao ser atingida
    [SerializeField] private GameObject destructionEffectPrefab; // Efeito visual ao ser destruída

    // Evento para notificar outros scripts quando a vida muda (útil para UI)
    public static event System.Action<int, int> OnHealthChanged; // currentHealth, maxHealth
    // Evento para notificar quando a torre é destruída
    public static event System.Action OnTowerDestroyed;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        // Informa a UI (ou outros sistemas) sobre a vida inicial
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentHealth <= 0) return; // Já destruída, não faz nada

        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0); // Garante que a vida não seja negativa

        Debug.Log("Torre tomou " + damageAmount + " de dano. Vida restante: " + currentHealth + "/" + maxHealth);

        // Invoca o evento de mudança de vida
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        // Instancia o efeito de hit se existir
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        if (currentHealth <= 0)
        {
            Die(); // <-- Isso precisa estar ativo!
        }
    }

    private void Die()
    {
        Debug.Log("TORRE DESTRUÍDA! Fim de jogo.");

        // Invoca o evento de destruição da torre
        OnTowerDestroyed?.Invoke();

        // Opcional: Instanciar efeito de destruição
        /*if (destructionEffectPrefab != null)
        {
            Instantiate(destructionEffectPrefab, transform.position, Quaternion.identity);
        }*/

        // Destrói o GameObject da torre
        Destroy(gameObject);

        // Aqui você pode adicionar lógica de Game Over, como:
        // Time.timeScale = 0f; // Pausa o jogo
        // GameManager.Instance.ShowGameOverScreen(); // Se tiver um GameManager
    }
}
