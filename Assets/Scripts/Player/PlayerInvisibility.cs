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
            SetPlyApperanceRpc(playerController.PlayerArrayId, false);
        }
        else
        {
            SetPlyApperanceRpc(playerController.PlayerArrayId, true);
        }
    }

    [Rpc(SendTo.ClientsAndHost, InvokePermission = RpcInvokePermission.Everyone)]
    public void SetPlyApperanceRpc(int playerArradyId, bool value)
    {
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
