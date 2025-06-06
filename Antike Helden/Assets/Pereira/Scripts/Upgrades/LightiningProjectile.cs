using UnityEngine;

public class LightningProjectile : MonoBehaviour
{
    private Transform target;
    private int damage;
    private float chainRadius;
    private int maxChainTargets;
    private int chainsRemaining;

    public void Initialize(Transform _target, int _damage, float _chainRadius, int _maxChainTargets)
    {
        target = _target;
        damage = _damage;
        chainRadius = _chainRadius;
        maxChainTargets = _maxChainTargets;
        chainsRemaining = _maxChainTargets;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = 20f * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        // Aplica dano ao alvo atual
        EnemyBehavior enemy = target.GetComponent<EnemyBehavior>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Encontra próximo alvo em cadeia
        chainsRemaining--;
        if (chainsRemaining > 0)
        {
            FindNextTarget();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void FindNextTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, chainRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy") && collider.transform != target)
            {
                target = collider.transform;
                return;
            }
        }
        Destroy(gameObject);
    }
}