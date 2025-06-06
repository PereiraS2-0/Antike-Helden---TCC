using UnityEngine;


public class EnemyBehavior : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int damageToTower = 1;

    [Header("Targeting")]
    [SerializeField] private string towerTag = "Tower";
    [SerializeField] private float towerDetectionRadius = 0.5f; // Raio para detectar colisão com a torre
    private Transform towerTransform;

    [Header("Effects (Opcional)")]
    [SerializeField] private GameObject deathEffectPrefab;

    private int currentHealth;
    private bool isDead = false;
    private float originalSpeed;
    private float slowTimer;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        GameObject towerObject = GameObject.FindGameObjectWithTag(towerTag);
        if (towerObject != null)
        {
            towerTransform = towerObject.transform;
        }
        else
        {
            Debug.LogWarning("Torre com tag '" + towerTag + "' não encontrada.");
        }
        originalSpeed = moveSpeed;
    }

    void Update()
    {
        if (isDead) return;

        if (slowTimer > 0)
        {
            slowTimer -= Time.deltaTime;
            if (slowTimer <= 0)
            {
                moveSpeed = originalSpeed;
            }
        }

        MoveTowardsTarget();
        CheckTowerCollision();
    }

    void MoveTowardsTarget()
    {
        Vector3 targetPosition = towerTransform != null ? towerTransform.position : Vector3.zero;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    void CheckTowerCollision()
    {
        if (towerTransform == null || isDead) return;

        // Verifica colisão usando distância (mais confiável que OnCollision)
        if (Vector3.Distance(transform.position, towerTransform.position) <= towerDetectionRadius)
        {
            AttackTower();
        }
    }

    void AttackTower()
    {
        TowerHealth towerHealth = towerTransform.GetComponent<TowerHealth>();
        if (towerHealth != null)
        {
            towerHealth.TakeDamage(damageToTower);
        }

        // Efeito de morte opcional
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    public void ApplySlow(float slowPercent, float duration)
    {
        moveSpeed = originalSpeed * (1f - slowPercent);
        slowTimer = duration;
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
