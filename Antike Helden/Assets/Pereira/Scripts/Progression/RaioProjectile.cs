using UnityEngine;

public class RaioProjectile : MonoBehaviour
{
    public float speed = 20f; // Velocidade do raio
    // public int damage = 5; // O dano será passado pelo método Seek
    private int currentDamage; // Para armazenar o dano recebido
    public float lifeTime = 3f;

    private Transform target;
    private Vector3 directionToTarget; // Usado se o alvo for destruído

    // Este método será chamado pelo BracoDeZeus.cs (ou TowerController.cs)
    public void Seek(Transform _target, int _damage)
    {
        target = _target;
        currentDamage = _damage;

        if (target != null)
        {
            // Calcula a direção inicial para o alvo
            // Se o projétil não for homing, esta direção será usada continuamente
            directionToTarget = (target.position - transform.position).normalized;
        }
        else
        {
            // Se não houver alvo, define uma direção padrão (ex: para frente do ponto de disparo)
            // Ou você pode optar por não disparar/destruir o projétil se não houver alvo
            directionToTarget = transform.right; // Assume que o projétil está orientado para a direita por padrão
            Debug.LogWarning("RaioProjectile: Alvo é nulo no momento do Seek. Disparando para frente.");
        }

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (target != null)
        {
            // Opção A: Projétil Homing (segue o alvo)
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Opcional: Rotacionar o projétil para olhar para o alvo
            // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            // Opção B: Projétil se move na direção calculada no Seek (se o alvo for destruído ou não for homing)
            transform.position += directionToTarget * speed * Time.deltaTime;
            // Se o alvo foi destruído, o projétil continua em sua última trajetória
            // Se o projétil não era homing desde o início, ele usará essa direção.
        }

        // Se o alvo se tornou nulo após ser definido (ex: inimigo morreu por outra causa)
        // e você não quer que o projétil continue, você pode adicionar uma lógica aqui para destruí-lo
        // ou mudar seu comportamento.
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) // Certifique-se que seus inimigos têm a tag "Enemy"
        {
            Basic enemy = collision.GetComponent<Basic>(); // Supondo que o script do inimigo se chama EnemyHealth
            if (enemy != null)
            {
                enemy.TakeDamage(currentDamage); // Chame o método de dano do inimigo
            }
            Destroy(gameObject); // Destrói o projétil ao atingir um inimigo
        }
        // Opcional: Adicionar colisão com outros objetos (ex: cenário) se o projétil deve ser destruído
        // else if (collision.CompareTag("Obstacle"))
        // {
        //     Destroy(gameObject);
        // }
    }
}