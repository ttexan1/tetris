using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LShapeRightScript : MonoBehaviour
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
        //基本のL型の左上が常にindexPositionとなリマス。回転しても変えない予定です。
        if (angleState == 1)//基本のL型
        {
            arg = new int[,] {
                { 0, startPoint }, { 1, startPoint },
                { 2, startPoint }, { 2, startPoint + 1}
            };
        }
        else if (angleState == 2) // Lを右に90度回転
        {
            arg = new int[,] {
                { 0, startPoint }, { 0, startPoint - 1 },
                { 0, startPoint - 2 }, { 1, startPoint - 2 }
            };
        }
        else if (angleState == 3) // Lの上下逆
        {
            arg = new int[,] {
                { 2, startPoint }, { 1, startPoint },
                { 0, startPoint }, { 1, startPoint - 1 }
            };
        }
        else //(angleState == 4) Lを左に90度回転
        {
            arg = new int[,] {
                { 1, startPoint }, { 1, startPoint + 1 },
                { 1, startPoint + 2 }, { 0, startPoint +2 }
            };
        }
        return arg;
    }


    public bool CanDrop(int angleState, int[] basePos)
    {
        if (angleState == 1)//L型
        {
            Debug.Log((basePos[0] + 3).ToString() + basePos[1].ToString());
            if (gameManagerScript.stageArray[basePos[0] + 3, basePos[1]] == 1 ||
                gameManagerScript.stageArray[basePos[0] + 3, basePos[1] + 1] == 1)
            {
                return false;
            }
            int[,] fromArgument = { { basePos[0], basePos[1] }, { basePos[0] + 2, basePos[1] + 1 } };
            int[,] toArgument = { { basePos[0] + 3, basePos[1] }, { basePos[0] + 3, basePos[1] + 1 } };
            gameManagerScript.ChangeStatus(toArgument, fromArgument);
        }

        if (angleState == 2)//右90度
        {
            if (gameManagerScript.stageArray[basePos[0] + 1, basePos[1]] == 1 ||
               gameManagerScript.stageArray[basePos[0] + 1, basePos[1] - 1] == 1 ||
                gameManagerScript.stageArray[basePos[0] + 2, basePos[1] - 2] == 1)
            {
                return false;
            }

            int[,] fromArgument = { 
                { basePos[0], basePos[1] },
                { basePos[0], basePos[1] - 1 },
                { basePos[0], basePos[1] - 2 },
            };
            int[,] toArgument = { 
                { basePos[0] + 1, basePos[1] },
                { basePos[0] + 1, basePos[1] - 1 },
                { basePos[0] + 2, basePos[1] - 2 }
            };
            gameManagerScript.ChangeStatus(toArgument, fromArgument);
        }
        return true;
    }

    public bool CanSlide(int moveWay, int angleState, int[] basePos)
    {
        if (angleState == 1)
        {
            if (moveWay == 1) {
                if (moveWay + basePos[1] + 1 >= 10)
                {
                    return false;
                }
                if (gameManagerScript.stageArray[basePos[0], moveWay + basePos[1]] == 1 ||
                    gameManagerScript.stageArray[basePos[0] + 1, moveWay + basePos[1]] == 1 ||
                    gameManagerScript.stageArray[basePos[0] + 2, moveWay + basePos[1] + 1] == 1)
                {
                    return false;
                }
                int[,] toArgument = {
                        { basePos[0], moveWay + basePos[1] },
                        { basePos[0] + 1, moveWay + basePos[1] },
                        { basePos[0] + 2, moveWay + basePos[1] + 1 }
                    };
                int[,] fromArgument = {
                        { basePos[0], basePos[1] },
                        { basePos[0] + 1, basePos[1] },
                        { basePos[0] + 2, basePos[1] }
                    };
                gameManagerScript.ChangeStatus(toArgument, fromArgument);
            } 
            else if ( moveWay == -1)
            {
                if (moveWay + basePos[1] < 0)
                {
                    return false;
                }
                for (int i = 0; i < 3; i++)
                {
                    if (gameManagerScript.stageArray[basePos[0] + i, moveWay + basePos[1]] == 1)
                    {
                        return false;
                    }
                }
                int[,] toArgument = {
                        { basePos[0], moveWay + basePos[1] },
                        { basePos[0] + 1, moveWay + basePos[1] },
                        { basePos[0] + 2, moveWay + basePos[1] }
                    };
                int[,] fromArgument = {
                        { basePos[0], basePos[1] },
                        { basePos[0] + 1, basePos[1] },
                        { basePos[0] + 2, basePos[1] + 1}
                    };
                gameManagerScript.ChangeStatus(toArgument, fromArgument);
            }
        }

        if (angleState == 2)
        {
            if (moveWay == 1)
            {
                if (moveWay + basePos[1] >= 10)
                {
                    return false;
                }
                if (gameManagerScript.stageArray[basePos[0], moveWay + basePos[1]] == 1 ||
                    gameManagerScript.stageArray[basePos[0] + 1, moveWay + basePos[1] - 2] == 1)
                {
                    return false;
                }
                int[,] toArgument = {
                        { basePos[0], moveWay + basePos[1] },
                        { basePos[0] + 1, moveWay + basePos[1] - 2 }
                    };
                int[,] fromArgument = {
                        { basePos[0], basePos[1] - 2 },
                        { basePos[0] + 1, basePos[1] - 2}
                    };
                gameManagerScript.ChangeStatus(toArgument, fromArgument);
            }
            else if (moveWay == -1)
            {
                if (moveWay + basePos[1] -2 < 0)
                {
                    return false;
                }
                if (gameManagerScript.stageArray[basePos[0], moveWay + basePos[1] - 2] == 1 ||
                    gameManagerScript.stageArray[basePos[0] + 1, moveWay + basePos[1] - 2] == 1)
                {
                    return false;
                }
                int[,] toArgument = {
                        { basePos[0], moveWay + basePos[1] - 2 },
                        { basePos[0] + 1, moveWay + basePos[1] - 2 }
                    };
                int[,] fromArgument = {
                        { basePos[0], basePos[1] },
                        { basePos[0] + 1, basePos[1] -2}
                    };
                gameManagerScript.ChangeStatus(toArgument, fromArgument);
            }
        }
        return true;
    }
}
