using System;
using System.Collections.Generic;
using UnityEngine;


public enum ResourceType
{
    None,
    Coal,
    Iron,
    Gold,
    Diamond,
    Quartz
}

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    // Data
    private Dictionary<ResourceType, int> _resources = new Dictionary<ResourceType, int>();
    public int Credits { get; private set; }
    public int MaxPower { get; private set; }
    public int CurrentLoad { get; private set; }
    public bool IsInBrownout => CurrentLoad > MaxPower;

    // Events
    public event Action<ResourceType, int> OnResourceChanged;
    public event Action<int> OnCreditsChanged;
    public event Action<int, int, bool> OnPowerStateChanged; // MaxPower, CurrentLoad, IsBrownout

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;        
        DontDestroyOnLoad(gameObject); 

        // Testing
        AddCredits(500);
        UpdatePowerCapacity(100);
        
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            _resources[type] = 0;
        }
    }
    
    public void AddResource(ResourceType type, int amount)
    {
        if (type == ResourceType.None) return;
        _resources[type] += amount;
        OnResourceChanged?.Invoke(type, _resources[type]);
    }

    public bool SpendResource(ResourceType type, int amount)
    {
        if (_resources[type] >= amount)
        {
            _resources[type] -= amount;
            OnResourceChanged?.Invoke(type, _resources[type]);
            return true;
        }
        return false;
    }

    public void AddCredits(int amount)
    {
        Credits += amount;
        OnCreditsChanged?.Invoke(Credits);
    }
    
    public void UpdatePowerCapacity(int capacityDelta)
    {
        MaxPower += capacityDelta;
        CheckPowerState();
    }

    public void UpdatePowerLoad(int loadDelta)
    {
        CurrentLoad += loadDelta;
        CheckPowerState();
    }

    private void CheckPowerState()
    {        
        OnPowerStateChanged?.Invoke(MaxPower, CurrentLoad, IsInBrownout);
    }
}