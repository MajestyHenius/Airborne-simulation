using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inputtext : MonoBehaviour
{
    public float got=0f;
    public string gotstr;
    void Start()
    {
        transform.GetComponent<InputField>().onValueChanged.AddListener(Changed_Value);

        transform.GetComponent<InputField>().onEndEdit.AddListener(End_Value);

    }

    public void Changed_Value(string inp)
    {

        //print("正在输入:" + inp);

    }

    public void End_Value(string inp)
    {

        gotstr = inp;
        got = float.Parse(inp.ToString());

    }


    
}
