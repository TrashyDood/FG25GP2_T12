using System;
using UnityEngine;
using UnityEngine.Events;

public class XPManager : MonoBehaviour
{
    [SerializeField]
    AnimationCurve XPReqScaling;
    [SerializeField]
    float pickupRange = 5;
    [SerializeField]
    LayerMask pickupLayer;

    [Header("Events")]
    [SerializeField]
    UnityEvent<float, float> onXPGainedEvent,
        onLevelGainedEvent;

    public event Action<int, int> OnXPGainedEvent,
        OnLevelsGainedEvent;

    int currentXP,
        currentLevel,
        currentXPReq;

    public int CurrentLevel => currentLevel;

    public int CurrentXP
    {
        get => currentXP;
        set => AddXP(Mathf.Max(0, currentXP - value));
    }

    private void FixedUpdate()
    {
        Collider[] pickups = Physics.OverlapSphere(transform.position, pickupRange, pickupLayer);

        if (pickups != null)
            foreach (var pickup in pickups)
                pickup.GetComponent<XPPickup>().OnPickup(gameObject);
    }

    private void Awake()
    {
        currentXPReq = Mathf.CeilToInt(XPReqScaling.Evaluate(currentLevel));
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
        
        onXPGainedEvent.Invoke(currentXP, currentXPReq);
        OnXPGainedEvent?.Invoke(currentXP, currentXPReq);
        
        while (currentXP >= currentXPReq)
        {
            currentLevel++;
            currentXP -= currentXPReq;
            currentXPReq = Mathf.CeilToInt(XPReqScaling.Evaluate(currentLevel));

            onLevelGainedEvent.Invoke(prevLevel, currentLevel);
            OnLevelsGainedEvent?.Invoke(prevLevel, currentLevel);
        }
    }
}
