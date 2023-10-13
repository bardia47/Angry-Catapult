using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public float moveHorizontal, moveVertical;
    public float offsetHorizontal, offsetVertical;

    private Vector2 mousePos;

    private void Update()
    {
        mousePos = Input.mousePosition;
    }
    private void LateUpdate()
    {
        Vector3 pos = Camera.main.transform.position;
        pos.x = mousePos.normalized.x * moveHorizontal + offsetHorizontal;
        pos.y = mousePos.normalized.y * moveVertical + offsetVertical;
        Camera.main.transform.position = pos;
    }
}
