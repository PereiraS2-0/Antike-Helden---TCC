using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Variável para definir a Tag do objeto Torre.
    // Você pode configurar isso no Inspector do Unity se quiser que seja flexível,
    // ou deixar fixo no código como "Tower".
    public string towerTag = "Tower";

    // Método chamado quando este Collider/Rigidbody entra em contato com outro Collider marcado como Trigger.
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto com o qual colidimos tem a tag definida (por exemplo, "Tower")
        if (other.gameObject.CompareTag(towerTag))
        {
            Debug.Log(gameObject.name + " colidiu com a torre: " + other.gameObject.name + ". Destruindo inimigo.");

            // Destrói o GameObject ao qual este script está anexado (o inimigo)
            Destroy(gameObject);

            // Opcional: Se a torre tiver um script para receber dano, você pode chamá-lo aqui antes de destruir o inimigo.
            // Exemplo:
            TowerHealth towerHealth = other.gameObject.GetComponent<TowerHealth>();
            if (towerHealth != null)
             {
                towerHealth.TakeDamage(1); // Supondo que o inimigo causa 1 de dano
             }
        }
    }

    // ALTERNATIVA: Se você quiser usar colisão física em vez de trigger
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(towerTag))
        {
            Debug.Log(gameObject.name + " colidiu fisicamente com a torre: " + collision.gameObject.name + ". Destruindo inimigo.");
            Destroy(gameObject);
        }
    }
    
}