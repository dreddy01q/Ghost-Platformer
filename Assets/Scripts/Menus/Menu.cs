using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private GameObject menuDisplay;

    private protected void Start()
    {
        menuDisplay = GetComponent<GameObject>();
    }

    public void DisplayMenu(bool value)
    {
        menuDisplay.SetActive(value);
    }

    public void LoadMenuScene(string scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
