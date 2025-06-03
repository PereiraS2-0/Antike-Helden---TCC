using UnityEngine;

public class Basic : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 2f;

    [SerializeField]
    private int damageToTower = 1;

    [SerializeField]
    private int maxHealth = 5;
    private int currentHealth;

    public GameObject coinPrefab;
    public GameObject xpPrefab;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        Vector3 targetPosition = Vector3.zero;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log(gameObject.name + " levou " + amount + " de dano. Vida atual: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tower"))
        {
            TowerHealth towerHealth = collision.gameObject.GetComponent<TowerHealth>();

            if (towerHealth != null)
            {
                towerHealth.TakeDamage(damageToTower);
            }
            else
            {
                Debug.LogError("TowerHealth não encontrado!");
            }

            Destroy(gameObject); // Morre ao bater na torre
        }
    }

    void Die()
    {
        Instantiate(coinPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        if (xpPrefab != null)
        {
            Instantiate(xpPrefab, transform.position, Quaternion.identity);
        }
    }
}
