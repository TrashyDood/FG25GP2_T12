using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject settingUI;
    [SerializeField] private GameObject creditsUI;

    public MenuState state;
    public enum MenuState
    {
        MainMenu = 0,
        Settings = 1
    }
    private void Update()
    {
        switch (state)
        {
            case MenuState.MainMenu:
                break;
            case MenuState.Settings:
                break;
        }
    }
    public void SwitchState(int aState)
    {
        SwitchState((MenuState)aState);
    }
    public void SwitchState(MenuState aState)
    {
        menuUI.SetActive(aState == MenuState.MainMenu);
        settingUI.SetActive(aState == MenuState.Settings);

        state = aState;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("You have quit the game");
    }
}
