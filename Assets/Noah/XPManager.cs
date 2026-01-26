using System;
using UnityEngine;
using UnityEngine.Events;

public class XPManager : MonoBehaviour
{
    [SerializeField]
    AnimationCurve _XPReqScaling;
    [SerializeField]
    UnityEvent<int, int> _onXPGained,
        _onLevelsGained;

    public event Action<int, int> OnXPGainedEvent,
        OnLevelsGainedEvent;

    int _currentXP,
        _currentLevel,
        _currentXPReq;

    public int CurrentXP
    {
        get => _currentXP;
        set => AddXP(Mathf.Max(0, _currentXP - value));
    }

    public int CurrentLevel => _currentLevel;

    public void AddXP(int toAdd)
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
            _currentXPReq = Mathf.CeilToInt(_currentLevel);
        }
        
        _onLevelsGained.Invoke(prevLevel, _currentLevel);
        OnLevelsGainedEvent?.Invoke(prevLevel, _currentLevel);
    }
}
