using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeriesScript : MonoBehaviour
{
    GameManagerScript gameManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = gameObject.GetComponent<GameManagerScript>();
    }

    public int[,] InitialState(int angleState, int startPoint)
    {
        int[,] arg;
        if (angleState == 1)
        {
            arg = new int[,] {
                { 0, startPoint }, { 1, startPoint },
                { 2, startPoint }, { 3, startPoint }
            };
        } else {
            arg = new int[,] {
                { 0, startPoint }, { 0, startPoint +1 },
                { 0, startPoint +2 }, { 0, startPoint +3 }
            };
        }
        return arg;
    }


    public bool CanDrop(int angleState, int[] indexPosition)
    {
        if (angleState == 1)//縦長
        {
            if (gameManagerScript.stageArray[indexPosition[0] + 4, indexPosition[1]] == 1)
            {
                return false;
            }
            int[,] fromArgument = { { indexPosition[0], indexPosition[1] } };
            int[,] toArgument = { { indexPosition[0] + 4, indexPosition[1] } };
            gameManagerScript.ChangeStatus(toArgument, fromArgument);
        }

        if (angleState == 2)//横長
        {
            for (int i = 0; i < 4; i++)
            {
                if (gameManagerScript.stageArray[indexPosition[0] + 1, indexPosition[1] + i] == 1)
                {
                    return false;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                int[,] fromArgument = { { indexPosition[0], indexPosition[1] + i } };
                int[,] toArgument = { { indexPosition[0] + 1, indexPosition[1] + i } };
                gameManagerScript.ChangeStatus(toArgument, fromArgument);
            }
        }
        return true;
    }

    public bool CanSlide(int moveWay, int angleState, int[] indexPosition)
    {
        if (angleState == 2)
        {
            if (moveWay + indexPosition[1] >= 0 && moveWay + indexPosition[1] + 3 < 10)
            {
                if (moveWay == -1 && gameManagerScript.stageArray[indexPosition[0], indexPosition[1] - 1] != 1)
                {
                    int[,] toArgument = { { indexPosition[0], indexPosition[1] - 1 } };
                    int[,] fromArgument = { { indexPosition[0], indexPosition[1] + 3 } };
                    gameManagerScript.ChangeStatus(toArgument, fromArgument);
                    return true;
                }
                if (moveWay == 1 && gameManagerScript.stageArray[indexPosition[0], indexPosition[1] + 4] != 1)
                {
                    int[,] toArgument = { { indexPosition[0], indexPosition[1] + 4 } };
                    int[,] fromArgument = { { indexPosition[0], indexPosition[1] } };
                    gameManagerScript.ChangeStatus(toArgument, fromArgument);
                    return true;
                }
            }
        }

        if (angleState == 1)
        {
            if (moveWay + indexPosition[1] >= 0 && moveWay + indexPosition[1] < 10)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (gameManagerScript.stageArray[indexPosition[0] + i, moveWay + indexPosition[1]] == 1)
                    {
                        return false;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    int[,] toArgument = { { indexPosition[0] + i, moveWay + indexPosition[1] } };
                    int[,] fromArgument = { { indexPosition[0] + i, indexPosition[1] } };
                    gameManagerScript.ChangeStatus(toArgument, fromArgument);
                }
                return true;
            }
        }
        return false;
    }
}
