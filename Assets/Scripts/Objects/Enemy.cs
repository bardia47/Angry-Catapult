using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float health;

    public float Health
    {
        get { return health; }
        set {
            health = Math.Clamp(value, 0, maxHealth);
            HandleHealth();
        }
    }

    public float maxHealth = 100;
    public float killRotation = 20f;
    public bool canDamage = false;
    public bool isDead = false;
    public bool isMoving = false;
    public int points;
    private Rigidbody2D rb;
    public Animator anim;

    private void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        Health = maxHealth;
    }
    private void Update()
    {

        if (rb.velocity.magnitude <= 0f)
            canDamage = true;
        if (canDamage)
            if (MathF.Abs(this.transform.rotation.eulerAngles.z) > killRotation &&
                MathF.Abs(this.transform.rotation.eulerAngles.z) < 360 - killRotation)
            {
                TakeDamage((20f + (this.transform.rotation.eulerAngles.z * 0.1f))*Time.deltaTime);
            }

    }

    public void TakeDamage(float v)
    {
        isMoving = true;
        Health -= v;
        anim.SetTrigger("hurt");

    }

    private void HandleHealth()
    {
        if (isDead)
            return;
        if (health <= 0f)
        {
            isDead = true;
            UIManager.instance.ShowEnemyScore(this.GetComponentInChildren<EnemyHeartBox>().transform,points);

            health = 0;
            AudioManager.instance.CreateAndPlaySound(SoundClips.POOF, null, 0.3f, 1f);
            ScoreManager.IncreasePoint(points);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            Quaternion rotation = transform.rotation;
            rotation.z = 0f;
            transform.rotation = rotation;
            anim.SetTrigger("dead");
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Cannonball"))
        {
            DustManager.CreateDust(other.collider.ClosestPoint(this.transform.position));
            Health -= maxHealth;
        }
    }
}
