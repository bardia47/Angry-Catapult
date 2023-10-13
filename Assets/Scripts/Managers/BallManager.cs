using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BallManager : MonoBehaviour
{


    public static BallManager instance;

    public GameObject ballPrefab;
    public Transform catapultFireSlot;
    public Catapult catapult;

    public int ballsLeft = 3;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        SceneManager.sceneLoaded += OnLoadScene;
    }

    private void Start()
    {

        catapult = FindObjectOfType<Catapult>();
        SpawnBall();
    }

    private void OnLoadScene(Scene scene, LoadSceneMode arg1)
    {
        ballsLeft = 3;

    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLoadScene;
    }
    public void SpawnBall() {

        Transform t = Instantiate(ballPrefab, catapultFireSlot).transform;
        catapult.cannonball = t;
        FindObjectOfType<MoveCamera>().target = t.gameObject;
        UIManager.instance.UpdateBallsLeft();
    }
}

