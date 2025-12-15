using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string mainGameScene;

	public void LoadGame()
	{
		SceneManager.LoadSceneAsync(mainGameScene);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

}
