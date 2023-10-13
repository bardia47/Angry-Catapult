using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    public void OnEventDead() {
        if (LevelManager.instance.TotalEnemiesLeft == 1f && this.transform.parent.GetComponent<Enemy>().Health <=0f)
            if (UIManager.instance.victoryCg.alpha <= 0f)
                UIManager.instance.ToggleEndScreen(ScoreManager.instance.isAboveThresholds);
        Destroy(this.transform.parent.gameObject);
    }
}
