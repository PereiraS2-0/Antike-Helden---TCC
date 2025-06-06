using UnityEngine;

public class BracoDeZeus : Power
{
    public GameObject lightningPrefab;
    public float chainRadius = 3f;
    public int maxChainTargets = 3;

    private TowerController towerController;

    void Start()
    {
        // Adicione:
        powerName = "Braço de Zeus";
        description = "Dispara um raio que acorrenta entre inimigos";
        towerController = FindFirstObjectByType<TowerController>();
    }

    public override void Activate()
    {
        if (currentCooldown > 0 || towerController.currentTarget == null) return;

        // Corrigido:
        GameObject primaryLightning = Instantiate(
            lightningPrefab,
            towerController.firePoint.position,
            Quaternion.identity
        );

        LightningProjectile lightning = primaryLightning.GetComponent<LightningProjectile>();
        lightning.Initialize(
            towerController.currentTarget,
            GetDamage(),
            chainRadius,
            maxChainTargets
        );

        currentCooldown = GetCooldown();
    }


    public override void Upgrade()
    {
        level++;
        maxChainTargets++;
        damagePerLevel += 5;

        Debug.Log($"{powerName} evoluído para nível {level}!");
    }
}