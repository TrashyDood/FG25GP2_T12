using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    RectTransform levelUpScreen;

    public static GameManager Instance { get; private set; }
    public Player Player { get; private set; }

    private void Awake()
    {
        if (Instance != this)
            Destroy(Instance);

        Player = GameObject.FindFirstObjectByType<Player>();
        Instance = this;
    }

    private void Start()
    {
        SwitchState(GameState.Playing);
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
                Player.Body.RunEditMode(() => SwitchState(GameState.Playing));
                break;
            case GameState.Dead:

                break;
            case GameState.Victory:
                
                break;
            case GameState.Menu:

                break;
        }
    }

    public void SwitchState(int newState) =>
        SwitchState((GameState)newState);

    public void SwitchState(GameState newState)
    {
        switch (currentState)
        {
            case GameState.Playing:

                break;
            case GameState.Paused:

                break;
            case GameState.Upgrading:
                Time.timeScale = 1;
                levelUpScreen?.gameObject.SetActive(false);
                break;
            case GameState.Dead:

                break;
            case GameState.Victory:

                break;
            case GameState.Menu:

                break;
        }

        currentState = newState;

        switch (currentState)
        {
            case GameState.Playing:

                break;
            case GameState.Paused:

                break;
            case GameState.Upgrading:
                Time.timeScale = 0;
                levelUpScreen?.gameObject.SetActive(true);
                break;
            case GameState.Dead:

                break;
            case GameState.Victory:

                break;
            case GameState.Menu:

                break;
        }
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