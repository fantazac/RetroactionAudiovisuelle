using UnityEngine;

public class StaticObjects : MonoBehaviour
{
    public static GameObject Player { get; set; }
    public static MouseManager PlayerMouseManager { get; set; }
    public static GameController GameController { get; set; }
    public static PlayerMovement PlayerMovement { get; set; }
    public static PickaxeManager PlayerPickaxeManager { get; set; }
    public static SoundEffectManager SoundEffectManager { get; set; }
    public static FoleySoundEffectManager FoleySoundEffectManager { get; set; }
    public static BGMManager BGMManager { get; set; }
}
