using Unity.Netcode;

public class PlayerInstance : NetworkBehaviour
{
    private static bool playerHost = false;
    private static ulong playerClientId;

    public static bool PlayerHost { get => playerHost; set => playerHost = value; }
    public static ulong PlayerClientId { get => playerClientId; set => playerClientId = value; }
}
