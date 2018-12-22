using UnityEngine;

public class MouseEvent : MonoBehaviour
{
    private Rock rock;

    private void Start()
    {
        rock = GetComponent<Rock>();
    }

    private void OnMouseEnter()
    {
        StaticObjects.PlayerMouseManager.HoverRock(rock);
    }

    private void OnMouseExit()
    {
        StaticObjects.PlayerMouseManager.UnhoverRock(rock);
    }
}
