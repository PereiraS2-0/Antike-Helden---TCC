using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] public int maxHealth = 10;
    public int currentHealth;

    [Header("Collision Settings")]
    [SerializeField] private float damageCooldown = 0.5f; // Previne múltiplos danos rápidos
    private float lastDamageTime;

    [Header("Effects (Opcional)")]
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private GameObject destructionEffectPrefab;

    // Eventos
    public static event System.Action<int, int> OnHealthChanged;
    public static event System.Action OnTowerDestroyed;

    private float shieldTimer;
    private float shieldAbsorption;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Update()
    {
        // Sistema de escudo
        if (shieldTimer > 0)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0)
            {
                shieldAbsorption = 0;
            }
        }

        // Verificação adicional para inimigos dentro da torre
        CheckForEnemiesInside();
    }

    void CheckForEnemiesInside()
    {
        // Detecta inimigos dentro do raio da torre
        Collider[] enemies = Physics.OverlapSphere(transform.position, 1.5f);
        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Enemy") && Time.time - lastDamageTime >= damageCooldown)
            {
                TakeDamage(1);
                Destroy(enemy.gameObject);
                lastDamageTime = Time.time;
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentHealth <= 0) return;

        // Aplica escudo se existir
        if (shieldAbsorption > 0)
        {
            float absorbed = Mathf.Min(damageAmount, shieldAbsorption);
            damageAmount -= (int)absorbed;
            shieldAbsorption -= absorbed;

            if (shieldAbsorption <= 0)
            {
                // Escudo quebrado
            }
        }

        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log("Torre tomou " + damageAmount + " de dano. Vida restante: " + currentHealth + "/" + maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("TORRE DESTRUÍDA! Fim de jogo.");
        OnTowerDestroyed?.Invoke();

        if (destructionEffectPrefab != null)
        {
            Instantiate(destructionEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    public void ActivateShield(float duration, float absorption)
    {
        shieldTimer = duration;
        shieldAbsorption = absorption;
    }

    // Desenha gizmo para visualizar o raio de detecção
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
}