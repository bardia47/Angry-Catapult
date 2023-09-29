using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
    public void OnAnimComplete() {
        DustManager.instance.Recycle(this.transform.parent.gameObject);
    }
}
