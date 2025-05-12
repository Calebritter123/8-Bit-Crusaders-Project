using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    public Transform leftEdge;
    public Transform rightEdge;

    [Header("Enemy Object (with Sprite & Animator)")]
    public Transform enemy;

    [Header("Movement parameters")]
    public float speed = 2.5f;
    private bool movingLeft = true;

    [Header("Idle Behaviour")]
    public float idleDuration = 1f;
    private float idleTimer;

    private Animator anim;
    private Rigidbody2D rb;
    private Vector3 initScale;

    private void Awake()
    {
        if (enemy != null)
        {
            anim = enemy.GetComponent<Animator>();
            rb = enemy.GetComponent<Rigidbody2D>();
            initScale = enemy.localScale;
        }
        else
        {
            Debug.LogError("Enemy (child with sprite & rigidbody) is not assigned!");
        }
    }

    private void Update()
    {
        if (enemy == null || anim == null || rb == null)
            return;

        bool isMoving = false;

        if (movingLeft)
        {
            if (enemy.position.x > leftEdge.position.x)
            {
                MoveInDirection(-1);
                isMoving = true;
            }
            else
            {
                StartIdle();
            }
        }
        else
        {
            if (enemy.position.x < rightEdge.position.x)
            {
                MoveInDirection(1);
                isMoving = true;
            }
            else
            {
                StartIdle();
            }
        }

        anim.SetBool("moving", isMoving);
    }

    private void StartIdle()
    {
        anim.SetBool("moving", false);
        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDuration)
        {
            movingLeft = !movingLeft;
            idleTimer = 0;
        }

        // Stop horizontal movement during idle
        if (rb != null)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    private void MoveInDirection(int direction)
    {
        idleTimer = 0;

        // Move using Rigidbody2D
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        // Flip sprite based on direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);
    }
}
