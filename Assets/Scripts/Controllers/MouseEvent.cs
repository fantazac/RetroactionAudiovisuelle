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
        if (StaticObjects.PlayerMouseManager)
        {
            StaticObjects.PlayerMouseManager.HoverRock(rock);
        }
    }

    private void OnMouseExit()
    {
        if (StaticObjects.PlayerMouseManager)
        {
            StaticObjects.PlayerMouseManager.UnhoverRock(rock);
        }
    }
}
