using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LShapeLeftScript : MonoBehaviour
{
    GameManagerScript gameManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = gameObject.GetComponent<GameManagerScript>();
    }

    public int[,] InitialState(int angleState, int startPoint)
    {
        int[,] arg = { };
        return arg;
    }


    public bool CanDrop(int angleState, int[] indexPosition)
    {
        return true;
    }

    public bool CanSlide(int moveWay, int angleState, int[] indexPosition)
    {
        return true;
    }
}
