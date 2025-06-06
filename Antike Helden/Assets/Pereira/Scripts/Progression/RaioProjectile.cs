using UnityEngine;

public class RaioProjectile : MonoBehaviour
{
    public float speed = 20f; // Velocidade do raio
    // public int damage = 5; // O dano ser� passado pelo m�todo Seek
    private int currentDamage; // Para armazenar o dano recebido
    public float lifeTime = 3f;

    private Transform target;
    private Vector3 directionToTarget; // Usado se o alvo for destru�do

    // Este m�todo ser� chamado pelo BracoDeZeus.cs (ou TowerController.cs)
    public void Seek(Transform _target, int _damage)
    {
        target = _target;
        currentDamage = _damage;

        if (target != null)
        {
            // Calcula a dire��o inicial para o alvo
            // Se o proj�til n�o for homing, esta dire��o ser� usada continuamente
            directionToTarget = (target.position - transform.position).normalized;
        }
        else
        {
            // Se n�o houver alvo, define uma dire��o padr�o (ex: para frente do ponto de disparo)
            // Ou voc� pode optar por n�o disparar/destruir o proj�til se n�o houver alvo
            directionToTarget = transform.right; // Assume que o proj�til est� orientado para a direita por padr�o
            Debug.LogWarning("RaioProjectile: Alvo � nulo no momento do Seek. Disparando para frente.");
        }

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (target != null)
        {
            // Op��o A: Proj�til Homing (segue o alvo)
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Opcional: Rotacionar o proj�til para olhar para o alvo
            // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            // Op��o B: Proj�til se move na dire��o calculada no Seek (se o alvo for destru�do ou n�o for homing)
            transform.position += directionToTarget * speed * Time.deltaTime;
            // Se o alvo foi destru�do, o proj�til continua em sua �ltima trajet�ria
            // Se o proj�til n�o era homing desde o in�cio, ele usar� essa dire��o.
        }

        // Se o alvo se tornou nulo ap�s ser definido (ex: inimigo morreu por outra causa)
        // e voc� n�o quer que o proj�til continue, voc� pode adicionar uma l�gica aqui para destru�-lo
        // ou mudar seu comportamento.
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) // Certifique-se que seus inimigos t�m a tag "Enemy"
        {
            Basic enemy = collision.GetComponent<Basic>(); // Supondo que o script do inimigo se chama EnemyHealth
            if (enemy != null)
            {
                enemy.TakeDamage(currentDamage); // Chame o m�todo de dano do inimigo
            }
            Destroy(gameObject); // Destr�i o proj�til ao atingir um inimigo
        }
        // Opcional: Adicionar colis�o com outros objetos (ex: cen�rio) se o proj�til deve ser destru�do
        // else if (collision.CompareTag("Obstacle"))
        // {
        //     Destroy(gameObject);
        // }
    }
}