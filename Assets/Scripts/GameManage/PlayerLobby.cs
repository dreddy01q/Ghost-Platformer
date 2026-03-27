using System.Linq;
using System.Net;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLobby : NetworkBehaviour
{
    public string MainLevel;

    public TextMeshProUGUI PlayerCountDisplay;
    public TextMeshProUGUI TestDetails;
    public void StartGame()
    {
        //SceneManager.LoadScene(MainLevel);
        NetworkManager.SceneManager.LoadScene(MainLevel,LoadSceneMode.Single);
    }

    public void JoinLobby()
    {
        NetworkManager.Singleton.StartHost();
        Debug.Log("Host Joined");
    }

    private void Update()
    {
        UpdatePlayerCount();
        UpdateTestDetails();
    }

    public void UpdatePlayerCount()
    {
        if (PlayerNetworkManager.isHost)
        {
            PlayerCountDisplay.text = PlayerNetworkManager.PlayerIds.Count + " players joined";
        }
        else
        {
            PlayerCountDisplay.text = "Waiting for host to start...!";
        }
    }

    public void UpdateTestDetails()
    {
        var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        TestDetails.text = GetLocalIPv4() + "/" + utp.ConnectionData.Port;
    }
    public string GetLocalIPv4()
    {
        return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(
        f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        .ToString();
    }

}
