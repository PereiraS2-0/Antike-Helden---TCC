using UnityEngine;

public class AguaDoBoto : Power
{
    public GameObject bubblePrefab;
    public float slowEffect = 0.3f;
    public float slowDuration = 3f;

    private TowerController towerController;

    void Start()
    {
        powerName = "Água do Boto";
        description = "Lança bolhas que causam dano e lentidão";
        towerController = FindFirstObjectByType<TowerController>();
    }

    public override void Activate()
    {
        if (currentCooldown > 0 || towerController.currentTarget == null) return;

        GameObject bubble = Instantiate(
            bubblePrefab,
            towerController.firePoint.position,
            Quaternion.identity
        );

        BubbleProjectile bubbleScript = bubble.GetComponent<BubbleProjectile>();
        bubbleScript.Initialize(
            towerController.currentTarget,
            GetDamage(),
            slowEffect,
            slowDuration
        );

        currentCooldown = GetCooldown();
    }

    public override void Upgrade()
    {
        level++;
        slowEffect += 0.05f;
        slowDuration += 0.5f;

        Debug.Log($"{powerName} evoluído para nível {level}!");
    }
}