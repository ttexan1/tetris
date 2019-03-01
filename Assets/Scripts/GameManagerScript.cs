using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {

    float generalTime;
    public int[,] stageArray = new int[15, 10]; //ステージの元となる二次元配列初期値は全て0

    //最初にレンダーするブロック群
    public GameObject groundObj;
    public GameObject blockObj;
    public GameObject transparentObj;

    //ブロックの名前と左上の座標位置,回転状態で空間を決定する。(重要)
    int[] indexPosition;    //ブロックの左上の二次元座標
    string movingBlockName; //ブロックの名前とこの座標位置で空間を決定する。
    int angleState;         //ブロックの回転状態 => 形によってとる値の数が違う

    //ブロックの変数格納
    const string series = "series";
    const string square = "square";
    const string zShapeRight = "zShapeRight";
    const string zShapeLeft = "zShapeLeft";
    const string lShapeRight = "lShapeRight";
    const string lShapeLeft = "lShapeLeft";
    const string convex = "convex";
    bool onUpdatingBoard = true; //見た目のために使用！後で修正

    //格形の動き方や状態をまとめたスクリプトに分ける。
    SeriesScript seriesScript;
    SquareScript squareScript;
    ZShapeRightScript zShapeRightScript;
    ZShapeLeftScript zShapeLeftScript;
    LShapeRightScript lShapeRightScript;
    LShapeLeftScript lShapeLeftScript;
    ConvexScript convexScript;

    // Use this for initialization
    void Start () {
        seriesScript = gameObject.GetComponent<SeriesScript>();
        squareScript = gameObject.GetComponent<SquareScript>();
        zShapeRightScript = gameObject.GetComponent<ZShapeRightScript>();
        zShapeLeftScript = gameObject.GetComponent<ZShapeLeftScript>();
        lShapeRightScript = gameObject.GetComponent<LShapeRightScript>();
        lShapeLeftScript = gameObject.GetComponent<LShapeLeftScript>();
        convexScript = gameObject.GetComponent<ConvexScript>();

        stageArray = StageImage();
        RenderBlock(); 
        InstantiateNextBlock();
    }

	// Update is called once per frame
	void Update () {
        generalTime += Time.deltaTime;
        if (generalTime > 0.2f)
        {
            VerticalMove(indexPosition);
            generalTime = 0;
        }
        BlockMove();
    }

    //キーボード入力への対応
    void BlockMove()
    {
        if (Input.GetKeyDown("left"))
        {
            //Debug.Log("left");
            HorizontalMove(-1);
        }
        if (Input.GetKeyDown("right"))
        {
            //Debug.Log("right");
            HorizontalMove(1);
        }
    }

    void InstantiateNextBlock()
    {
        movingBlockName = series;
        int startPoint = Random.Range(0, 6);
        indexPosition = new int[2] { 0, startPoint};
        int[,] toArgument = { };

        switch (movingBlockName)
        {
            case series:
                angleState = Random.Range(1, 3);
                toArgument = seriesScript.InitialState(angleState, startPoint);
                break;
            case square:
                angleState = 1;
                toArgument = squareScript.InitialState(angleState, startPoint);
                break;
            case zShapeRight:
                toArgument = zShapeRightScript.InitialState(angleState, startPoint);
                break;
            case zShapeLeft:
                toArgument = zShapeLeftScript.InitialState(angleState, startPoint);
                break;
            case lShapeRight:
                toArgument = lShapeRightScript.InitialState(angleState, startPoint);
                break;
            case lShapeLeft:
                toArgument = lShapeLeftScript.InitialState(angleState, startPoint);
                break;
            case convex:
                toArgument = convexScript.InitialState(angleState, startPoint);
                break;
            default:
                break;
        }
        ChangeStatus(toArgument, new int[,] { });
        onUpdatingBoard = true;
    }

    //垂直方向移動
    void VerticalMove(int[] index)
    {
        bool dropped = true;
        if (!onUpdatingBoard)
        {
            return;
        }
        switch (movingBlockName)
        {
            case series:
                dropped = seriesScript.CanDrop(angleState, index);
                break;
            case square:
                dropped = squareScript.CanDrop(angleState, index);
                break;
            case zShapeRight:
                dropped = zShapeRightScript.CanDrop(angleState, index);
                break;
            case zShapeLeft:
                dropped = zShapeLeftScript.CanDrop(angleState, index);
                break;
            case lShapeRight:
                dropped = lShapeRightScript.CanDrop(angleState, index);
                break;
            case lShapeLeft:
                dropped = lShapeLeftScript.CanDrop(angleState, index);
                break;
            case convex:
                dropped = convexScript.CanDrop(angleState, index);
                break;
            default:
                break;
        }
        indexPosition[0]++;
        if (!dropped)
        {
            onUpdatingBoard = false;
            DeleteFilledLine();
            Invoke("InstantiateNextBlock", 0.2f);//DeleteFilledLine()のRender処理のせい => 改善しないと
            //InstantiateNextBlock();
        }
    }

    //水平方向移動
    void HorizontalMove(int moveWay)
    {
        bool slided = true;
        switch (movingBlockName)
        {
            case series:
                slided = seriesScript.CanSlide(moveWay, angleState, indexPosition);
                break;
            case square:
                slided = squareScript.CanSlide(moveWay, angleState, indexPosition);
                break;
            case zShapeRight:
                slided = zShapeRightScript.CanSlide(moveWay, angleState, indexPosition);
                break;
            case zShapeLeft:
                slided = zShapeLeftScript.CanSlide(moveWay, angleState, indexPosition);
                break;
            case lShapeRight:
                slided = lShapeRightScript.CanSlide(moveWay, angleState, indexPosition);
                break;
            case lShapeLeft:
                slided = lShapeLeftScript.CanSlide(moveWay, angleState, indexPosition);
                break;
            case convex:
                slided = convexScript.CanSlide(moveWay, angleState, indexPosition);
                break;
            default:
                break;
        }
        if (slided)
        {
            indexPosition[1] += moveWay;
        }
    }

    //データと見た目を更新
    public void ChangeStatus(int[,] toAddress, int[,] fromAddress)
    {
        GameObject target;
        for (int i = 0; i < toAddress.GetLength(0); i++)
        {
            stageArray[toAddress[i, 0], toAddress[i, 1]] = 1;
            target = GameObject.Find("Block" + toAddress[i, 0].ToString() + toAddress[i, 1].ToString() + "(Clone)");
            target.GetComponent<Renderer>().material.color = Color.blue;
        }

        for (int i = 0; i < fromAddress.GetLength(0); i++)
        {
            stageArray[fromAddress[i, 0], fromAddress[i, 1]] = 0;
            target = GameObject.Find("Block" + fromAddress[i, 0].ToString() + fromAddress[i, 1].ToString() + "(Clone)");
            target.GetComponent<Renderer>().material.color = Color.white;
        }
    }


    //配列の初期状態
    int[,] StageImage()
    {
        int[,] intImage = new int[,] {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			//ここよ上が新しいブロックの出現位置になる。 
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            //{ -1,-1,-1,-1,-1,-1,-1,-1,-1,-1}
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
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

    //初期値のRender
    void RenderBlock()
    {
        for (int i = 0; i < stageArray.GetLength(0); i++)
        {
            for (int j = 0; j < stageArray.GetLength(1); j++)
            {
                Vector3 blockPosition = new Vector3(j, i, 0);
                GameObject nextBlock = transparentObj;
                if (stageArray[i, j] == -1 || i == 14)
                {
                    nextBlock = groundObj;
                }
                else if (stageArray[i, j] == 1)
                {
                    nextBlock = blockObj;
                }
                nextBlock.name = "Block" + i.ToString() + j.ToString();
                Instantiate(nextBlock, blockPosition, Quaternion.identity);
            }
        }
    }

    //即席で作ったのであまりよくない
    void DeleteFilledLine()
    {
        var map = new Dictionary<int, bool>();
        for (int i = stageArray.GetLength(0)-2; i >= 0; i--)
        {
            bool isFilled = true;
            for (int j = 0; j < stageArray.GetLength(1); j++)
            {
                if (stageArray[i,j] != 1)
                {
                    isFilled = false;
                    break;
                }
            }
            map.Add(i, isFilled);
        }
        int deletingLineCount = 0;
        var deleteList = new Dictionary<int, int>();
        for (int i = stageArray.GetLength(0)-2; i >= 0; i--)
        {
            if (map[i])
            {
                int[,] fromValue = 
                {
                    { i, 0 }, { i, 1 }, { i, 2 },
                    { i, 3 }, { i, 4 }, { i, 5 },
                    { i, 6 }, { i, 7 }, { i, 8 },
                };
                ChangeStatus(new int[,]{ }, fromValue);
                deletingLineCount++;
                deleteList.Add(i, 0);
            }
            else
            {
                deleteList.Add(i, deletingLineCount);
            }
        }
        for (int i = stageArray.GetLength(0)-2; i >= 0; i--)
        {
            if (deleteList[i] != 0)
            {
                for (int j = 0; j < stageArray.GetLength(1); j++)
                {
                    //ポインタでうまくいかない可能性あり。
                    stageArray[i + deleteList[i], j] = stageArray[i, j];
                }

            }
        }
        for (int i = 0; i < stageArray.GetLength(0); i++)
        {
            for (int j = 0; j < stageArray.GetLength(1); j++)
            {
                Destroy(GameObject.Find("Block" + i.ToString() + j.ToString() + "(Clone)"));
            }
        }
        RenderBlock();
    }
}
