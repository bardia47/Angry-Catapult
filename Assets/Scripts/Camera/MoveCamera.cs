using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MoveCamera : MonoBehaviour
{
    public float speed = 15f;
    public float minDistance;

    public float endingOrthoSize;

    public GameObject target;


    public Vector3 offset;
    public Vector3 targetPos;

    public bool toggleFollow = false;

    private void Start()
    {
        targetPos = transform.position;
    }

    private void Update()
    {
        if (target && toggleFollow)
        {
            Vector3 posNoZ = transform.position + offset;
            Vector3 targetDirection = target.transform.position - posNoZ;
            float interpVelocity = targetDirection.magnitude * speed;
            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

            targetPos.z = -10f;
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.25f);
        }
    }
    public static void Move(Vector3 pos, float time, bool lockToTarget)
    {
        MoveCamera c = FindObjectOfType<MoveCamera>();
        c.toggleFollow = false;
        pos.z = Camera.main.transform.position.z;
        Camera.main.transform.DOMove(pos, time).OnComplete(() =>

         { if (lockToTarget)
                 c.toggleFollow = true;
                 });
            
    }
}

