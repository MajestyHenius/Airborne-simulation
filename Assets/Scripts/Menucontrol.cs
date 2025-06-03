using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menucontrol : MonoBehaviour
{
    public GameObject Switch;
    public GameObject Menu;//本菜单实体
    public GameObject Missle;//获取飞弹实体
    public GameObject Target;//获取目标实体

    public GameObject Target1;//获取目标实体
    public GameObject Target2;//获取目标实体
    public GameObject Target3;//获取目标实体

    public GameObject Speedtxt;
    public GameObject Heighttxt;
    public GameObject startDistancetxt;
    public GameObject Distancetxt;
    public GameObject ElevationAngletxt;
    public GameObject MissleAngletxt;
    public GameObject TargetAngletxt;
    float[] eachAngles = { -22.5f, -19.5f, -16.5f, -13.5f, -10.5f, -7.5f, -4.5f, -1.5f, 1.5f, 4.5f, 7.5f, 10.5f, 13.5f, 16.5f, 19.5f, 22.5f };//每束激光距离垂线的角度
    float[] eachTan = new float[16];
    private float temp;
    Vector3[] eachDirections = new Vector3[16];
    public void startSimulation()
    {   
        Time.timeScale = 1f;//游戏时间比例
        Menu.SetActive(false);//菜单消失
        
    }
    public void confirmParameters()
    {
        //Debug.Log(Switch.GetComponent<TargetDrop>().switchNum);

        switch(Switch.GetComponent<TargetDrop>().switchNum)
        {
            case 0:
                Target = Target1;
                Target.transform.position = new Vector3(0, Target1.transform.position.y , Target1.transform.position.z);
                Target2.transform.position = new Vector3(-20, Target2.transform.position.y, Target2.transform.position.z);
                Target3.transform.position = new Vector3(-20, Target3.transform.position.y, Target3.transform.position.z);
                break;
            case 1:
                Target = Target2;
                Target.transform.position = new Vector3(0, Target2.transform.position.y, Target2.transform.position.z);
                //Target.transform.position = new Vector3(-20, Target.transform.position.y, Target.transform.position.z);
                Target1.transform.position = new Vector3(-20, Target1.transform.position.y, Target1.transform.position.z);
                Target3.transform.position = new Vector3(-20, Target3.transform.position.y, Target3.transform.position.z);
                swap(Target, Target2);
                break;
            case 2:
                Target = Target3;
                Target.transform.position = new Vector3(0, Target3.transform.position.y, Target3.transform.position.z);
                Target1.transform.position = new Vector3(-20, Target1.transform.position.y, Target1.transform.position.z);
                Target2.transform.position = new Vector3(-20, Target2.transform.position.y, Target2.transform.position.z);
                //Target3.transform.position = new Vector3(0, Target3.transform.position.y, Target3.transform.position.z);
                //Target.transform.position = new Vector3(-20, Target.transform.position.y, Target.transform.position.z);
                //swap(Target, Target3);
                break;
        }
            


        Missle.GetComponent<MissleScript>().violate = false;
        Time.timeScale = 0f;//游戏时间比例
        if (Speedtxt.GetComponent<Inputtext>().gotstr == "")
        { Missle.GetComponent<MissleScript>().MissleSpeed = 3f; }
        else
        { Missle.GetComponent<MissleScript>().MissleSpeed = Speedtxt.GetComponent<Inputtext>().got; }

        if (Heighttxt.GetComponent<Inputtext>().gotstr == "")
        { Missle.GetComponent<MissleScript>().Height = 10f; }
        else
        { Missle.GetComponent<MissleScript>().Height = Heighttxt.GetComponent<Inputtext>().got; }
        if (startDistancetxt.GetComponent<Inputtext>().gotstr == "")
        { Missle.GetComponent<MissleScript>().Startdistance = 10f; }
        else
        { Missle.GetComponent<MissleScript>().Startdistance = startDistancetxt.GetComponent<Inputtext>().got; }
        if (Distancetxt.GetComponent<Inputtext>().gotstr == "")
        { Missle.GetComponent<MissleScript>().Distance = 20f; }
        else
        { Missle.GetComponent<MissleScript>().Distance = Distancetxt.GetComponent<Inputtext>().got; }
        if (ElevationAngletxt.GetComponent<Inputtext>().gotstr == "")
        { Missle.GetComponent<MissleScript>().ElevationAngles = 0f; }
        else
        { Missle.GetComponent<MissleScript>().ElevationAngles = ElevationAngletxt.GetComponent<Inputtext>().got; }
        if (MissleAngletxt.GetComponent<Inputtext>().gotstr == "")
        {
            
            Missle.GetComponent<MissleScript>().MissleAngle = 0f; }
        else
        { Missle.GetComponent<MissleScript>().MissleAngle = MissleAngletxt.GetComponent<Inputtext>().got; }


        if (TargetAngletxt.GetComponent<Inputtext>().gotstr == "")
        {

            Missle.GetComponent<MissleScript>().TargetAngle = 0f;
        }
        else
        { Missle.GetComponent<MissleScript>().TargetAngle = TargetAngletxt.GetComponent<Inputtext>().got; }




        //rb = GetComponent<Rigidbody>();//施加速度时用
        //if (Speedtxt.GetComponent<Inputtext>().gotstr == "")
        //{ Missle.GetComponent<MissleScript>().MissleSpeed = 1f; }
        //else
        //{ Missle.GetComponent<MissleScript>().MissleSpeed = Speedtxt.GetComponent<Inputtext>().got; }//目标转的角度



        Missle.transform.position = new Vector3(0, Missle.GetComponent<MissleScript>().Height, -Missle.GetComponent<MissleScript>().Startdistance);//初始化飞弹位置
        Target.transform.localEulerAngles = new Vector3(0, Missle.GetComponent<MissleScript>().TargetAngle, 0); //初始化目标位置

        RayDireConfirm();
    }

    void RayDireConfirm()
    {
        
        float ElevationAnglesanother = 360 - Missle.GetComponent<MissleScript>().ElevationAngles;
        for (int i = 0; i <= 15; i++)
        {
            eachTan[i] = Mathf.Tan(eachAngles[i] * Mathf.PI / 180);//tan
                                                                   //eachDirections[i] = new Vector3(eachTan[i], -1, 0);//射线方向数组 初始，垂直下

            float x = eachTan[i];
            float y = -1 * Mathf.Cos(ElevationAnglesanother * Mathf.PI / 180);
            float z = -1 * Mathf.Sin(ElevationAnglesanother * Mathf.PI / 180);
            //eachDirections[i] = new Vector3(x, y, z);//仰角
            float xx = x * Mathf.Cos(Missle.GetComponent<MissleScript>().MissleAngle * Mathf.PI / 180) + z * Mathf.Sin(Missle.GetComponent<MissleScript>().MissleAngle * Mathf.PI / 180);
            float zz = -x * Mathf.Sin(Missle.GetComponent<MissleScript>().MissleAngle * Mathf.PI / 180) + z * Mathf.Cos(Missle.GetComponent<MissleScript>().MissleAngle * Mathf.PI / 180);
            Missle.GetComponent<MissleScript>().eachDirections[i] = new Vector3(xx, y, zz); //射线修正偏转之后


        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }


    public void ViolateSim2()
    {


        
        Missle.GetComponent<MissleScript>().violate = true;
        //Debug.Log(Missle.GetComponent<MissleScript>().violate);
        startSimulation();


    }
    public void ViolateSim()
    {
        //利用循环设定参数，然后开始跑
        //Time.timeScale = 0f;//游戏时间比例
        //confirmParameters();//先把变量补上
        //confirmParameters();

        
           // while(Missle.GetComponent<MissleScript>().Height < 20f)
            //{
               // while (Missle.GetComponent<MissleScript>().ElevationAngles <20f)
                //{
                    
            RayDireConfirm();//确定射线方向
        Debug.Log(Missle.GetComponent<MissleScript>().MissleSpeed);
        Missle.GetComponent<MissleScript>().Height =10f; 
        Missle.GetComponent<MissleScript>().Startdistance = 10f;
        Missle.GetComponent<MissleScript>().Distance = 20f;
        Missle.GetComponent<MissleScript>().ElevationAngles = 0f;
        Missle.GetComponent<MissleScript>().MissleAngle = 0f;
        Missle.GetComponent<MissleScript>().TargetAngle = 0f;
        //{
        //Debug.Log(Missle.GetComponent<MissleScript>().MissleSpeed);
        //temp = Missle.GetComponent<MissleScript>().MissleSpeed;
        //temp++;
        // Missle.GetComponent<MissleScript>().MissleSpeed=temp;
        //Missle.GetComponent<MissleScript>().MissleSpeed += 1f;
        //Debug.Log(Missle.GetComponent<MissleScript>().MissleSpeed);
        //Debug.Log(Missle.GetComponent<MissleScript> ().transform.position.z);
        //Missle.GetComponent<MissleScript>().MissleSpeed = spd;
            Missle.transform.position = new Vector3(0, Missle.GetComponent<MissleScript>().Height, -20);//初始化飞弹位置 Missle.GetComponent<MissleScript>().Startdistance
            RayDireConfirm();
            Time.timeScale = 1f;//游戏时间比例 continue;
            if (Missle.GetComponent<MissleScript>().stopped)   
            {
                Missle.GetComponent<MissleScript>().MissleSpeed+=1f;
                //Debug.Log(Missle.GetComponent<MissleScript>().MissleSpeed);
                Time.timeScale = 0f;
            }
            //Menu.SetActive(false);//菜单消失
        //}
        //Debug.Log(Missle.GetComponent<MissleScript>().transform.position.z);

        //Missle.transform.position = new Vector3(0, Missle.GetComponent<MissleScript>().Height, -Missle.GetComponent<MissleScript>().Startdistance);//初始化飞弹位置
        //RayDireConfirm();
        //startSimulation();

        //Missle.GetComponent<MissleScript>().MissleSpeed += 10f;




        //Missle.GetComponent<MissleScript>().Height += 1;

    }


    public void swap(GameObject a1, GameObject a2)
    {

        GameObject temp;
        temp = a1;
        a1 = a2;
        a2 = temp;

    }
}

