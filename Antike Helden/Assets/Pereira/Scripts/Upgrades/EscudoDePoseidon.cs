using UnityEngine;

public class EscudoDePoseidon : Power
{
    public GameObject shieldPrefab;
    public float shieldDuration = 5f;
    public float damageAbsorption = 50f;

    private GameObject currentShield;
    private TowerHealth towerHealth;

    void Start()
    {
        powerName = "Escudo de Poseidon";
        description = "Protege a torre contra danos";
        towerHealth = FindFirstObjectByType<TowerHealth>();
    }

    public override void Activate()
    {
        if (currentCooldown > 0) return;

        if (currentShield == null)
        {
            currentShield = Instantiate(shieldPrefab, towerHealth.transform);
        }

        // Método corrigido:
        towerHealth.ActivateShield(shieldDuration, damageAbsorption * level);

        currentCooldown = GetCooldown();
    }

    public override void Upgrade()
    {
        level++;
        shieldDuration += 1f;
        damageAbsorption += 25f;

        Debug.Log($"{powerName} evoluído para nível {level}!");
    }

}