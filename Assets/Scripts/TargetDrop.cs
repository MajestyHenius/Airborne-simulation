using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TargetDrop : MonoBehaviour
{
    public int switchNum = 0;
    //public GameObject T;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void ConsoleResult(int value)
    {
        //这里用 if else if也可，看自己喜欢
        //分别对应：第一项、第二项....以此类推
        switch (value)
        {
            case 0:
                //print("第1页");
                switchNum = 0;
                break;
            case 1:
                //print("第2页");
                switchNum = 1;
                break;
            case 2:
                //print("第3页");
                switchNum = 2;
                break;

        }
    }
}





/// <summary>
/// Chinar例子脚本，用以输出
/// </summary>
