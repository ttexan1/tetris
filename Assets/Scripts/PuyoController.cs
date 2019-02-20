using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoController : MonoBehaviour {

    public GameObject[] puyo;
    public bool OnGround;
    float place = 1.0f;
    float PuyoCreateTime = 0;



    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        PuyoCreateTime += Time.deltaTime;
        if (OnGround)
        {
            CreatePuyo();
            OnGround = false;
        }

    }
    void CreatePuyo()
    {
        Vector3 enemyAppearPosition = new Vector3(2*Random.Range(-2,3), 20, 0);
        Instantiate(puyo[0], enemyAppearPosition, Quaternion.identity);
    }
}
