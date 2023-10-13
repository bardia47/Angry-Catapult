using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Catapult : MonoBehaviour
{
    public Rigidbody2D armRb;
    public Transform cannonball;

    public Quaternion initArmRotation;

    public float forcePower;
    public float rotateSpeed;
    public float minForce;
    public float maxForce;
    public float forceScalar;

    public float maxRotate;
    public float minRotate;
    public float CurrentRotate;

    private float angVel = 0f;
    

    public bool allowInput = true;
    public bool launched = false;


    private float horizontal;
    private float vertical;


    private void Update()
    {

        if (GameManager.instance.currentState == GameStates.END ||
            GameManager.instance.currentState == GameStates.PAUSE ||
            GameManager.instance.currentState == GameStates.MAINMENU)
            return;

        if (armRb.angularVelocity > angVel)
        {
            angVel = armRb.angularVelocity;
        }
        if (launched || !allowInput)
            return;

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("Fire1"))
        {
            AudioManager.instance.CreateAndPlaySound(SoundClips.CATAPULT, this.transform.position, 1f, 1f, false);
            Plank[] planks = FindObjectsOfType<Plank>();
            foreach (Plank p in planks){
                p.damageable = true;
            }
            allowInput = false;
            launched = true;

            UIManager.instance.ToggleCatapultBars();

            Camera.main.DOOrthoSize(FindObjectOfType<MoveCamera>().endingOrthoSize, 2.2f).SetDelay(0.7f);
        }

        if (vertical != 0f) {
            CurrentRotate += vertical * Time.deltaTime;
           // forcePower = (minForce + maxForce) / 2;
            CurrentRotate = Mathf.Clamp(CurrentRotate,maxRotate,minRotate);
            UIManager.instance.angleSlider.value = CurrentRotate;
            UIManager.instance.UpdateAngle();
        }
        if (horizontal != 0f)
        {
            forcePower += horizontal * forceScalar * Time.deltaTime;
            forcePower = Mathf.Clamp(forcePower, minForce, maxForce);
            UIManager.instance.powerSlider.value = forcePower;
            UIManager.instance.UpdatePower();
        }

    }
    private void Start()
    {
        CurrentRotate = (minRotate + maxRotate) / 2;
        forcePower = (minForce + maxForce) / 2;
        initArmRotation = armRb.transform.rotation;

        UIManager.instance.angleSlider.minValue = maxRotate;
        UIManager.instance.angleSlider.maxValue = minRotate;
        UIManager.instance.angleSlider.value= (minRotate + maxRotate) / 2;
        UIManager.instance.UpdateAngle();

        UIManager.instance.powerSlider.minValue = minForce;
        UIManager.instance.powerSlider.maxValue = maxForce;
        UIManager.instance.powerSlider.value = (minForce + maxForce) / 2;
        UIManager.instance.UpdatePower();


    }

    private void FixedUpdate()
    {
        if (launched)
        {
            if (Mathf.Rad2Deg * armRb.transform.localRotation.z < CurrentRotate)
            {
                armRb.GetComponent<Collider2D>().enabled = false;
                cannonball.SetParent(null, true);
                cannonball.GetComponent<Rigidbody2D>().AddForce(cannonball.up * angVel * forcePower);
                cannonball.GetComponent<CannonBall>().launched = true;
                launched = false;
            }
            armRb.transform.Rotate(-Vector3.forward,rotateSpeed);
        }
    }

    public void ResetCatapult() {
        armRb.GetComponent<Collider2D>().enabled = true;
        armRb.transform.rotation = initArmRotation;
        angVel = 0f;
        armRb.angularVelocity = 0f;
        allowInput = true;
        Camera.main.DOOrthoSize(3.5f, 0.5f);
        UIManager.instance.ToggleCatapultBars();

    }
}
