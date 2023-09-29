using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    public void OnEventDead() {
        Destroy(this.transform.parent.gameObject);
    }
}
