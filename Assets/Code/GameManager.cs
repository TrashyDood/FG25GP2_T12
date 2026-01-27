using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region Singelton
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            throw new System.Exception("2 Singletons of type [GamesManager exist]");
        }
        instance = this;
    }
    #endregion

    public enum GameState
    {
        Playing,
        Paused,
        Upgrading,
        Dead,
        Victory,
        Menu
    }
    public GameState currentState;
    public GameState GetCurrentState => currentState;

    public bool GamePaused;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Playing)
                SwitchState(GameState.Paused);
            else if (currentState == GameState.Paused)
                SwitchState(GameState.Playing);
        }
    }

    public void SwitchState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.Playing:
                SetPausedGame(false);
                break;

            case GameState.Paused:
                SetPausedGame(true);
                break;

            case GameState.Upgrading:
                SetPausedGame(true);
                break;

            case GameState.Menu:
            case GameState.Dead:
            case GameState.Victory:
                SetPausedGame(true);
                break;
        }
    }


    public void SetPausedGame(bool paused)
    {
        GamePaused = paused;

        Time.timeScale = GamePaused ? 0 : 1;
    }

    public void EnterUpgradingState()
    {
        SwitchState(GameState.Upgrading);
    }
}
