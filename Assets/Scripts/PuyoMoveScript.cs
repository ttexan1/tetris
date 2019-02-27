using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoMoveScript : MonoBehaviour {

    float PuyoCreateTime;
    int[,] stageArray = new int[15, 10]; //ステージの元となる二次元配列初期値は全て0
    GameObject[,] stageObjects = new GameObject[15, 10];
    public GameObject groundObj;
    public GameObject blockObj;
    public GameObject transparentObj;


    //ブロックの名前と左上の座標位置で空間を決定する。
    int[] indexPosition;//ブロックの左上の二次元座標
    string movingBlockName = "";//ブロックの名前とこの座標位置で空間を決定する。

    string yokonaga = "yokonaga";
    string tatenaga = "tatenaga";
    //string mashikaku = "mashikaku";
    //string zshape = "zshape";

    // Use this for initialization
    void Start () {
        stageArray = StageImage();
        RenderBlock();
        InstantiateNextBlock();
    }
	
	// Update is called once per frame
	void Update () {
        PuyoCreateTime += Time.deltaTime;

        if (PuyoCreateTime > 0.5f)
        {
            //Debug.Log("ブロック一つ目");
            DropBlock(indexPosition);
            PuyoCreateTime = 0;
        }
        //if (movingBlockName == yokonaga)
        //{
        //    YokonagaBlockMove();
        //}
        if (movingBlockName == tatenaga)
        {
            TatenagaBlockMove();
        }

    }
    void DropBlock(int[] index)
    {
        bool canDrop = true;
        if (movingBlockName == yokonaga)
        {
            for (int i = 0; i < 4; i++)
            {
                if (stageArray[index[0] + 1, index[1] + i] == 1)
                {
                    canDrop = false;
                    break;
                }
            }
            if (canDrop)
            {
                for (int i = 0; i < 4; i++)
                {
                    stageArray[index[0] + 1, index[1] + i] = 1;
                    stageArray[index[0], index[1] + i] = 0;
                }
            }
        }

        if (movingBlockName == tatenaga)
        {

            if (stageArray[index[0] + 4, index[1]] == 1)
            {
                canDrop = false;
            }
            
            if (canDrop)
            {
                stageArray[index[0], index[1]] = 0;
                stageArray[index[0]+4, index[1]] = 1;
                GameObject target = GameObject.Find("Block" + index[0].ToString() + index[1].ToString() + "(Clone)");
                target.GetComponent<Renderer>().material.color = Color.white;
                target = GameObject.Find("Block" + (index[0]+4).ToString() + index[1].ToString()+ "(Clone)");
                target.GetComponent<Renderer>().material.color = Color.blue;
            }
        }
        indexPosition[0] ++;
        if (!canDrop)
        {
            InstantiateNextBlock();
        }
    }

    void InstantiateNextBlock()
    {
        movingBlockName = tatenaga;
        int aaaaaaa = Random.Range(0, 6);
        indexPosition = new int[2] { 0, aaaaaaa};

        if (movingBlockName == tatenaga) {
            GameObject target = GameObject.Find("Block0" + aaaaaaa.ToString() + "(Clone)");
            target.GetComponent<Renderer>().material.color = Color.blue;
            target = GameObject.Find("Block1" + aaaaaaa.ToString() + "(Clone)");
            target.GetComponent<Renderer>().material.color = Color.blue;
            target = GameObject.Find("Block2" + aaaaaaa.ToString() + "(Clone)");
            target.GetComponent<Renderer>().material.color = Color.blue;
            target = GameObject.Find("Block3" + aaaaaaa.ToString() + "(Clone)");
            target.GetComponent<Renderer>().material.color = Color.blue;
        }

    }

    void YokonagaBlockMove()
    {
        if (Input.GetKeyDown("left"))
        {
            ConvertBlock(-1, yokonaga);
        }
        if (Input.GetKeyDown("right"))
        {
            ConvertBlock(1, yokonaga);
        }
    }

    void TatenagaBlockMove()
    {
        if (Input.GetKeyDown("left"))
        {
            Debug.Log("left");
            ConvertBlock(-1, tatenaga);
        }
        if (Input.GetKeyDown("right"))
        {
            Debug.Log("right");
            ConvertBlock(1, tatenaga);
        }
    }
    void ChangeColor() 
    {

    }


    void ConvertBlock(int moveWay, string blockName)
    {

        if ( blockName == yokonaga)
        {
            if (moveWay + indexPosition[1] > 0 && moveWay + indexPosition[1]+3 < 10)
            {
                if (moveWay == -1 && stageArray[indexPosition[0], indexPosition[1] -1] != 1)
                {
                    stageArray[indexPosition[0], indexPosition[1]-1] = 1;
                    stageArray[indexPosition[0], indexPosition[1] + 3] = 0;
                }
                if (moveWay == 1 && stageArray[indexPosition[0], indexPosition[1] + 4] != 1)
                {
                    stageArray[indexPosition[0], indexPosition[1]] = 0;
                    stageArray[indexPosition[0], indexPosition[1] + 4] = 1;
                }
            }
        }

        if (blockName == tatenaga)
        {
            //Debug.Log(moveWay.ToString() + indexPosition[1].ToString() + "次のインデックス");
            //Debug.Log(indexPosition[0]);
            if (moveWay + indexPosition[1] > 0 && moveWay + indexPosition[1] < 10)
            {
                bool canMove = true;
                for (int i = 0; i < 4; i++)
                {
                    if (stageArray[indexPosition[0] + i, moveWay + indexPosition[1]] == 1)
                    {
                        canMove = false;
                        break;
                    }
                }
                if (canMove)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        stageArray[indexPosition[0] + i, moveWay + indexPosition[1]] = 1;
                        stageArray[indexPosition[0] + i, indexPosition[1]] = 0;
                        //Debug.Log((indexPosition[0] + i).ToString()+ indexPosition[1].ToString());
                        GameObject target = GameObject.Find("Block" + (indexPosition[0] + i).ToString() + (moveWay + indexPosition[1]).ToString() + "(Clone)");
                        target.GetComponent<Renderer>().material.color = Color.blue;
                        target = GameObject.Find("Block" + (indexPosition[0] + i).ToString() + indexPosition[1].ToString() + "(Clone)");
                        target.GetComponent<Renderer>().material.color = Color.white;
                    }
                    indexPosition[1] += moveWay;
                }

            }
        }
    }


    void DestroyBlock()
    {
        for (int i = 0; i < stageArray.GetLength(0); i++)
        {
            for (int j = 0; j < stageArray.GetLength(1); j++)
            {
                if (stageArray[i, j] == 1)
                {

                }
            }
        }
    }


    int[,] StageImage()
    {
        int[,] intImage = new int[,] {
            //ここよ0上が新しいブロックの出現位置になる。
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			  
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            //{ -1,-1,-1,-1,-1,-1,-1,-1,-1,- 1}
            { 1,1,1,1,1,1,1,1,1,1}
        };
        //Debug.Log("[0,0]" + intImage[0,0].ToString());//左上
        //Debug.Log("[0,9]" + intImage[0,9].ToString());//右上
        //Debug.Log("[14,0]" + intImage[14,0].ToString());//左下
        //Debug.Log("[14,9]" + intImage[14,9].ToString());//右下
        //Debug.Log(intImage.GetLength(0));
        //Debug.Log(intImage.GetLength(1));
        return intImage;
    }
    void RenderBlock()
    {
        //Debug.Log(stageObjects.GetLength(0));
        //Debug.Log(stageObjects.GetLength(1));
        //Debug.Log(stageArray.GetLength(0));
        //Debug.Log(stageArray.GetLength(1));

        for (int i = 0; i < stageArray.GetLength(0); i++)
        {
            for (int j = 0; j < stageArray.GetLength(1); j++)
            {
                Vector3 blockPosition = new Vector3(j, i, 0);
                GameObject nextBlock = transparentObj;
                if (stageArray[i, j] == -1)
                {
                    nextBlock = groundObj;
                }
                if (stageArray[i, j] == 1)
                {
                    nextBlock = blockObj;
                }
                nextBlock.name = "Block" + i.ToString() + j.ToString();
                Instantiate(nextBlock, blockPosition, Quaternion.identity);
                stageObjects[i, j] = nextBlock;
            }
        }
    }

}
