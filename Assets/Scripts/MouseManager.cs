using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public Rock HoveredRock { get; private set; }

    private RaycastHit hit;

    private PickaxeManager pickaxeManager;

    private void Start()
    {
        GetComponent<InputManager>().OnLeftClick += OnLeftClick;
        pickaxeManager = GetComponent<PickaxeManager>();
    }

    public void HoverRock(Rock hoveredRock)
    {
        HoveredRock = hoveredRock;
    }

    public void UnhoverRock(Rock unhoveredRock)
    {
        if (HoveredRock == unhoveredRock)
        {
            HoveredRock = null;
        }
    }

    private void OnLeftClick(Vector3 mousePosition)
    {
        if (HoveredRock != null)
        {
            pickaxeManager.HitRockInRange(HoveredRock);
        }
    }
}
