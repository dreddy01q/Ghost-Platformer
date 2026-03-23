using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class MainMenu : MonoBehaviour
{
    public string mainGameScene;
    public string multiplayerGameScene;

    public void LoadGame()
	{
		SceneManager.LoadSceneAsync(mainGameScene);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

    public void StartGame()
    {
        PlayerInstance.PlayerHost = true;
        SceneManager.LoadSceneAsync(multiplayerGameScene);
    }

    public void JoinGame()
    {
        PlayerInstance.PlayerHost = false;
        SceneManager.LoadSceneAsync(multiplayerGameScene);
    }

}
