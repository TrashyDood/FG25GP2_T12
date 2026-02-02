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
        _onLevelsGained;

    public event Action<int, int> OnXPGainedEvent,
        OnLevelsGainedEvent;

    [SerializeField]
    int _currentXP,
        _currentLevel,
        _currentXPReq;

    public int CurrentXP
    {
        get => _currentXP;
        set => AddXP(Mathf.Max(0, _currentXP - value));
    }

    public int CurrentLevel => _currentLevel;

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

    public void AddXP(int toAdd = 1)
    {
        int prevLevel = _currentLevel;
        int prevXP = _currentXP;
        
        _currentXP += toAdd;
        
        _onXPGained.Invoke(prevXP, _currentLevel);
        OnXPGainedEvent?.Invoke(prevXP, _currentLevel);
        
        while (_currentXP >= _currentXPReq)
        {
            _currentLevel++;
            _currentXP -= _currentXPReq;
            _currentXPReq = Mathf.CeilToInt(_XPReqScaling.Evaluate(_currentLevel));
        }
        
        _onLevelsGained.Invoke(prevLevel, _currentLevel);
        OnLevelsGainedEvent?.Invoke(prevLevel, _currentLevel);
    }
    public void UpdateHUD()
    {
        xpBar.FillAmount = (float)_currentXP / _currentXPReq;
    }
}
