// using Unity.VisualScripting; // Remova se não estiver usando Visual Scripting explicitamente para algo aqui
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [Header("Targeting Settings")]
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Normal Attacking Settings")]
    public GameObject projectilePrefab;   // Já era public
    public Transform firePoint;           // Mude para public
    public Transform currentTarget;       // Mude de private para public
    public float attackRate = 1f; // Tiros normais por segundo
    public int projectileDamage = 1;
    [Header("Normal Attacking Settings")]

    private float normalAttackCooldown = 0f;

    [Header("Special Attack Settings (Braco de Zeus)")]
    public Transform bracoDeZeusObject; // Arraste aqui o GameObject que tem o script BracoDeZeus.cs
                                        // Este campo será preenchido/verificado quando o "prefab do Braço de Zeus aparecer"
    [SerializeField] private float specialAttackRate = 0.2f; // Ex: 1 tiro especial a cada 5 segundos (1/5 = 0.2)
    private float specialAttackCooldown = 0f;
    //private BracoDeZeus bracoDeZeusScript; // Cache do script

    [Header("Power Integration")]
    public PowerManager powerManager;

    void Start()
    {
        // Tenta obter o script do Braço de Zeus se o objeto já estiver atribuído e ativo
        // O cooldown especial pode começar já carregado ou não, dependendo da sua preferência
        // specialAttackCooldown = 1f / specialAttackRate; // Começa em cooldown
    }

    // Modifique o Update():
    void Update()
    {
        FindNearestEnemy();

        if (normalAttackCooldown > 0f)
        {
            normalAttackCooldown -= Time.deltaTime;
        }

        if (currentTarget != null && normalAttackCooldown <= 0f)
        {
            ShootNormal();
            normalAttackCooldown = 1f / attackRate;
        }

        // Ativa todos os poderes
        powerManager.ActivatePowers();
    }

    void FindNearestEnemy()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;
        foreach (Collider2D enemyCollider in enemiesInRange)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemyCollider.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemyCollider.transform;
            }
        }
        currentTarget = (nearestEnemy != null && shortestDistance <= attackRange) ? nearestEnemy : null;
    }

    void ShootNormal()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogError("TowerController: Projectile Prefab ou Fire Point não configurados!");
            return;
        }

        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectileScript = projectileGO.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            projectileScript.Seek(currentTarget, projectileDamage);
        }
        else
        {
            Debug.LogError("TowerController: Prefab do projétil normal não tem o script Projectile.cs!");
            Destroy(projectileGO);
        }
    }

    void ShootSpecial()
    {
        // bracoDeZeusScript já foi verificado como não nulo antes de chamar este método
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}