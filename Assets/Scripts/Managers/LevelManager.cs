using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public Sprite[] brokenPlanks;

    public Sprite PickBrokenPlantSprite => brokenPlanks[Random.Range(0, brokenPlanks.Length)];

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
        MoveCamera.Move(FindObjectOfType<Enemy>().transform.position + Vector3.up,0.5f,false);
        StartCoroutine(MoveToCatapult());
    }

    private IEnumerator MoveToCatapult()
    {
        yield return new WaitForSeconds(2.5f);
        FindObjectOfType<MoveCamera>().toggleFollow = true;
        FindObjectOfType<Catapult>().allowInput = true;
    }
}
