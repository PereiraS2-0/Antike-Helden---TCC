using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int damageToTower = 1; // Dano que este inimigo causa à torre se alcançá-la
    // [SerializeField] private int xpDropped = 5; // Futuramente para dropar XP

    [Header("Targeting")]
    [SerializeField] private string towerTag = "Tower"; // Tag do objeto da Torre
    private Transform towerTransform; // Referência ao transform da torre para movimento

    [Header("Effects (Opcional)")]
    [SerializeField] private GameObject deathEffectPrefab; // Efeito visual ao morrer

    private int currentHealth;
    private bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        // Encontra a torre pelo centro (Vector3.zero) ou pela tag
        // Se sua torre não estiver em Vector3.zero, use FindGameObjectWithTag
        GameObject towerObject = GameObject.FindGameObjectWithTag(towerTag);
        if (towerObject != null)
        {
            towerTransform = towerObject.transform;
        }
        else
        {
            // Se a torre estiver sempre no centro e não tiver tag ou você não quiser usar tag para isso:
            // towerTransform = // Você pode criar um ponto fixo ou assumir Vector3.zero se for o caso.
            // Se for Vector3.zero, não precisa necessariamente de uma referência de transform.
            Debug.LogWarning("EnemyBehavior: Torre com tag '" + towerTag + "' não encontrada. O inimigo se moverá para Vector3.zero se towerTransform for nulo.");
        }
    }

    void Update()
    {
        if (isDead) return; // Não faz nada se já estiver morto

        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        Vector3 targetPosition;

        if (towerTransform != null)
        {
            targetPosition = towerTransform.position;
        }
        else
        {
            // Se a torre não foi encontrada (ou se você quer que vá para o centro por padrão)
            targetPosition = Vector3.zero;
        }

        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // Opcional: Rotacionar o inimigo para olhar na direção do movimento
        /* if (moveDirection != Vector3.zero)
        {
            // Subtrai 90 graus se o sprite do inimigo "aponta para cima" por padrão
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        }*/
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        // Debug.Log(gameObject.name + " tomou " + damageAmount + " de dano. Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        // else
        // {
        //     // Opcional: Efeito de hit (piscar, etc.)
        // }
    }

    private void Die()
    {
        isDead = true; // Marca como morto para evitar ações duplicadas
        Debug.Log(gameObject.name + " morreu!");

        // Opcional: Instanciar efeito de morte
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        // Futuramente: dropar XP Orb aqui
        // XPOrbManager.Instance.DropXP(transform.position, xpDropped);

        // Destruir o GameObject do inimigo
        // Se estiver usando Object Pooling, você o retornaria ao pool aqui.
        Destroy(gameObject);
    }

    // Colisão com a Torre (se o inimigo chegar lá)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return; // Não faz nada se já estiver morto

        if (collision.gameObject.CompareTag(towerTag))
        {
            // Debug.Log(gameObject.name + " atingiu a Torre.");
            TowerHealth towerHealth = collision.gameObject.GetComponent<TowerHealth>();
            if (towerHealth != null)
            {
                towerHealth.TakeDamage(damageToTower);
            }

            // Destrói o inimigo após colidir com a torre
            // Mesmo que não cause dano (ex: se a torre já estiver destruída mas o objeto ainda existe)
            // Ou poderia chamar Die() para ter o efeito de morte, etc.
            Destroy(gameObject);
        }
    }

    // Opcional: Para escalar stats com dificuldade (se for implementar EnemySpawner mais avançado)
    // public void InitializeStats(float difficultyMultiplier)
    // {
    //    maxHealth = Mathf.RoundToInt(maxHealth * difficultyMultiplier);
    //    currentHealth = maxHealth;
    //    damageToTower = Mathf.RoundToInt(damageToTower * difficultyMultiplier);
    //    // moveSpeed *= (1 + (difficultyMultiplier-1)/2); // Aumenta menos a velocidade
    // }
}
