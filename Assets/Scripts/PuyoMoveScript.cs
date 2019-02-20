using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoMoveScript : MonoBehaviour {

    public GameObject[] puyo;
    float PuyoCreateTime;
    bool OnGround;
    PuyoController script;
    int[,] puyoArray = new int[5, 10];

    // Use this for initialization
    void Start () {
        script = GameObject.Find("PuyoManager").GetComponent<PuyoController>();
        puyoArray[0, 1] = 2;
    }
	
	// Update is called once per frame
	void Update () {
        PuyoCreateTime += Time.deltaTime;

        if (PuyoCreateTime > 0.3f)
        {
            DropPuyo();
            PuyoCreateTime = 0;
        }

        if (!OnGround)
        {
            MovePuyo();
        }
    }
    void DropPuyo()
    {
        if (!OnGround)
        {
            this.transform.position -= new Vector3(0, 1, 0);
        }
    }
    void MovePuyo()
    {
        if (Input.GetKeyDown("left"))
        {
            this.transform.position += new Vector3(-2, 0, 0);
        }
        if (Input.GetKeyDown("right"))
        {
            this.transform.position += new Vector3(2, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground" && this.gameObject.tag == "movePuyo")
        {
            OnGround = true;
            script.OnGround = true;
            this.gameObject.tag = "stayPuyo";
            JudgePuyo();
            //Debug.Log("from puyo:"+script.OnGround);
        }
    }
    void JudgePuyo()
    {
        PuyoIntoArray();

    }
    void PuyoIntoArray()
    {

    }
}
