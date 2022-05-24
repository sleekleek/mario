using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float originalX;
    private float maxOffSet = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;

    private Rigidbody2D enemyBody;

    // Start is called before the first frame update
    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        // Get the starting position
        originalX = transform.position.x;
        computeVelocity();
    }

    void computeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffSet / enemyPatroltime, 0);
    }

    void MoveGomba() {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffSet)
        {
            MoveGomba();
        } else 
        {
            // Change direction
            moveRight *= -1;
            computeVelocity();
            MoveGomba();
        }
    }
}
