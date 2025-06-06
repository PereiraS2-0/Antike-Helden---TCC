using UnityEngine;

public abstract class Power : MonoBehaviour
{
    public string powerName;
    public string description;
    public Sprite icon;
    public int level = 1;
    public int maxLevel = 5;
    public int baseDamage;
    public int damagePerLevel;
    public float baseCooldown;
    public float cooldownReductionPerLevel;
    public float currentCooldown;

    public abstract void Activate();
    public abstract void Upgrade();

    protected virtual void Update()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    public int GetDamage()
    {
        return baseDamage + (damagePerLevel * (level - 1));
    }

    public float GetCooldown()
    {
        return Mathf.Max(0.1f, baseCooldown - (cooldownReductionPerLevel * (level - 1)));
    }
}