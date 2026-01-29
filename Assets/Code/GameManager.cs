using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    RectTransform levelUpScreen;
    [SerializeField]
    EventState[] genericStates;

    StateManager stateManager;

    public StateManager StateManager => stateManager;
    public static GameManager Instance { get; private set; }
    public Player Player { get; private set; }

    private void Awake()
    {
        if (Instance != this)
            Destroy(Instance);

        Player = GameObject.FindFirstObjectByType<Player>();
        Instance = this;
        stateManager.Initialize(genericStates);
    }

    public void SetState(GameState newState)
    {
        
    }
}

public enum GameState
{
    Playing = 0,
    Paused = 1,
    Upgrading = 2,
    Dead = 3,
    Victory = 4,
    Menu = 5
}

[Serializable]
public class EventState : State
{
    [SerializeField]
    UnityEvent<State> onEnter, onExit;

    public override void Evaluate() { }

    public override void OnEnter(State prevState) => onEnter.Invoke(prevState);
    public override void OnExit(State newState) => onExit.Invoke(newState);
}

[Serializable]
public class StateManager
{
    [SerializeField]
    UnityEvent<State> onStateChanged;
    [SerializeField]
    int currentIndex = -1;

    List<State> states = new();

    State currentState;

    public event Action<State> OnStateChanged;

    public void Initialize(params State[] states) =>
        AddStates(states);

    public State CurrentState
    {
        get => currentState;
        set
        {
            currentState?.OnExit(value);
            value?.OnEnter(currentState);
            OnStateChanged?.Invoke(value);
            onStateChanged.Invoke(value);

            currentState = value;
        }
    }

    public void AddStates(ICollection<State> states)
    {
        foreach (var state in states)
            AddState(state);
    }

    public int AddState(State newState)
    {
        newState.Initialize();
        states.Add(newState);
        return states.Count - 1;
    }

    public int AddState<T>() where T : State, new()
    {
        T newState = new T();
        return AddState(newState);
    }

    public void SetState(int stateIndex)
    {
        CurrentState = states[stateIndex];
        currentIndex = stateIndex;
    }

    public void SetState<TEnum>(TEnum newState) where TEnum : Enum =>
        CurrentState = states[Convert.ToInt32(newState)];

    public State GetState<TEnum>(TEnum state) where TEnum : Enum =>
        states[Convert.ToInt32(state)] ?? null;

    public void Evaluate()
    {
        if (currentState != null)
            currentState.Evaluate();
    }
}

[Serializable]
public abstract class State
{
    public virtual void Initialize() { }
    public abstract void OnEnter(State prevState);
    public abstract void OnExit(State newState);
    public virtual void Evaluate() { }
}