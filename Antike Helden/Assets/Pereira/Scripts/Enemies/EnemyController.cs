using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Vari�vel para definir a Tag do objeto Torre.
    // Voc� pode configurar isso no Inspector do Unity se quiser que seja flex�vel,
    // ou deixar fixo no c�digo como "Tower".
    public string towerTag = "Tower";

    // M�todo chamado quando este Collider/Rigidbody entra em contato com outro Collider marcado como Trigger.
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto com o qual colidimos tem a tag definida (por exemplo, "Tower")
        if (other.gameObject.CompareTag(towerTag))
        {
            Debug.Log(gameObject.name + " colidiu com a torre: " + other.gameObject.name + ". Destruindo inimigo.");

            // Destr�i o GameObject ao qual este script est� anexado (o inimigo)
            Destroy(gameObject);

            // Opcional: Se a torre tiver um script para receber dano, voc� pode cham�-lo aqui antes de destruir o inimigo.
            // Exemplo:
            TowerHealth towerHealth = other.gameObject.GetComponent<TowerHealth>();
            if (towerHealth != null)
             {
                towerHealth.TakeDamage(1); // Supondo que o inimigo causa 1 de dano
             }
        }
    }

    // ALTERNATIVA: Se voc� quiser usar colis�o f�sica em vez de trigger
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(towerTag))
        {
            Debug.Log(gameObject.name + " colidiu fisicamente com a torre: " + collision.gameObject.name + ". Destruindo inimigo.");
            Destroy(gameObject);
        }
    }
    
}