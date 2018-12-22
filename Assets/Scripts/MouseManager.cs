using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public Rock HoveredRock { get; private set; }

    private RaycastHit hit;

    private void Start()
    {
        GetComponent<InputManager>().OnLeftClick += OnLeftClick;
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
            Debug.Log("Clicked rock!");
        }
    }
}
