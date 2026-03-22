using UnityEngine;

public class PlayerInvisibility : MonoBehaviour
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
            playerController.plyAppereance.SetActive(false);
        }
        else
        {
            playerController.plyAppereance.SetActive(true);
        }
    }
}
