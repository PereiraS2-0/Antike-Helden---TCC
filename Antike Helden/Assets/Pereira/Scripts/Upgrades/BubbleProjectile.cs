using UnityEngine;

public class BubbleProjectile : MonoBehaviour
{
    private Transform target;
    private int damage;
    private float slowEffect;
    private float slowDuration;

    public void Initialize(Transform _target, int _damage, float _slowEffect, float _slowDuration)
    {
        target = _target;
        damage = _damage;
        slowEffect = _slowEffect;
        slowDuration = _slowDuration;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = 10f * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        EnemyBehavior enemy = target.GetComponent<EnemyBehavior>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            enemy.ApplySlow(slowEffect, slowDuration);
        }
        Destroy(gameObject);
    }
}