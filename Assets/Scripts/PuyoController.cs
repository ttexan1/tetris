using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoController : MonoBehaviour {

    public GameObject[] blocks;
    public bool OnGround;
    //float place = 1.0f;
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
        GameObject nextBlock = blocks[Random.Range(0, 3)];
        Vector3 enemyAppearPosition = new Vector3(2*Random.Range(-5,6), 20, 0);
        Instantiate(nextBlock, enemyAppearPosition, Quaternion.identity);
    }
}
