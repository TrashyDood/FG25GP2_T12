using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != this)
            Destroy(Instance);

        Instance = this;
    }

    public GameState currentState;
    public GameState GetCurrentState => currentState;

    private void Update()
    {
        switch (currentState)
        {
            case GameState.Playing:

                break;
            case GameState.Paused:

                break;
            case GameState.Upgrading:

                break;
            case GameState.Dead:

                break;
            case GameState.Victory:
                
                break;
            case GameState.Menu:

                break;
        }
    }

    public void SwitchState(GameState aState)
    {
        currentState = aState;
    }
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