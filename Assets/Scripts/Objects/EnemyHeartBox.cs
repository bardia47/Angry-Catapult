using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeartBox : MonoBehaviour
{
    private bool canDamage = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canDamage)
        {
            GetComponentInParent<Enemy>().TakeDamage(collision.collider.GetComponent<Rigidbody2D>().velocity.magnitude
                * (collision.transform.rotation.eulerAngles.z * 0.1f));
        }
        canDamage = true;
    }
}
