using UnityEngine;

public class TowerController : MonoBehaviour
{
    [Header("Targeting Settings")]
    [SerializeField] private float attackRange = 10f; // Raio de detec��o e ataque
    [SerializeField] private LayerMask enemyLayer;    // Configure no Inspector para a layer dos inimigos
    [SerializeField] private float rotationSpeed = 0f; // Velocidade de rota��o da torre

    [Header("Attacking Settings")]
    [SerializeField] private GameObject projectilePrefab; // Arraste o Prefab do Proj�til aqui
    [SerializeField] private Transform firePoint;       // Ponto de onde o proj�til sai (um Empty GameObject filho da torre)
    [SerializeField] private float attackRate = 1f;     // Tiros por segundo
    public int projectileDamage = 1;       // Dano que cada proj�til da torre causa

    private Transform currentTarget;
    private float attackCooldown = 0f;

    void Update()
    {
        FindNearestEnemy();
        /*RotateTowardsTarget();*/

        // L�gica de ataque
        if (currentTarget != null) // S� tenta atirar se tiver um alvo
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
            // Se n�o h� alvo, reseta o cooldown para que possa atirar imediatamente quando um alvo aparecer
            // (ou pode deixar o cooldown continuar, dependendo da prefer�ncia de design)
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
            // Verifica se o inimigo tem o script EnemyBehavior e n�o est� morto (se for implementar isDead no futuro)
            // EnemyBehavior enemyBehavior = enemyCollider.GetComponent<EnemyBehavior>();
            // if (enemyBehavior != null && !enemyBehavior.IsDead()) // Supondo um m�todo IsDead() no EnemyBehavior

            float distanceToEnemy = Vector2.Distance(transform.position, enemyCollider.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemyCollider.transform;
            }
        }

        // Define o alvo se um inimigo v�lido foi encontrado
        if (nearestEnemy != null && shortestDistance <= attackRange)
        {
            currentTarget = nearestEnemy;
        }
        else
        {
            currentTarget = null; // Nenhum alvo v�lido encontrado
        }
    }

    void RotateTowardsTarget()
    {
        if (currentTarget == null)
        {
            // Opcional: Fazer a torre voltar para uma rota��o padr�o ou parar de girar
            // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
            return;
        }

        // Calcula a dire��o para o alvo
        Vector2 direction = (Vector2)currentTarget.position - (Vector2)transform.position;
        direction.Normalize();

        // Calcula o �ngulo e rotaciona
        // Subtrai 90 graus se o sprite da torre "aponta para cima" (ao longo do eixo Y local) por padr�o
       float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

        // Suaviza a rota��o
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogError("TowerController: Projectile Prefab ou Fire Point n�o configurados!");
            return;
        }

        // Instancia o proj�til
        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectileScript = projectileGO.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            // Passa o dano atualizado!
            projectileScript.Seek(currentTarget, projectileDamage);
        }
        else
        {
            Debug.LogError("TowerController: Prefab do proj�til n�o tem o script Projectile.cs!");
            Destroy(projectileGO); // se o prefab estiver errado, destr�i
        }
    }

    // Para visualizar o raio de ataque no Editor do Unity (muito �til!)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
