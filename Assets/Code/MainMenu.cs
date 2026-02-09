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
        Settings = 1,
        //Credits = 2
    }
    private void Update()
    {
        switch (state)
        {
            case MenuState.MainMenu:
                break;
            case MenuState.Settings:
                break;
            //case MenuState.Credits:
            //    break;
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
        //creditsUI.SetActive(aState == MenuState.Credits);

        state = aState;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Beta_Demo");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("You have quit the game");
    }
}
