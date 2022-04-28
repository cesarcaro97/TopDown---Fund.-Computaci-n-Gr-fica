using System;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private LayerMask wallsLayer;
    [Range(0, 1)]
    [SerializeField] private float movementInputDeadTime = 0.25f;

    private Vector2 _inputs;
    private Vector2 targetPosition;
    private bool reachedTargetPos = false;
    float elapsedInputTime = 0;

    private CircleCollider2D col = null;

    private void Awake()
    {
        col = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (!GameManager.Instance.LevelStarted) return;

        if (IsMoving())
        {
            if (_inputs.magnitude != 0)
                _inputs = Vector2.zero;

            if (reachedTargetPos)
                reachedTargetPos = false;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            if (!reachedTargetPos)
            {
                transform.position = targetPosition;
                reachedTargetPos = true;
                elapsedInputTime = 0;
            }

            elapsedInputTime += Time.deltaTime;

            if(elapsedInputTime >= movementInputDeadTime)
                _inputs = Vector2.right * Input.GetAxisRaw("Horizontal") + Vector2.up * Input.GetAxisRaw("Vertical");

            MovementDirection movementDirection = GetMovementDirection(_inputs);
            targetPosition = GetPosition(transform.position, movementDirection);

            if (movementDirection != MovementDirection.Idle)
                transform.rotation = GetMovementRotation(movementDirection);

            if (!CanMove(targetPosition))
            {   
                targetPosition = transform.position;
                _inputs = Vector2.zero;
            }
        }

    }

    private Quaternion GetMovementRotation(MovementDirection movementDirection)
    {
        switch (movementDirection)
        {
            case MovementDirection.Up:
                return Quaternion.AngleAxis(90, Vector3.forward);
            case MovementDirection.Right:
                return Quaternion.AngleAxis(0, Vector3.forward);
            case MovementDirection.Down:
                return Quaternion.AngleAxis(270, Vector3.forward);
            case MovementDirection.Left:
                return Quaternion.AngleAxis(180, Vector3.forward);
            default:
                return Quaternion.identity;
        }
    }

    private bool CanMove(Vector3 toPosition)
    {
        return !Physics2D.OverlapCircle(toPosition, col.radius, wallsLayer);
    }

    private bool IsMoving()
    {
        return Vector3.Distance(transform.position, targetPosition) > 0.05f;
    }

    private Vector2 GetPosition(Vector2 from, MovementDirection direction)
    {
        Vector2 targetPos = from;
        switch (direction)
        {
            case MovementDirection.Up:
                targetPos += Vector2.up;
                break;
            case MovementDirection.Right:
                targetPos += Vector2.right;
                break;
            case MovementDirection.Down:
                targetPos += Vector2.down;
                break;
            case MovementDirection.Left:
                targetPos += Vector2.left;
                break;
        }

        return targetPos;
    }

    private MovementDirection GetMovementDirection(Vector2 inputs)
    {
        //The only way we can move is when we just move in one direction, won't admit digonal movement
        if (inputs.x == 0 && inputs.y != 0)
        {
            if (inputs.y == 1)
            {
                return MovementDirection.Up;
            }
            else
                if (inputs.y == -1)
            {
                return MovementDirection.Down;
            }
        }
        else if (inputs.x != 0 && inputs.y == 0)
        {
            if (inputs.x == 1)
            {
                return MovementDirection.Right;
            }
            else
                if (inputs.x == -1)
            {
                return MovementDirection.Left;
            }
        }

        return MovementDirection.Idle;
    }
}
