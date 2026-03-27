using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : Menu
{
    // Scenes
    public string mainGameScene;
    public string PlayerLobbyScene;

    private PlayerNetworkManager playerNetworkManager;


    // Join Game Pop Up
    [Header("Join Game Pop Up")]
    public GameObject JoinGamePopUp;
    private string HostIPAddress;
    public TMP_InputField HostIPInputfield;

    private void Start()
    {
        base.Start();
        playerNetworkManager=GetComponent<PlayerNetworkManager>();
    }


    public void HostGameBt()
    {
        playerNetworkManager.JoinPlayerHost();
        base.LoadMenuScene(PlayerLobbyScene);
    }

    public void JoinGameBt()
    {
        JoinGamePopUp.SetActive(true);
    }

    public void JoinGamePopUpBt()
    {
        HostIPAddress = HostIPInputfield.text;
        if (string.IsNullOrEmpty(HostIPAddress)) {
            return;
        }
        playerNetworkManager.JoinPlayerClient(HostIPAddress);
        base.LoadMenuScene(PlayerLobbyScene);
    }



    public void StartGame()
    {
        PlayerInstance.PlayerHost = true;
        //SceneManager.LoadSceneAsync(multiplayerGameScene);
    }

    public void JoinGame()
    {
        PlayerInstance.PlayerHost = false;
        //SceneManager.LoadSceneAsync(multiplayerGameScene);
    }

}
