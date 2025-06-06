using UnityEngine;
public class SabedoriaDeAtena : Power
{
    public float attackSpeedBonus = 0.1f;
    public float damageBonus = 0.1f;

    private TowerController towerController;

    void Start()
    {
        powerName = "Sabedoria de Atena";
        description = "Aumenta permanentemente os atributos da torre";
        towerController = FindFirstObjectByType<TowerController>();

        // Aplica os b�nus imediatamente
        ApplyBonuses();
    }

    public override void Activate()
    {
        // Poder passivo - n�o precisa de ativa��o manual
    }

    public override void Upgrade()
    {
        level++;
        ApplyBonuses();
        Debug.Log($"{powerName} evolu�do para n�vel {level}!");
    }

    private void ApplyBonuses()
    {
        // Corrigido:
        towerController.attackRate *= (1 + (attackSpeedBonus * level));
        towerController.projectileDamage += Mathf.RoundToInt(
            towerController.projectileDamage * (damageBonus * level)
        );
    }
}