using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    public Transform leftEdge;
    public Transform rightEdge;

    [Header("Child object with sprite & animator")]
    public Transform enemy; // Assign your visible child object here (e.g., "MeleeEnemy")

    [Header("Movement parameters")]
    public float speed = 2.5f;
    private bool movingLeft = true;

    [Header("Idle Behaviour")]
    public float idleDuration = 1f;
    private float idleTimer;

    [Header("Animator")]
    private Animator anim;

    private Vector3 initScale;

    private void Awake()
    {
        if (enemy != null)
        {
            anim = enemy.GetComponent<Animator>();
            initScale = enemy.localScale;
        }
        else
        {
            Debug.LogError("Enemy (child with sprite & animator) is not assigned!");
        }
    }

    private void Update()
    {
        if (enemy == null || anim == null)
            return;

        if (movingLeft)
        {
            if (enemy.position.x > leftEdge.position.x)
                MoveInDirection(-1);
            else
                StartIdle();
        }
        else
        {
            if (enemy.position.x < rightEdge.position.x)
                MoveInDirection(1);
            else
                StartIdle();
        }
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
    }

    private void MoveInDirection(int direction)
    {
        anim.SetBool("moving", true);
        idleTimer = 0;

        // Move the enemy sprite
        enemy.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // Flip the sprite visually
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);
    }
}
