using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
//发射LeddarTech16线的射线。
//后续改成可调节线数，角度，探测距离。

public class MissleScript : MonoBehaviour
{
    [Header("激光雷达线数")]
    public int Lines = 16;//线数暂时不能改
    [Header("激光雷达视场角")] public float laserAngles = 48;//角度，暂时不能改
    [Header("飞行速度")]
    public float MissleSpeed = 10f;//飞行速度
    [Header("飞行偏航角")]
    public float MissleAngle = 0f;//飞行偏转角度
    [Header("激光发射频率")]
    public float Frequency = 3;//激光发射频率
    [Header("最大探测距离")]
    public float Distance = 20f;//最大探测距离
    [Header("飞行高度")]
    public float Height = 10f;//飞弹高度
    [Header("探测前仰角")]
    public float ElevationAngles = 0f; //探测器抬起来的角度
    private float ElevationAnglesanother = 0f;//用于储存逆时针
    [Header("飞弹初始距离")]
    public float Startdistance = 30f; //飞弹与目标距离
    [Header("目标角度")]
    public float TargetAngle = 0f; //飞弹与目标距离


    public LineRenderer line;//真实画射线
    private LineRenderer[] lineRendArray;

    public Boolean stopped;
    public Boolean violate;
    //public float tempheight;
    //myline[] lines = new myline[16];
    public RaycastHit hit;//这个类用于保存射线打到之后的信息。
    //public GameObject Speedtxt;
    float[] eachAngles = { -22.5f, -19.5f, -16.5f, -13.5f, -10.5f, -7.5f, -4.5f, -1.5f, 1.5f, 4.5f, 7.5f, 10.5f, 13.5f, 16.5f, 19.5f, 22.5f };//每束激光距离垂线的角度
    float[] eachTan = new float[16];
    public Vector3[] eachDirections= new Vector3[16];
    public GameObject Menu;

    // Start is called before the first frame update
    private Rigidbody rb;
    string path=" ";//文件名用的字符串
    StreamWriter writer;
    StreamReader reader;//读写流
    //FileStream fs = File.OpenWrite("D:/PC1.txt");

    string pointcloud="";//用来保存点云的全局变量。
    private void Awake()
    {
        //Time.timeScale = 0;//初始暂停状态
        
        
    }

    void Start()
    {
        //应该开始后获取这些数据
        /*ElevationAnglesanother = 360 - ElevationAngles;
        for (int i = 0; i <= 15; i++)
        {
            eachTan[i] = Mathf.Tan(eachAngles[i] * Mathf.PI / 180);//tan
            //eachDirections[i] = new Vector3(eachTan[i], -1, 0);//射线方向数组 初始，垂直下
            
            float x = eachTan[i];
            float y = -1 * Mathf.Cos(ElevationAnglesanother * Mathf.PI / 180);
            float z = -1 * Mathf.Sin(ElevationAnglesanother * Mathf.PI / 180);
            //eachDirections[i] = new Vector3(x, y, z);//仰角
            float xx = x * Mathf.Cos(MissleAngle * Mathf.PI / 180) + z * Mathf.Sin(MissleAngle * Mathf.PI / 180);
            float zz = -x * Mathf.Sin(MissleAngle * Mathf.PI / 180) + z * Mathf.Cos(MissleAngle * Mathf.PI / 180);
            eachDirections[i] = new Vector3(xx, y,zz); //射线修正偏转之后

            //Debug.Log(eachTan[i]);//不确定射线数组是否能用
        }*/
        rb = GetComponent<Rigidbody>();//施加速度时用
        line= GetComponent<LineRenderer>();//获取renderer
        lineRendArray = new LineRenderer[16];
        for (int i=0;i<16;i++)
        {
            lineRendArray[i]= new GameObject().AddComponent<LineRenderer>();
        }
        
        //transform.position  = new Vector3 (0,Height,-Startdistance);//初始化飞弹位置
        Time.timeScale = 0;
        //Text tt = speedtxt.transform.Find("Text").GetComponent<Text>();
        //MissleSpeed = Speedtxt.GetComponent<Inputtext>().got;
        //Debug.Log(MissleSpeed);
    }
    // Update is called once per frame
    
    void Update()
    {
       
        MissleMovement();
        
    }
    private void FixedUpdate()
    {
        drawRay();
        //path用参数命名，并放到D盘，后期要加界面实现指定位置指定名称。
        path = "D:/" + "探测距离" + Distance.ToString()+",发射频率"+ Frequency.ToString()+ ",飞行高度"+ Height.ToString()+ ",飞行速度" +MissleSpeed.ToString()+ ",仰角"+ ElevationAngles.ToString()+ ",偏航角" + MissleAngle.ToString()+".txt";
        WriteIntoTxt(pointcloud,path);
    }
    void drawRay()
    {
       
        Vector3 Raypoint = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);//激光发射位置

       


        for (int i = 0; i <= 15; i++)//16条线工作
        {
            Ray ray = new Ray(transform.position, eachDirections[i]);//向下画条线
            if (Physics.Raycast(ray, out hit, Distance))
            {
                
                
                pointcloud = pointcloud + hit.point.x + " " + hit.point.y + " " + hit.point.z +" "+ hit.distance+ "\r\n";
                //byte[] data = new UTF8Encoding().GetBytes(pointcloud);


                // 如果射线与平面碰撞，打印碰撞物体信息  
                //Debug.Log("碰撞对象: " + hit.collider.name);
                // 在场景视图中绘制射线  
                //Debug.DrawLine(ray.origin, hit.point, Color.green);
                
                lineRendArray[i].material = new Material(Shader.Find("Sprites/Default"));
                lineRendArray[i].SetVertexCount(2);
                lineRendArray[i].SetColors(Color.red, Color.red);
                lineRendArray[i].SetWidth(0.05f, 0.05f);//设置直线宽度
                lineRendArray[i].SetPosition(0, ray.origin);
                lineRendArray[i].SetPosition(1, hit.point);


            }
        }
        
    }

    void MissleMovement()
    {
        //在这里写导弹的旋转、飞行姿态
        
        float x = rb.velocity.x;
        float y = rb.velocity.y;
        float z = MissleSpeed /* 100 * Time.fixedDeltaTime*/;

        float xx = -z * Mathf.Sin(MissleAngle * Mathf.PI / 180);
        float zz = z * Mathf.Cos(MissleAngle * Mathf.PI / 180);
        //float xx = x * Mathf.Cos(MissleAngle * Mathf.PI / 180) - y * Mathf.Sin(MissleAngle * Mathf.PI / 180);
        //float yy = x * Mathf.Sin(MissleAngle * Mathf.PI / 180) + y * Mathf.Cos(MissleAngle * Mathf.PI / 180);
        //rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, MissleSpeed *100* Time.fixedDeltaTime);//* Time.fixedDeltaTime //原始直着走

        //向着偏转角移动
        rb.velocity = new Vector3(x, y, MissleSpeed);//平移但是视角变化 认为1m=100cm=100unitym
        //rb.velocity = new Vector3(x, y, MissleSpeed * 60*Time.fixedDeltaTime);//平移但是视角变化 认为1m=100cm=100unitym，同时假设60fps
        //rb.velocity = new Vector3(x, y, MissleSpeed * Time.fixedDeltaTime);
        //rb.velocity = new Vector3(x, y, MissleSpeed* Time.deltaTime);//平移但是视角变化 认为1m=100cm=100unitym
        stopped = false;
        //rb.velocity = new Vector3(xx, y, zz);//视角也扭，移动也扭
        //transform.position = new Vector3(0, Height, -20);
        //后面做一个飞过多少距离后终止，思路是指定飞行距离d，current.z-former.z>d，stop并销毁
        if(transform.position.z>20)
        {


            //Height += 1f;
            //if(Height==20)
            //{
            //进位，\
            // Height = 5f;
            //Debug.Log(violate);
            pointcloud = "";
            if (!violate)
            {
                Time.timeScale = 0f;
                Menu.SetActive(true);//菜单出现
            }
            else
            {
                pointcloud = "";
                MissleSpeed += 1f;
                if (MissleSpeed >= 101f)
                { 
                    Height += 1; MissleSpeed = 2f; 
                }
                if(Height>=20)
                {
                    Time.timeScale = 0f;
                    Menu.SetActive(true);//菜单出现
                }
            }
                
                
            //}
            

            rb.velocity = new Vector3(0, 0, 0);//停止运动
            //
            //Debug.Log("stopped");
            rb.transform.position = new Vector3(0, Height, -Startdistance);//初始化位置
            
            //rb.transform.position = new Vector3(0, 10, -Startdistance);
            //下一步弹出窗口
            stopped = true;
            //
            //Destory(hit.collider.gameObject);
        }
    }

    public void WriteIntoTxt(string message,string path)
    {
        
        //string path = "D:/saved.txt";  //创建文件并命名
        FileStream fs = File.OpenWrite(path);//写入文件

        byte[] data = Encoding.UTF8.GetBytes(message);   
        fs.Write(data,0, data.Length);
        fs.Close();
        fs.Dispose();
        //Debug.Log(message);
    }
    
    
}
