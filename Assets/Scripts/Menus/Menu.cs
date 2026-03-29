using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject menuDisplay;

    private protected void Start()
    {
        //menuDisplay = GetComponentInChildren<GameObject>();
    }

    public void DisplayMenu(bool value)
    {
        menuDisplay.SetActive(value);
    }

    public void LoadMenuScene(string scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }

    public void CloseMenu()
    {
        DisplayMenu(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
