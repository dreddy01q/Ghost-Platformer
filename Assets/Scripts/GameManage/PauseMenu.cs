using Unity.Netcode;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public Menu PauseMenuDisplay;
    private PlayerNetworkManager playerNetworkManager;
    private SceneLoader sceneLoader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerNetworkManager=GetComponent<PlayerNetworkManager>();
        sceneLoader=GetComponent<SceneLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            displayPause();
        }
    }

    private void displayPause()
    {
        PauseMenuDisplay.DisplayMenu(true);
    }

    public void ReturnToLobby()
    {
        sceneLoader.LoadPlayerLobby();
    }

    public void LeaveGame()
    {
        playerNetworkManager.DisconnectClient();
    }
}
