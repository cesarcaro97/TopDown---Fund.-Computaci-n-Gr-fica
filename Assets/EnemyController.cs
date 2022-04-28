using PathFinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 1;

    Vector2 targetPosition;

    private void Start()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        if(IsMoving())
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        else
        {
            transform.position = targetPosition;
            targetPosition = transform.position;
        }
    }

    public void SetTargetPos(Vector2 target)
    {
        targetPosition = target;
        transform.rotation = GetMovementRotation(GetMovementDirection(targetPosition));
    }

    private MovementDirection GetMovementDirection(Vector2 targetPosition)
    {
        if (targetPosition.x < transform.position.x)
            return MovementDirection.Left;
        else if (targetPosition.x > transform.position.x)
            return MovementDirection.Right;
        else if (targetPosition.y < transform.position.y)
            return MovementDirection.Down;
        else if (targetPosition.y > transform.position.y)
            return MovementDirection.Up;

        return MovementDirection.Idle;
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

    public bool IsMoving()
    {
        return Vector3.Distance(transform.position, targetPosition) > 0.05f;
    }

    internal bool HasTarget()
    {
        return (Vector3)targetPosition != transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.root.tag == "Player")
        {
            GameManager.Instance.GameOver();
        }
    }
}
