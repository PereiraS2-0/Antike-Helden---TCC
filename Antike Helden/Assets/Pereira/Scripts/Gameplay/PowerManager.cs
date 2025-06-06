using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    public static PowerManager Instance;

    public List<Power> allPowers = new List<Power>();
    public Dictionary<string, Power> activePowers = new Dictionary<string, Power>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Preenche a lista de todos os poderes disponíveis
        InitializePowerList();
    }

    private void InitializePowerList()
    {
        // Isso seria preenchido no Inspector com prefabs de poderes
        // allPowers.Add(...);
    }

    public void AddPower(Power newPower)
    {
        string powerKey = newPower.GetType().Name;

        if (activePowers.ContainsKey(powerKey))
        {
            // Evolui o poder existente
            activePowers[powerKey].Upgrade();
        }
        else
        {
            // Adiciona um novo poder
            Power instance = Instantiate(newPower, transform);
            activePowers.Add(powerKey, instance);
            instance.Activate();
        }
    }

    public void ActivatePowers()
    {
        foreach (Power power in activePowers.Values)
        {
            power.Activate();
        }
    }

    void Update()
    {
        ActivatePowers();
    }
}