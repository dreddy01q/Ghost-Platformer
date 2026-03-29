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

    [Header("Local/LAN")]
    public Menu LocalGameMenu;
    public Menu LANGameMenu;


    // Join Game Pop Up
    [Header("Join LAN Game Pop Up")]
    public Menu JoinGamePopUp;
    private string HostIPAddress;
    public TMP_InputField HostIPInputfield;

    private void Start()
    {
        base.Start();
        playerNetworkManager=GetComponent<PlayerNetworkManager>();
    }

    public void OpenLocalGame()
    {
        LocalGameMenu.DisplayMenu(true);
    }

    public void LocalHostGameBt()
    {
        playerNetworkManager.JoinPlayerHost(false);
        base.LoadMenuScene(PlayerLobbyScene);
    }

    public void LocalJoinGameBt()
    {
        playerNetworkManager.JoinPlayerClient(false);
        base.LoadMenuScene(PlayerLobbyScene);
    }



    public void OpenLANGame()
    {
        LANGameMenu.DisplayMenu(true);
    }

    public void LANHostGameBt()
    {
        playerNetworkManager.JoinPlayerHost(true);
        base.LoadMenuScene(PlayerLobbyScene);
    }

    public void LANJoinGameBt()
    {
        JoinGamePopUp.DisplayMenu(true);
    }

    public void JoinGamePopUpBt()
    {
        HostIPAddress = HostIPInputfield.text;
        if (string.IsNullOrEmpty(HostIPAddress)) {
            return;
        }
        playerNetworkManager.JoinPlayerClient(true, HostIPAddress);
        base.LoadMenuScene(PlayerLobbyScene);
    }
}
