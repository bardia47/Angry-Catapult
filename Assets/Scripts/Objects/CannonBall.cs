using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    Rigidbody2D rb;
    public bool launched = false;

    public float minVelocity = 0.5f;
    public float resetTimer = 2f;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    private void Update()
    {
        if (BallManager.instance.catapult.cannonball != transform)
            return;

        if (!launched)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            return;
        
        }
        if (rb.velocity.magnitude <= minVelocity)
        {
            if (resetTimer > 0f)
            {

                resetTimer -= Time.deltaTime;
                if (resetTimer <= 0f)
                {

                    BallManager.instance.ballsLeft--;
                    if (BallManager.instance.ballsLeft > 0)
                    {
                        BallManager.instance.catapult.ResetCatapult();
                        BallManager.instance.SpawnBall();
                    }
                }
            }
        
        }
        if (rb.velocity.y < 0f)
        {

            rb.mass += 0.01f;

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground")) {
            DustManager.CreateDust(other.collider.ClosestPoint(this.transform.position));
        }
    }
}
