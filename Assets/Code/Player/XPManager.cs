using System;
using UnityEngine;
using UnityEngine.Events;

public class XPManager : MonoBehaviour
{
    [SerializeField]
    FillbarUI xpBar;
    [SerializeField]
    AnimationCurve _XPReqScaling;
    [SerializeField]
    float pickupRange = 5;
    [SerializeField]
    LayerMask pickupLayer;
    [SerializeField]
    UnityEvent<int, int> _onXPGained,
        _onLevelGained;

    public event Action<int, int> OnXPGainedEvent,
        OnLevelsGainedEvent;

    [SerializeField]
    int currentXP,
        currentLevel,
        currentXPReq;

    public int CurrentXP
    {
        get => currentXP;
        set => AddXP(Mathf.Max(0, currentXP - value));
    }

    public int CurrentLevel => currentLevel;

    private void FixedUpdate()
    {
        Collider[] pickups = Physics.OverlapSphere(transform.position, pickupRange, pickupLayer);

        if (pickups != null)
            foreach (var pickup in pickups)
            {
                AddXP();
                Destroy(pickup.gameObject);
            }
    }

    private void Awake()
    {
        currentXPReq = Mathf.CeilToInt(_XPReqScaling.Evaluate(currentLevel));
    }

    public void AddLevel(uint levelsToAdd = 1)
    {
        for (int i = 0; i < levelsToAdd; i++)
            AddXP(currentXPReq - currentXP);
    }

    public void AddXP(int toAdd = 1)
    {
        int prevLevel = currentLevel;
        int prevXP = currentXP;
        
        currentXP += toAdd;
        
        _onXPGained.Invoke(prevXP, currentLevel);
        OnXPGainedEvent?.Invoke(prevXP, currentLevel);
        
        while (currentXP >= currentXPReq)
        {
            currentLevel++;
            currentXP -= currentXPReq;
            currentXPReq = Mathf.CeilToInt(_XPReqScaling.Evaluate(currentLevel));

            _onLevelGained.Invoke(prevLevel, currentLevel);
            OnLevelsGainedEvent?.Invoke(prevLevel, currentLevel);
        }
    }

    public void UpdateHUD()
    {
        xpBar.FillAmount = (float)currentXP / currentXPReq;
    }
}
