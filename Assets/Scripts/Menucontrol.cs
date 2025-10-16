using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menucontrol : MonoBehaviour
{
    public GameObject Switch;
    public GameObject Menu;//本菜单实体
    public GameObject AirPlatform;//获取机载平台实体
    public GameObject Target;//获取目标实体

    public GameObject Target1;//获取目标实体
    public GameObject Target2;//获取目标实体
    public GameObject Target3;//获取目标实体

    public GameObject Linestxt;
    public GameObject viewAngletxt;

    public GameObject Speedtxt;
    public GameObject Heighttxt;
    public GameObject startDistancetxt;
    public GameObject Distancetxt;
    public GameObject stopDistancetxt;
    public GameObject ElevationAngletxt;
    public GameObject FlightAngletxt;
    public GameObject TargetAngletxt;

    public GameObject Missdirecttxt;

    public GameObject Frequencytxt;
    public GameObject AttackAngletxt;
    //public GameObject SlopeAngletxt;
    //public GameObject SlopeHeighttxt;
    int N;
    float Angle;
    float[] eachAngles = new float[64];//每束激光距离垂线的角度
    
    private float temp;
    //Vector3[] eachDirections = new Vector3[32];
    public void startSimulation()
    {   
        Time.timeScale = 1f;//游戏时间比例
        Menu.SetActive(false);//菜单消失
        
    }
    public void confirmParameters()
    {
        

        switch (Switch.GetComponent<TargetDrop>().switchNum)
        {
            case 0:
                Target = Target1;
                Target.transform.position = new Vector3(0, Target1.transform.position.y , Target1.transform.position.z);
                Target2.transform.position = new Vector3(-60, Target2.transform.position.y, Target2.transform.position.z);
                Target3.transform.position = new Vector3(-60, Target3.transform.position.y, Target3.transform.position.z);
                break;
            case 1:
                Target = Target2;
                Target.transform.position = new Vector3(0, Target2.transform.position.y, Target2.transform.position.z);
                //Target.transform.position = new Vector3(-20, Target.transform.position.y, Target.transform.position.z);
                Target1.transform.position = new Vector3(-60, Target1.transform.position.y, Target1.transform.position.z);
                Target3.transform.position = new Vector3(-60, Target3.transform.position.y, Target3.transform.position.z);
                swap(Target, Target2);
                break;
            case 2:
                Target = Target3;
                Target.transform.position = new Vector3(0, Target3.transform.position.y, Target3.transform.position.z);
                Target1.transform.position = new Vector3(-60, Target1.transform.position.y, Target1.transform.position.z);
                Target2.transform.position = new Vector3(-60, Target2.transform.position.y, Target2.transform.position.z);
                //Target3.transform.position = new Vector3(0, Target3.transform.position.y, Target3.transform.position.z);
                //Target.transform.position = new Vector3(-20, Target.transform.position.y, Target.transform.position.z);
                //swap(Target, Target3);
                break;
        }


        //在这实现射线角度变化 功能，


        //首先是调取输入框内的N和角度数据，同调取高度数据一样

        if (Linestxt.GetComponent<Inputtext>().gotstr == "")
        {
            AirPlatform.GetComponent<FlightScript>().Lines = 16;
            N = 16; 
        }
        else
        {
            AirPlatform.GetComponent<FlightScript>().Lines = Convert.ToInt32(Linestxt.GetComponent<Inputtext>().got);
            N = Convert.ToInt32(Linestxt.GetComponent<Inputtext>().got);
        }


        if (viewAngletxt.GetComponent<Inputtext>().gotstr == "")
        {
            AirPlatform.GetComponent<FlightScript>().laserAngles = 48f;
            Angle = 48f;
        }
        else
        {
            AirPlatform.GetComponent<FlightScript>().laserAngles = viewAngletxt.GetComponent<Inputtext>().got;
            Angle = viewAngletxt.GetComponent<Inputtext>().got;
        }
        
        for (int i=1;i<=N;i++)
        {

            eachAngles[i - 1] = (2 * i - N - 1) * Angle / (2 * N);
            //Debug.Log(eachAngles[i - 1]);
        }


        //Debug.Log(Switch.GetComponent<TargetDrop>().switchNum);
        AirPlatform.GetComponent<FlightScript>().violate = false;
        Time.timeScale = 0f;//游戏时间比例
        ConfigureFlightParameters();



        //Missle.transform.localEulerAngles = new Vector3(45, 0, 0);

        float x = AirPlatform.GetComponent<FlightScript>().Height * Mathf.Cos((AirPlatform.GetComponent<FlightScript>().MissDirect)* Mathf.PI / 180);
        float y = AirPlatform.GetComponent<FlightScript>().Height * Mathf.Sin((AirPlatform.GetComponent<FlightScript>().MissDirect)* Mathf.PI / 180);
        float z = -AirPlatform.GetComponent<FlightScript>().Startdistance;
        //Debug.Log(yy);
        //Missle.transform.position = new Vector3(xxx, yyy, -Missle.GetComponent<MissleScript>().Startdistance);//初始化飞弹位置
        float y2 = y * Mathf.Cos(AirPlatform.GetComponent<FlightScript>().AttackAngle * Mathf.PI / 180);
        float z2 = -y * Mathf.Sin(AirPlatform.GetComponent<FlightScript>().AttackAngle * Mathf.PI / 180);

        AirPlatform.transform.position = new Vector3(x, y2,z2+z);
        AirPlatform.transform.localEulerAngles = new Vector3(AirPlatform.GetComponent<FlightScript>().AttackAngle, 0, 0);
        Target.transform.localEulerAngles = new Vector3(0, AirPlatform.GetComponent<FlightScript>().TargetAngle, 0); //初始化目标位置

        RayDireConfirm();
    }

    void RayDireConfirm()
    {
        float[] eachTan = new float[N];
        float ElevationAnglesanother = 360 - AirPlatform.GetComponent<FlightScript>().ElevationAngles;
        
        for (int i = 0; i < N; i++)
        {
            eachTan[i] = Mathf.Tan(eachAngles[i] * Mathf.PI / 180);
            //Debug.Log(eachTan[i]);
        }

        for (int i = 0; i < N; i++)
        {
            
            float x = eachTan[i];
            float y = -1 * Mathf.Cos(ElevationAnglesanother * Mathf.PI / 180);
            float z = -1 * Mathf.Sin(ElevationAnglesanother * Mathf.PI / 180);
            
            //eachDirections[i] = new Vector3(x, y, z);//仰角
            float xx = x * Mathf.Cos(AirPlatform.GetComponent<FlightScript>().FlightAngle * Mathf.PI / 180) + z * Mathf.Sin(AirPlatform.GetComponent<FlightScript>().FlightAngle * Mathf.PI / 180);
            float zz = -x * Mathf.Sin(AirPlatform.GetComponent<FlightScript>().FlightAngle * Mathf.PI / 180) + z * Mathf.Cos(AirPlatform.GetComponent<FlightScript>().FlightAngle * Mathf.PI / 180);
            AirPlatform.GetComponent<FlightScript>().Directions0[i] = new Vector3(xx, y, zz); //射线修正偏转之后
            //Missle.GetComponent<MissleScript>().Directions[i] = new Vector3(x, y, z);//没有乘以矩阵
            //Debug.Log(Missle.GetComponent<MissleScript>().Directions0[i]);
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }


    public void ViolateSim2()
    {



        AirPlatform.GetComponent<FlightScript>().violate = true;
        startSimulation();


    }
    

    public void swap(GameObject a1, GameObject a2)
    {

        GameObject temp;
        temp = a1;
        a1 = a2;
        a2 = temp;

    }

    public void OpenPointCloudFolder()
    {
        // 打开工程根目录下的 PointClouds 文件夹
        string path = Path.Combine(Application.dataPath, "..", "PointClouds");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        // 获取绝对路径
        path = Path.GetFullPath(path);

        // 在不同平台打开文件夹
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        System.Diagnostics.Process.Start("explorer.exe", path.Replace('/', '\\'));
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        System.Diagnostics.Process.Start("open", path);
#else
        Debug.Log($"点云文件保存在: {path}");
#endif
    }

    // 修改清理旧数据的方法
    public void CleanOldPointClouds()
    {
        string path = Path.Combine(Application.dataPath, "..", "PointClouds");

        if (Directory.Exists(path))
        {
            // 保留最近7天的数据
            DateTime cutoffDate = DateTime.Now.AddDays(-7);
            DirectoryInfo dir = new DirectoryInfo(path);

            foreach (DirectoryInfo subDir in dir.GetDirectories())
            {
                if (subDir.CreationTime < cutoffDate)
                {
                    subDir.Delete(true);
                    Debug.Log($"已删除旧数据文件夹: {subDir.Name}");
                }
            }
        }
    }

    // 获取最新数据文件夹的方法
    public string GetLatestDataFolder()
    {
        string path = Path.Combine(Application.dataPath, "..", "PointClouds");

        if (!Directory.Exists(path))
            return null;

        DirectoryInfo dir = new DirectoryInfo(path);
        DirectoryInfo[] subDirs = dir.GetDirectories();

        if (subDirs.Length == 0)
            return null;

        // 按创建时间排序，返回最新的
        Array.Sort(subDirs, (x, y) => y.CreationTime.CompareTo(x.CreationTime));
        return subDirs[0].FullName;
    }


    // 统一的配置方法
    private void ConfigureFlightParameters()
    {
        var flightScript = AirPlatform.GetComponent<FlightScript>();

        // 使用字典定义默认值，更清晰易维护
        var defaultValues = new Dictionary<string, float>
    {
        { "FlightSpeed", 3f },
        { "Height", 10f },
        { "Startdistance", 10f },
        { "Stopdistance", 10f },
        { "Distance", 40f },
        { "ElevationAngles", 0f },
        { "Frequency", 1f },
        { "FlightAngle", 0f },
        { "TargetAngle", 0f },
        { "MissDirect", 90f },
        { "AttackAngle", 0f }
    };

        // 统一配置所有参数
        flightScript.FlightSpeed = GetInputValue(Speedtxt, defaultValues["FlightSpeed"]);
        flightScript.Height = GetInputValue(Heighttxt, defaultValues["Height"]);
        flightScript.Startdistance = GetInputValue(startDistancetxt, defaultValues["Startdistance"]);
        flightScript.Stopdistance = GetInputValue(stopDistancetxt, defaultValues["Stopdistance"]);
        flightScript.Distance = GetInputValue(Distancetxt, defaultValues["Distance"]);
        flightScript.ElevationAngles = GetInputValue(ElevationAngletxt, defaultValues["ElevationAngles"]);
        flightScript.Frequency = GetInputValue(Frequencytxt, defaultValues["Frequency"]);
        flightScript.FlightAngle = GetInputValue(FlightAngletxt, defaultValues["FlightAngle"]);
        flightScript.TargetAngle = GetInputValue(TargetAngletxt, defaultValues["TargetAngle"]);
        flightScript.MissDirect = GetInputValue(Missdirecttxt, defaultValues["MissDirect"]);
        flightScript.AttackAngle = GetInputValue(AttackAngletxt, defaultValues["AttackAngle"]);
    }

    // 统一的输入值获取方法
    private float GetInputValue(GameObject textObject, float defaultValue)
    {
        var inputText = textObject.GetComponent<Inputtext>();
        return string.IsNullOrEmpty(inputText.gotstr) ? defaultValue : inputText.got;
    }


}

