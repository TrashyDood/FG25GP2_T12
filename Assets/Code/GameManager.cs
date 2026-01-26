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
