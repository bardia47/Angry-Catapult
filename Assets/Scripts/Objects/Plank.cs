using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plank : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sr;
    public int points = 100;
    public int health = 1;
    public bool damageable = false;
    public BoxCollider2D box;
    public CapsuleCollider2D capsule;
    public float breakSoundPitch = 1f;
    public float breakSoundVolume= .5f;

    /*  private void Start()
      {
          sr = GetComponentInChildren<SpriteRenderer>();
      }*/
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (health == 0 || !damageable)
        {
            if (other.collider.CompareTag("Cannonball"))
            {

                    AudioManager.instance.CreateAndPlaySound(SoundClips.THUD, null, 0.3f, 1f);
                DustManager.CreateDust(other.collider.ClosestPoint(this.transform.position));
            }

                return; 
        }
        if (other.collider.CompareTag("Cannonball"))
        {
            AudioManager.instance.CreateAndPlaySound(SoundClips.THUD, null, 0.3f, 1f);

            DustManager.CreateDust(other.collider.ClosestPoint(this.transform.position));
            
            health--;
            Break();
        }
        else if (other.collider.CompareTag("Plank"))
        {
            if (other.collider.GetComponent<Rigidbody2D>().velocity.magnitude > 0.5f)
            {
                DustManager.CreateDust(other.collider.ClosestPoint(this.transform.position));
                health--;
                Break();
            }
        }
    }

    private void Break()
    {

        AudioManager.instance.CreateAndPlaySound(SoundClips.WOODBREAK, null, breakSoundVolume, breakSoundPitch);

        sr.sprite = LevelManager.instance.PickBrokenPlantSprite;
        box.enabled = false;
        capsule.enabled = true;
        ScoreManager.IncreasePoint(points);
    }
}
