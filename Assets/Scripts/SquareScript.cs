using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareScript : MonoBehaviour
{
    GameManagerScript gameManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = gameObject.GetComponent<GameManagerScript>();
    }

    public int[,] InitialState(int angleState, int startPoint)
    {
        int[,] arg = { 
            { 0, startPoint }, { 0, startPoint + 1 },
            { 1, startPoint }, { 1, startPoint + 1 }
        };
        return arg;
    }


    public bool CanDrop(int angleState, int[] basePos)
    {
        if (gameManagerScript.stageArray[basePos[0] + 2, basePos[1]] == 1 ||
            gameManagerScript.stageArray[basePos[0] + 2, basePos[1] + 1] == 1)
        {
            return false;
        }
        int[,] toArgument = { { basePos[0] + 2, basePos[1] }, { basePos[0] + 2, basePos[1] + 1 } };
        int[,] fromArgument = { { basePos[0], basePos[1] }, { basePos[0], basePos[1] + 1 } };
        gameManagerScript.ChangeStatus(toArgument, fromArgument);
        return true;
    }

    public bool CanSlide(int moveWay, int angleState, int[] basePos)
    {
        if (moveWay == 1)
        {
            if (moveWay + basePos[1] + 1 >= 10)
            {
                return false;
            }
            if (gameManagerScript.stageArray[basePos[0], moveWay + basePos[1] + 1] == 1 ||
                gameManagerScript.stageArray[basePos[0] + 1, moveWay + basePos[1] + 1] == 1)
            {
                return false;
            }
            int[,] toArgument = { 
                { basePos[0], moveWay+ basePos[1] + 1 },
                { basePos[0] + 1, moveWay + basePos[1] + 1 }
            };
            int[,] fromArgument = { 
                { basePos[0], basePos[1] },
                { basePos[0] + 1, basePos[1] }
            };
            gameManagerScript.ChangeStatus(toArgument, fromArgument);
        }

        if (moveWay == -1)
        {
            if (moveWay + basePos[1] < 0)
            {
                return false;
            }
            if (gameManagerScript.stageArray[basePos[0], moveWay + basePos[1]] == 1 ||
                gameManagerScript.stageArray[basePos[0] + 1, moveWay + basePos[1]] == 1)
            {
                return false;
            }
            int[,] toArgument = {
                { basePos[0], moveWay+ basePos[1] },
                { basePos[0] + 1, moveWay + basePos[1] }
            };
            int[,] fromArgument = {
                { basePos[0], basePos[1] + 1 },
                { basePos[0] + 1, basePos[1] + 1}
            }; 
            gameManagerScript.ChangeStatus(toArgument, fromArgument);
        }
        return true;
    }
}
