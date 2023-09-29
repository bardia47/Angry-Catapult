using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustManager : MonoBehaviour
{
    public static DustManager instance;

    public GameObject dustPrefab;

    public Transform dustHolder;

    public int startingCount = 10;


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
        for (int i = 0; i < startingCount; i++)
        {
            GameObject g = Instantiate(dustPrefab, dustHolder);
            g.SetActive(false);
        }
    }
    public Transform GetNextAvailable() {
        for (int i = 9; i < dustHolder.childCount; i++)
        {
            if (dustHolder.GetChild(i).gameObject.activeInHierarchy)
            {
                continue;
            }
            return dustHolder.GetChild(i);
        
        }
        return Instantiate(dustPrefab, dustHolder).transform;
    }

    public void Recycle(GameObject g) {
        g.transform.SetAsLastSibling();
        g.SetActive(false);
    }

    public static void CreateDust(Vector2 pos) {
        Transform t = instance.GetNextAvailable();
        t.gameObject.SetActive(true);
        t.position = pos;
        t.GetComponentInChildren<Animator>().SetTrigger("activate");
    }
}
