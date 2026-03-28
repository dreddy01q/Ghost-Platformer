using Unity.Netcode;
using UnityEngine;

public class PlayerInvisibility : NetworkBehaviour
{
    PlayerController playerController;
    private void Start()
    {
        playerController=GetComponent<PlayerController>();
    }
    public void OnInvisible(bool invisible)
    {
        playerController.IsInvisible = invisible;

        if (invisible)
        {
            //NetworkObject networkPlayer = playerController.GameManage.PlayerManager.NetworkPlayers[playerController.PlayerArrayId];
            SetPlyApperanceSeverRpc(playerController.PlayerArrayId, false);
            //playerController.plyAppereance.SetActive(false);
        }
        else
        {
            //NetworkObject networkPlayer = playerController.GameManage.PlayerManager.NetworkPlayers[playerController.PlayerArrayId];
            SetPlyApperanceSeverRpc(playerController.PlayerArrayId, true);
            //playerController.plyAppereance.SetActive(true);
        }
    }

    [Rpc(SendTo.ClientsAndHost, InvokePermission = RpcInvokePermission.Everyone)]
    private void SetPlyApperanceSeverRpc(int playerArradyId, bool value)
    {
        //NetworkObject networkPlayer = playerController.GameManage.PlayerManager.NetworkPlayers[playerArradyId];
        //networkPlayer.GetComponent<PlayerController>().plyAppereance.SetActive(value);

        SetLocalApperance(playerArradyId, value);
    }

    private void SetLocalApperance(int playerArradyId, bool value)
    {
        foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            PlayerController controller = player.GetComponent<PlayerController>();
            if (controller.PlayerArrayId == playerArradyId)
            {
                controller.plyAppereance.SetActive(value);
            }
        }
    }
}
