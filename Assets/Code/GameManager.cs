using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.LowLevel;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    RectTransform buildingScreen,
        upgradeScreen;
    [SerializeField]
    StateManager stateManager;

    public StateManager StateManager => stateManager;
    public static GameManager Instance { get; private set; }
    public Player Player { get; private set; }

    private void Awake()
    {
        if (Instance != this)
            Destroy(Instance);

        Player = FindFirstObjectByType<Player>();
        Instance = this;

        CallbackState[] states = new CallbackState[]
        {
            new(), // Playing
            new(), // Paused
            new() // Upgrading
            {
                onEnterCallback = (state) =>
                {
                    Time.timeScale = 0;
                    Player.Body.EnterEditMode(() => { stateManager.SetState(GameState.Playing); });
                },
                onExitCallback = (state) =>
                {
                    Time.timeScale = 1;
                }
            },
            new(), // Dead
            new(), // Victory
            new() // Menu
        };

        stateManager = new(states);
        StateManager.SetState(GameState.Playing);
    }

    private void Update()
    {
        stateManager.Evaluate(Time.fixedDeltaTime);
    }

    public void SetState(int state) =>
        stateManager.SetState(state);
}

public enum GameState
{
    Playing,
    Paused,
    Upgrading,
    Dead,
    Victory,
    Menu
}

[Serializable]
public class CallbackState : State
{
    [SerializeField]
    UnityEvent<State> onEnter,
        onExit;

    public Action<float> onEvaluateCallback;
    public Action<State> onEnterCallback,
        onExitCallback;

    public override void OnEnter(State prevState)
    {
        onEnter?.Invoke(prevState);
        onEnterCallback?.Invoke(prevState);
    }

    public override void OnExit(State newState)
    {
        onExit?.Invoke(newState);
        onExitCallback?.Invoke(newState);
    }

    public override void Evaluate(float deltaTime)
    {
        onEvaluateCallback?.Invoke(deltaTime);
    }
}

[Serializable]
public abstract class State
{
    public virtual void Initialize() { }
    public abstract void OnEnter(State prevState);
    public abstract void OnExit(State newState);
    public virtual void Evaluate(float deltaTime) { }
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

    public StateManager(IEnumerable<State> states = null)
    {
        if (states != null)
            this.states = states.ToList();
    }

    public State CurrentState
    {
        get => currentState;
        set
        {
            currentState?.OnExit(value);
            value?.OnEnter(currentState);
            OnStateChanged?.Invoke(value);
            onStateChanged?.Invoke(value);

            currentState = value;
        }
    }

    public void AddStates(IEnumerable<State> states)
    {
        foreach (var state in states)
            AddState(state);
    }

    public void AddState(State state, int index = -1)
    {
        state.Initialize();

        if (index < 0)
            states.Add(state);
        else if (index >= states.Count)
            states.Insert(index, state);
        else
            states[index] = state;
    }

    public void AddState<TEnum>(TEnum index, State state) where TEnum : Enum =>
        AddState(state, Convert.ToInt32(index));

    public void SetState(int stateIndex)
    {
        CurrentState = states[stateIndex];
        currentIndex = stateIndex;
    }

    public void SetState<TEnum>(TEnum newState) where TEnum : Enum =>
        CurrentState = states[Convert.ToInt32(newState)];

    public State GetState<TEnum>(TEnum state) where TEnum : Enum =>
        states[Convert.ToInt32(state)] ?? null;

    public void Evaluate(float deltaTime)
    {
        currentState.Evaluate(deltaTime);
    }
}