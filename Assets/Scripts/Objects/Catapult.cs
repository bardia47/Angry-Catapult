using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Catapult : MonoBehaviour
{
    public Rigidbody2D armRb;
    public Transform cannonball;

    private Quaternion initArmRotation;
    public float forcePower;
    public int rotateSpeed;
    public float minForce;
    public float maxForce;
    //  public float forceScalar;

    public float maxRotate;
    public float minRotate;
    public float CurrentRotate;



    public bool allowInput = true;
    public bool launched = false;


    private void Update()
    {

        if (GameManager.instance.currentState == GameStates.END ||
            GameManager.instance.currentState == GameStates.PAUSE ||
            GameManager.instance.currentState == GameStates.MAINMENU)
            return;

       /* if (launched && armRb.angularVelocity > angVel)
        {
            angVel = armRb.angularVelocity;
        }*/

        if (launched || !allowInput)
            return;


        if (Input.GetButtonDown("Fire1"))
        {
            LaunchCatapult();
        }

      
    }


    private void FixedUpdate()
    {
        if (launched)
        {

            if (Mathf.Rad2Deg * armRb.transform.localRotation.z < CurrentRotate)
            {
                //armRb.GetComponent<Collider2D>().enabled = false;
                cannonball.SetParent(null, true);
                 float forceScalar =(1  - initArmRotation.z + armRb.transform.rotation.z) * 2;
                cannonball.GetComponent<Rigidbody2D>().AddForce(cannonball.up * forceScalar * forcePower, ForceMode2D.Impulse);
              
                cannonball.GetComponent<CannonBall>().launched = true;
                launched = false;
            }
            armRb.transform.Rotate(-Vector3.forward, rotateSpeed);
        }
    }


    public void LaunchCatapult()
    {


        if (GameManager.instance.currentState == GameStates.END ||
            GameManager.instance.currentState == GameStates.PAUSE ||
            GameManager.instance.currentState == GameStates.MAINMENU)
            return;


        if (launched || !allowInput)
            return;
        AudioManager.instance.CreateAndPlaySound(SoundClips.CATAPULT, this.transform.position, 1f, 1f, false);
        Plank[] planks = FindObjectsOfType<Plank>();
        foreach (Plank p in planks)
        {
            p.damageable = true;
        }
        allowInput = false;
        launched = true;

        UIManager.instance.ToggleCatapultBars();

        Camera.main.DOOrthoSize(FindObjectOfType<MoveCamera>().endingOrthoSize, 2.2f).SetDelay(0.7f);
    }


    private void Start()
    {
        CurrentRotate = (minRotate + maxRotate) / 2;
        forcePower = (minForce + maxForce) / 2;
        initArmRotation = armRb.transform.rotation;
        //  initArmTransform = armRb.transform.position;
        UIManager.instance.angleSlider.minValue = maxRotate;
        UIManager.instance.angleSlider.maxValue = minRotate;
        UIManager.instance.angleSlider.value = (minRotate + maxRotate) / 2;
        UIManager.instance.UpdateAngle();

        UIManager.instance.powerSlider.minValue = minForce;
        UIManager.instance.powerSlider.maxValue = maxForce;
        UIManager.instance.powerSlider.value = (minForce + maxForce) / 2;
        UIManager.instance.UpdatePower();


    }

    public void ResetCatapult()
    {
        armRb.transform.rotation = initArmRotation;
        armRb.angularVelocity = 0f;

       // armRb.GetComponent<Collider2D>().enabled = true;
        allowInput = true;
        Camera.main.DOOrthoSize(2.5f, 0.5f);
        UIManager.instance.ToggleCatapultBars();

    }
}
