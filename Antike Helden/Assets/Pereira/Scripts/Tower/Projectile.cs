using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifeTime = 3f;

    private int damage;
    private Transform target;
    private Vector3 targetDirection;

    public void Seek(Transform _target, int _damage)
    {
        target = _target;
        damage = _damage;

        if (target != null)
        {
            targetDirection = (target.position - transform.position).normalized;
            RotateTowardsDirection(targetDirection);
        }
        else
        {
            targetDirection = transform.up;
        }

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(targetDirection * speed * Time.deltaTime, Space.World);
    }

    void RotateTowardsDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Basic enemy = collision.GetComponent<Basic>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}