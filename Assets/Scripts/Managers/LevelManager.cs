using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public Sprite[] brokenPlanks;

    public Sprite PickBrokenPlantSprite => brokenPlanks[Random.Range(0, brokenPlanks.Length)];
    public int TotalEnemiesLeft => FindObjectsOfType<Enemy>().Length;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

    }

    private void Start()
    {
        UIManager.instance.ToggleCatapultBars();
        MoveCamera.Move(FindObjectOfType<Enemy>().transform.position + Vector3.up,0.5f,false);
        StartCoroutine(MoveToCatapult());
    }

    private IEnumerator MoveToCatapult()
    {
        yield return new WaitForSeconds(2.5f);
        FindObjectOfType<MoveCamera>().toggleFollow = true;
        FindObjectOfType<Catapult>().allowInput = true;
        UIManager.instance.ToggleCatapultBars();

    }

    public bool CheckForVictory
    {
        get {
            if (
        TotalEnemiesLeft == 0)
                return true;
            if (TotalEnemiesLeft > 0)
            {
                Enemy[] e = FindObjectsOfType<Enemy>();
                for (int i = 0; i < e.Length; i++)
                {
                    if (e[i].Health > 0f)
                        return false;
                }
                return true;
            }
            return false;
        }
    }
}
