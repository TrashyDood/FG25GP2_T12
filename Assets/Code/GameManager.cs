using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    RectTransform buildingScreen,
        upgradeScreen;
    [SerializeField]
    StateManager stateManager;
    [SerializeField]
    CallbackState[] states;

    public StateManager StateManager => stateManager;
    public static GameManager Instance { get; private set; }
    public Player Player { get; private set; }

    private void Awake()
    {
        if (Instance != this)
            Destroy(Instance);

        Player = FindFirstObjectByType<Player>();
        Instance = this;

        states[(int)GameState.Playing].onEnterCallback = (state) => { Time.timeScale = 1; };
        states[(int)GameState.Upgrading].onEnterCallback = (state) => { Time.timeScale = 0; };
        states[(int)GameState.Upgrading].onExitCallback += (state) => { Time.timeScale = 1; };
        states[(int)GameState.Dead].onEnterCallback = (state) => { StartCoroutine(Timer(() => { Time.timeScale = 0; })); };

        stateManager.AddStates(states);
        StateManager.SetState(GameState.Playing);
    }

    private void Start()
    {
        StartCoroutine(Timer(() => { stateManager.SetState(GameState.Upgrading); }));
    }

    private void FixedUpdate() =>
        stateManager.Evaluate(Time.fixedDeltaTime);

    public void SetState(int state) =>
        stateManager.SetState(state);

    public CallbackState GetCBState(GameState state) =>
        (CallbackState)stateManager.GetState(state);

    public void RestartCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator Timer(Action callback, float timer = 1)
    {
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        callback?.Invoke();
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

    public override void Evaluate(float deltaTime) =>
        onEvaluateCallback?.Invoke(deltaTime);
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

    public int CurrentIndex => currentIndex;

    public event Action<State> OnStateChanged;

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

    public void AddState(State state)
    {
        state.Initialize();
        states.Add(state);
    }

    public void SetState(int stateIndex)
    {
        CurrentState = states[stateIndex];
        currentIndex = stateIndex;
    }

    public void SetState<TEnum>(TEnum newState) where TEnum : Enum =>
        SetState(Convert.ToInt32(newState));

    public State GetState<TEnum>(TEnum state) where TEnum : Enum =>
        states[Convert.ToInt32(state)];

    public void Evaluate(float deltaTime)
    {
        currentState.Evaluate(deltaTime);
    }
}