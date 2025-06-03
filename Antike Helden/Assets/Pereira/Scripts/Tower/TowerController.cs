using UnityEngine;

public class TowerController : MonoBehaviour
{
    [Header("Targeting Settings")]
    [SerializeField] private float attackRange = 10f; // Raio de detecção e ataque
    [SerializeField] private LayerMask enemyLayer;    // Configure no Inspector para a layer dos inimigos
    [SerializeField] private float rotationSpeed = 0f; // Velocidade de rotação da torre

    [Header("Attacking Settings")]
    [SerializeField] private GameObject projectilePrefab; // Arraste o Prefab do Projétil aqui
    [SerializeField] private Transform firePoint;       // Ponto de onde o projétil sai (um Empty GameObject filho da torre)
    [SerializeField] private float attackRate = 1f;     // Tiros por segundo
    public int projectileDamage = 1;       // Dano que cada projétil da torre causa

    private Transform currentTarget;
    private float attackCooldown = 0f;

    void Update()
    {
        FindNearestEnemy();
        /*RotateTowardsTarget();*/

        // Lógica de ataque
        if (currentTarget != null) // Só tenta atirar se tiver um alvo
        {
            if (attackCooldown <= 0f)
            {
                Shoot();
                attackCooldown = 1f / attackRate; // Reseta o cooldown
            }
            else
            {
                attackCooldown -= Time.deltaTime;
            }
        }
        else
        {
            // Se não há alvo, reseta o cooldown para que possa atirar imediatamente quando um alvo aparecer
            // (ou pode deixar o cooldown continuar, dependendo da preferência de design)
            attackCooldown = 0f;
        }
    }

    void FindNearestEnemy()
    {
        // Encontra todos os colliders na layer de inimigos dentro do raio de ataque
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (Collider2D enemyCollider in enemiesInRange)
        {
            // Verifica se o inimigo tem o script EnemyBehavior e não está morto (se for implementar isDead no futuro)
            // EnemyBehavior enemyBehavior = enemyCollider.GetComponent<EnemyBehavior>();
            // if (enemyBehavior != null && !enemyBehavior.IsDead()) // Supondo um método IsDead() no EnemyBehavior

            float distanceToEnemy = Vector2.Distance(transform.position, enemyCollider.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemyCollider.transform;
            }
        }

        // Define o alvo se um inimigo válido foi encontrado
        if (nearestEnemy != null && shortestDistance <= attackRange)
        {
            currentTarget = nearestEnemy;
        }
        else
        {
            currentTarget = null; // Nenhum alvo válido encontrado
        }
    }

    void RotateTowardsTarget()
    {
        if (currentTarget == null)
        {
            // Opcional: Fazer a torre voltar para uma rotação padrão ou parar de girar
            // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
            return;
        }

        // Calcula a direção para o alvo
        Vector2 direction = (Vector2)currentTarget.position - (Vector2)transform.position;
        direction.Normalize();

        // Calcula o ângulo e rotaciona
        // Subtrai 90 graus se o sprite da torre "aponta para cima" (ao longo do eixo Y local) por padrão
       float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

        // Suaviza a rotação
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogError("TowerController: Projectile Prefab ou Fire Point não configurados!");
            return;
        }

        // Instancia o projétil
        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectileScript = projectileGO.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            // Passa o dano atualizado!
            projectileScript.Seek(currentTarget, projectileDamage);
        }
        else
        {
            Debug.LogError("TowerController: Prefab do projétil não tem o script Projectile.cs!");
            Destroy(projectileGO); // se o prefab estiver errado, destrói
        }
    }

    // Para visualizar o raio de ataque no Editor do Unity (muito útil!)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
