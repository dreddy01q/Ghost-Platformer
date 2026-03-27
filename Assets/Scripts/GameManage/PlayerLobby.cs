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

    [Header("Host Pop Up")]
    public GameObject HostDetailsBt;
    public Menu HostDetailsPopup;
    public TextMeshProUGUI HostDetails;

    public TextMeshProUGUI PlayerCountDisplay;

    public Menu ClientWarningPopup;

    private void Start()
    {
        if (PlayerNetworkManager.isHost)
        {
            HostDetailsBt.SetActive(true);
        }
    }

    private void Update()
    {
        UpdatePlayerCount();
    }

    public void StartGame()
    {
        if (PlayerNetworkManager.isHost)
        {
            NetworkManager.SceneManager.LoadScene(MainLevel, LoadSceneMode.Single);
        }
        else
        {
            ClientWarningPopup.DisplayMenu(true);
        }
    }

    public void UpdatePlayerCount()
    {
        if (PlayerNetworkManager.isHost)
        {
            PlayerCountDisplay.text = PlayerNetworkManager.PlayerIds.Count + " players joined";
        }
        else
        {
            PlayerCountDisplay.text = "Waiting for host to start...";
        }
    }


    public void DisplayHostDetails()
    {
        HostDetails.text = getHostDetails();
        HostDetailsPopup.DisplayMenu(true);
    }
    private string getHostDetails()
    {
        var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();

        string hostIpConnection = "Host IP: " + GetLocalIPv4();
        string portConnection="Port: "+ utp.ConnectionData.Port;

        return hostIpConnection + "\n" + portConnection;

    }
    private string GetLocalIPv4()
    {
        return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(
        f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        .ToString();
    }

}
