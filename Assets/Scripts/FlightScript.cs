using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
//发射LeddarTech16线的射线。
//后续改成可调节线数，角度，探测距离。

public class FlightScript : MonoBehaviour
{
    private List<string> pointCloudData = new List<string>(); // 替换原来的 pointcloud 字符串
    private List<float[,]> rangeMatrices = new List<float[,]>(); // 存储N*m的距离矩阵
    private float[,] currentRangeMatrix; // 当前扫描的距离矩阵
    private int scanIndex = 0; // 当前扫描索引
    private string sessionId; // 本次仿真的唯一ID

    [Header("激光雷达线数")]
    public int Lines = 64;//线数可改，最大64
    [Header("激光雷达视场角")] public float laserAngles = 128;//角度可改
    [Header("飞行速度")]
    public float FlightSpeed = 10f;//飞行速度
    [Header("飞行偏航角")]
    public float FlightAngle = 0f;//飞行偏转角度
    [Header("激光发射频率")]
    public float Frequency=1f;//激光发射频率
    [Header("最大探测距离")] 
    public float Distance = 20f;//最大探测距离
    [Header("飞行高度")]
    public float Height = 10f;//飞弹高度
    [Header("探测前仰角")]
    public float ElevationAngles = 0f; //探测器抬起来的角度
    private float ElevationAnglesanother = 0f;//用于储存逆时针
    [Header("初始距离")]
    public float Startdistance = 30f; //飞弹与目标距离
    [Header("停止距离")]
    public float Stopdistance = 30f; //飞弹与目标距离
    [Header("目标角度")]
    public float TargetAngle = 0f; //飞弹与目标角度
    [Header("脱靶量")]
    public float MissDist = 8f; //脱靶量
    [Header("脱靶方位")]
    public float MissDirect = 0f; //脱靶方位
    [Header("攻击角")]
    public float AttackAngle= 0f; //攻击角


    public LineRenderer line;//真实画射线
    private LineRenderer[] lineRendArray;

    public Boolean stopped;
    public Boolean violate;
    public RaycastHit hit;//这个类用于保存射线打到之后的信息。
    //以下只是声明空数组，用于存储方向，最大设为32.赋值内容在MenuControl中
    //float[] eachAngles = new float[32];
    //float[] eachTan = new float[Lines];
    public Vector3[] Directions0= new Vector3[128];
    public GameObject Menu;

    // Start is called before the first frame update
    private Rigidbody rb;
    string path=" ";//文件名用的字符串
    StreamWriter writer;
    StreamReader reader;//读写流
    //FileStream fs = File.OpenWrite("D:/PC1.txt");

    string pointcloud="";//用来保存点云的字符串变量。
    private float lastupdate;//计时器
    private void Awake()
    {
        
            //Time.timeScale = 0;//初始暂停状态 
    }
    void Start()
    {
        // 初始化会话ID和距离矩阵
        sessionId = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        currentRangeMatrix = new float[Lines, 1000]; // 假设最多1000次扫描
        scanIndex = 0;


        rb = GetComponent<Rigidbody>();//施加速度时用
        line= GetComponent<LineRenderer>();//获取renderer
        lineRendArray = new LineRenderer[128];
        for (int i=0;i< Lines; i++)
        {
            lineRendArray[i]= new GameObject().AddComponent<LineRenderer>();
        }
        Time.timeScale = 0;

    }
    // Update is called once per frame
    
    void Update()
    {
        //物理相关在fixed中实现

    }
    private void FixedUpdate()
    {
        FlightMovement();

        if (Time.fixedTime -lastupdate> 1/Frequency)
        {
            lastupdate = Time.fixedTime;
            drawRay();

        }
        if (transform.position.z > Stopdistance)
        {
            // 保存本次仿真的所有数据
            SaveSimulationData();

        }

    }
    void drawRay()
    {
        Vector3 lidarPosition = transform.position; // LiDAR当前位置
        float[,] currentScan = new float[Lines, 4]; // 存储当前扫描的数据 [x, y, z, range]

        for (int i = 0; i < Lines; i++)
        {
            Ray ray = new Ray(lidarPosition, Directions0[i]);

            if (Physics.Raycast(ray, out hit, Distance))
            {
                float range = hit.distance;

                // 存储距离到矩阵
                if (scanIndex < 1000)
                {
                    currentRangeMatrix[i, scanIndex] = range;
                }

                // 使用球坐标系反推世界坐标（真正的成像过程）
                Vector3 localHitPoint = CalculatePointFromRange(i, range, lidarPosition);

                // 存储点云数据
                string pointData = $"{localHitPoint.x:F3} {localHitPoint.y:F3} {localHitPoint.z:F3} {range:F3}";
                pointCloudData.Add(pointData);

                // 绘制射线（保持原有的可视化）
                if (lineRendArray[i] != null)
                {
                    lineRendArray[i].material = new Material(Shader.Find("Sprites/Default"));
                    lineRendArray[i].SetVertexCount(2);
                    lineRendArray[i].SetColors(Color.red, Color.red);
                    lineRendArray[i].SetWidth(0.05f, 0.05f);
                    lineRendArray[i].SetPosition(0, ray.origin);
                    lineRendArray[i].SetPosition(1, localHitPoint);
                }
            }
            else
            {
                // 未检测到目标时存储最大距离
                if (scanIndex < 1000)
                {
                    currentRangeMatrix[i, scanIndex] = Distance;
                }
            }
        }

        scanIndex++;
    }

    // 新增方法：根据距离和角度计算真实坐标
    private Vector3 CalculatePointFromRange(int lineIndex, float range, Vector3 lidarPos)
    {
        // 获取该束激光的方向向量
        Vector3 direction = Directions0[lineIndex];

        // 计算相对于LiDAR的局部坐标
        Vector3 localPoint = direction.normalized * range;

        // 转换到世界坐标
        Vector3 worldPoint = lidarPos + localPoint;

        return worldPoint;
    }

    void FlightMovement()
    {
        //在这里写导弹的旋转、飞行姿态
        
        float x = rb.velocity.x;
        float y = rb.velocity.y;
        float z = FlightSpeed /* 100 * Time.fixedDeltaTime*/;

        float xx = -z * Mathf.Sin(FlightAngle * Mathf.PI / 180);
        float zz = z * Mathf.Cos(FlightAngle * Mathf.PI / 180);
       
        rb.velocity = new Vector3(x, -FlightSpeed * Mathf.Sin(AttackAngle * Mathf.PI / 180), FlightSpeed*Mathf.Cos(AttackAngle * Mathf.PI / 180));//攻击角45度
        stopped = false;
        //rb.velocity = new Vector3(xx, y, zz);//视角也扭，移动也扭
        //transform.position = new Vector3(0, Height, -20);
        //后面做一个飞过多少距离后终止，思路是指定飞行距离d，current.z-former.z>d，stop并销毁
        if(transform.position.z>Stopdistance) //或者产生碰撞时
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
                FlightSpeed += 1f;
                if (FlightSpeed >= 101f)
                { 
                    Height += 1; FlightSpeed = 2f; 
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
            //rb.transform.position = new Vector3(0, Height, -Startdistance);//初始化位置
            //rb.transform.localEulerAngles = new Vector3(0, 0, 0);
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


    private void SaveSimulationData()
    {
        // 保存到工程根目录下的 PointClouds 文件夹
        string outputDir = Path.Combine(Application.dataPath, "..", "PointClouds", sessionId);

        // 或者如果你想保存到 Assets 同级的文件夹
        // string outputDir = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "PointClouds", sessionId);

        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // 保存点云数据
        SavePointCloud(outputDir);

        // 保存距离矩阵
        SaveRangeMatrix(outputDir);

        // 清理数据
        pointCloudData.Clear();
        scanIndex = 0;

        Debug.Log($"仿真数据已保存到: {outputDir}");

        // 在Unity编辑器中刷新资源
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    private void SavePointCloud(string outputDir)
    {
        string fileName = $"PointCloud_Speed{FlightSpeed:F1}_Height{Height:F1}_MissDir{MissDirect:F1}.txt";
        string filePath = Path.Combine(outputDir, fileName);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine($"# LiDAR Point Cloud Data");
            writer.WriteLine($"# Session: {sessionId}");
            writer.WriteLine($"# Lines: {Lines}, FOV: {laserAngles}°");
            writer.WriteLine($"# Format: X Y Z Range");
            writer.WriteLine($"# Total Points: {pointCloudData.Count}");

            foreach (string pointData in pointCloudData)
            {
                writer.WriteLine(pointData);
            }
        }
    }

    private void SaveRangeMatrix(string outputDir)
    {
        string fileName = $"RangeMatrix_{Lines}x{scanIndex}.csv";
        string filePath = Path.Combine(outputDir, fileName);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine($"# Range Matrix: {Lines} beams x {scanIndex} scans");

            for (int i = 0; i < Lines; i++)
            {
                List<string> rowData = new List<string>();
                for (int j = 0; j < scanIndex; j++)
                {
                    rowData.Add(currentRangeMatrix[i, j].ToString("F3"));
                }
                writer.WriteLine(string.Join(",", rowData));
            }
        }
    }



}
