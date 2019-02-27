using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoMoveScript : MonoBehaviour {

    float generalTime;
    int[,] stageArray = new int[15, 10]; //ステージの元となる二次元配列初期値は全て0
    //最初にレンダーするブロック群
    public GameObject groundObj;
    public GameObject blockObj;
    public GameObject transparentObj;

    //ブロックの名前と左上の座標位置で空間を決定する。(重要)
    int[] indexPosition;//ブロックの左上の二次元座標
    string movingBlockName;//ブロックの名前とこの座標位置で空間を決定する。

    //ブロックの変数格納
    string yokonaga = "yokonaga";
    string tatenaga = "tatenaga";
    string mashikaku = "mashikaku";
    string zshape = "zshape";

    // Use this for initialization
    void Start () {
        stageArray = StageImage();
        RenderBlock();
        InstantiateNextBlock();
    }
	
	// Update is called once per frame
	void Update () {
        generalTime += Time.deltaTime;
        if (generalTime > 0.5f)
        {
            DropBlock(indexPosition);
            generalTime = 0;
        }

        BlockMove();
    }

    //ブロックを落とせるかどうかの判定(関数名変更したい)
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
                int[,] fromArgument = { { index[0], index[1] } };
                int[,] toArgument = { { index[0] + 4, index[1] } };
                ChangeStatus(toArgument, fromArgument);
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
        int startPoint = Random.Range(0, 6);
        indexPosition = new int[2] { 0, startPoint};
        int[,] toArgument = { };

        if (movingBlockName == tatenaga)
        {
            toArgument = new int[,]
            {
                { 0, startPoint }, { 1, startPoint },
                { 2, startPoint }, { 3, startPoint }
            };
        }
        //それぞれの動きを実装
        //if (movingBlockName == yokonaga)
        //{
        //    toArgument = new int[,]
        //    {
        //        { 0, startPoint }, { 1, startPoint },
        //        { 2, startPoint }, { 3, startPoint }
        //    };
        //}
        //if (movingBlockName == mashikaku)
        //{
        //    toArgument = new int[,]
        //    {
        //        { 0, startPoint }, { 1, startPoint },
        //        { 2, startPoint }, { 3, startPoint }
        //    };
        //}
        //if (movingBlockName == zshape)
        //{
        //    toArgument = new int[,]
        //    {
        //        { 0, startPoint }, { 1, startPoint },
        //        { 2, startPoint }, { 3, startPoint }
        //    };
        //}
        ChangeStatus(toArgument, new int[,] { });
    }

    //外部からの入力への対応
    void BlockMove()
    {
        if (Input.GetKeyDown("left"))
        {
            Debug.Log("left");
            ConvertBlock(-1);
        }
        if (Input.GetKeyDown("right"))
        {
            Debug.Log("right");
            ConvertBlock(1);
        }
    }

    //ブロックのデータと見た目を更新
    void ChangeStatus(int[,] toAddress, int[,] fromAddress) 
    {
        GameObject target;
        for (int i=0; i<toAddress.GetLength(0); i++)
        {
            stageArray[toAddress[i, 0], toAddress[i,1]] = 1;
            target = GameObject.Find("Block" + toAddress[i,0].ToString() + toAddress[i,1].ToString() + "(Clone)");
            target.GetComponent<Renderer>().material.color = Color.blue;
        }

        for (int i = 0; i < fromAddress.GetLength(0); i++)
        {
            stageArray[fromAddress[i, 0], fromAddress[i, 1]] = 0;
            target = GameObject.Find("Block" + fromAddress[i, 0].ToString() + fromAddress[i, 1].ToString() + "(Clone)");
            target.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    //ブロックを左右に移動できるかどうかの判定
    void ConvertBlock(int moveWay)
    {
        if (movingBlockName == yokonaga)
        {
            if (moveWay + indexPosition[1] > 0 && moveWay + indexPosition[1]+3 < 10)
            {
                if (moveWay == -1 && stageArray[indexPosition[0], indexPosition[1] -1] != 1)
                {
                    //stageArray[indexPosition[0], indexPosition[1]-1] = 1;
                    //stageArray[indexPosition[0], indexPosition[1] + 3] = 0;
                    int[,] toArgument = { { indexPosition[0], indexPosition[1] - 1 } };
                    int[,] fromArgument = { { indexPosition[0], indexPosition[1] + 3 } };
                    ChangeStatus(toArgument, fromArgument);
                }
                if (moveWay == 1 && stageArray[indexPosition[0], indexPosition[1] + 4] != 1)
                {
                    //stageArray[indexPosition[0], indexPosition[1] + 4] = 1;
                    //stageArray[indexPosition[0], indexPosition[1]] = 0;
                    int[,] toArgument = { { indexPosition[0], indexPosition[1] + 4 } };
                    int[,] fromArgument = { { indexPosition[0], indexPosition[1] } };
                    ChangeStatus(toArgument, fromArgument);
                }
            }
        }

        if (movingBlockName == tatenaga)
        {
            if (moveWay + indexPosition[1] >= 0 && moveWay + indexPosition[1] < 10)
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
                        int[,] toArgument = { { indexPosition[0] + i, moveWay + indexPosition[1] } };
                        int[,] fromArgument = { { indexPosition[0] + i, indexPosition[1] } };
                        ChangeStatus(toArgument,fromArgument);
                    }
                    indexPosition[1] += moveWay;
                }
            }
        }
    }

    //ブロックの初期状態
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
        //Debug.Log(stageArray.GetLength(0));
        //Debug.Log(stageArray.GetLength(1));
        return intImage;
    }

    //初期値のレンダー
    void RenderBlock()
    {
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
            }
        }
    }

}
