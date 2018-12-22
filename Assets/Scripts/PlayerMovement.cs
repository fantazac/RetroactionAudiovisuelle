using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool moveLeft;
    private bool moveRight;
    private bool moveUp;
    private bool moveDown;

    private float movementSpeed;

    private PickaxeManager pickaxeManager;

    private PlayerMovement()
    {
        movementSpeed = 2.5f;
    }

    private void Start()
    {
        pickaxeManager = GetComponent<PickaxeManager>();
    }

    public void SetupMovementInputs(InputManager inputManager)
    {
        inputManager.OnMoveLeft += OnMoveLeft;
        inputManager.OnMoveRight += OnMoveRight;
        inputManager.OnMoveUp += OnMoveUp;
        inputManager.OnMoveDown += OnMoveDown;
    }

    private void Update()
    {
        if (!pickaxeManager.IsHittingRock)
        {
            if (moveLeft && !moveRight && Utility.World.CanMove(transform.position + Vector3.left * movementSpeed * Time.deltaTime))
            {
                transform.position += Vector3.left * movementSpeed * Time.deltaTime;
            }
            if (moveRight && !moveLeft && Utility.World.CanMove(transform.position + Vector3.right * movementSpeed * Time.deltaTime))
            {
                transform.position += Vector3.right * movementSpeed * Time.deltaTime;
            }
            if (moveUp && !moveDown && Utility.World.CanMove(transform.position + Vector3.forward * movementSpeed * Time.deltaTime))
            {
                transform.position += Vector3.forward * movementSpeed * Time.deltaTime;
            }
            if (moveDown && !moveUp && Utility.World.CanMove(transform.position + Vector3.back * movementSpeed * Time.deltaTime))
            {
                transform.position += Vector3.back * movementSpeed * Time.deltaTime;
            }
        }
    }

    private void OnMoveLeft(bool move)
    {
        moveLeft = move;
    }

    private void OnMoveRight(bool move)
    {
        moveRight = move;
    }

    private void OnMoveUp(bool move)
    {
        moveUp = move;
    }

    private void OnMoveDown(bool move)
    {
        moveDown = move;
    }

    public void StopAllMovement()
    {
        OnMoveLeft(false);
        OnMoveRight(false);
        OnMoveUp(false);
        OnMoveDown(false);
    }
}
