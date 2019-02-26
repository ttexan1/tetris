using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoMoveScript : MonoBehaviour {

    float PuyoCreateTime;
    int[,] stageArray = new int[10, 15]; //ステージの元となる二次元配列初期値は全て0
    public GameObject prefabObj;

    //ブロックの名前と左上の座標位置で空間を決定する。
    int[] indexPosition;//ブロックの左上の二次元座標
    string movingBlockName = "";//ブロックの名前とこの座標位置で空間を決定する。

    string yokonaga = "yokonaga";
    string tatenaga = "tatenaga";
    //string mashikaku = "mashikaku";
    //string zshape = "zshape";

    // Use this for initialization
    void Start () {
        StageImage();
        InstantiateNextBlock();
    }
	
	// Update is called once per frame
	void Update () {
        PuyoCreateTime += Time.deltaTime;

        if (PuyoCreateTime > 0.3f)
        {
            DropBlock(indexPosition);
            PuyoCreateTime = 0;
        }
        if (movingBlockName == yokonaga)
        {
            YokonagaBlockMove();
        }
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
        indexPosition[0] ++;
        if (!canDrop)
        {
            InstantiateNextBlock();
        }
    }

    void InstantiateNextBlock()
    {
        movingBlockName = tatenaga;
        indexPosition = new int[] { 0, Random.Range(0, 6)};
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
            ConvertBlock(-1, tatenaga);
        }
        if (Input.GetKeyDown("right"))
        {
            ConvertBlock(1, tatenaga);
        }
    }


    void ConvertBlock(int moveWay, string blockName)
    {
        //bool canMove = false;
        //int indexPosition[0] = 0;
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
                    }
                }

            }
        }
        indexPosition[1] += moveWay;
    }

    void RenderBlock()
    {
        for (int i =0; i<stageArray.GetLength(0); i++)
        {
            for (int j = 0; j < stageArray.GetLength(1); j++)
            {
                if (stageArray[i,j] == 1)
                {
                    Vector3 blockPosition = new Vector3(i,j,0);
                    Instantiate(prefabObj, blockPosition, Quaternion.identity);
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
                    Vector3 blockPosition = new Vector3(i, j, 0);
                    Instantiate(prefabObj, blockPosition, Quaternion.identity);
                }
            }
        }
    }


    void StageImage()
    {
        int[,] intImage = new int[,] {
            { 3, 1, 1, 1, 1, 1, 1, 1, 1, 4 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            //ここより上が新しいブロックの出現位置になる。
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 2 }
        };
        Debug.Log("[0,0]" + intImage[0,0].ToString());//3
        Debug.Log("[0,10]" + intImage[0,10].ToString());//4
        Debug.Log("[14,0]" + intImage[14,0].ToString());//1
        Debug.Log("[14,14]" + intImage[14,14].ToString());//2
    }
}
