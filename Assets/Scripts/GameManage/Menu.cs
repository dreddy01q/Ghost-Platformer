using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string mainGameScene = "MainLevel";
    public string mainMenuScene = "MainMenu";

    public void LoadGame()
    {
        SceneManager.LoadSceneAsync(mainGameScene);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(mainMenuScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
