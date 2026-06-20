# 🛰️ Airborne LiDAR Simulation System  (Push-broom)
_A Unity-based simulation framework for airborne LiDAR scanning, point cloud generation, and data export._

---

## 🧭 Overview

This project simulates an **airborne push-broom LiDAR system** for 3D scanning and point cloud data generation.  
It models real-time flight dynamics, configurable laser emission geometry, and automatic export of point cloud and range matrix data.

> Designed for research in LiDAR imaging, airborne sensing, and point cloud analysis.

---

## 🧱 1. System Architecture

<img width="854" height="681" alt="Airborne" src="https://github.com/user-attachments/assets/5b61f321-cc9c-4e87-8704-73547b957df3" />

---

## 🔭 2. LiDAR Simulation Principle

- **Multi-Beam Emission:** Configurable beam count (`Lines`) and field of view (`laserAngles`).
- **Raycasting:** Each beam performs a `Physics.Raycast` to simulate distance measurement.
- **Realistic Geometry:** Supports pitch, yaw, and elevation adjustments.
- **Data Capture:** Records both raw point cloud (XYZ + range) and range matrices.

📸 *(Figure 2 Placeholder: Multi-beam scanning and FOV illustration)*

---

## ⚙️ 3. Core Components

| Script | Function | Key Methods |
|---------|-----------|-------------|
| **FlightScript.cs** | Core LiDAR logic, ray emission, data export | `drawRay()`, `FlightMovement()`, `SaveSimulationData()` |
| **Menucontrol.cs** | User interface, parameter configuration, simulation control | `confirmParameters()`, `RayDireConfirm()` |
| **Inputtext.cs** | UI input listener and numeric parsing | `End_Value()` |
| **TargetDrop.cs** | Dropdown target selection logic | `ConsoleResult()` |
| **Targetcontrol.cs** | Target behavior placeholder for rotation or slope setup | *(reserved for expansion)* |

📸 *(Figure 3 Placeholder: Class dependency or module relationship diagram)*

---

## 📐 4. Simulation Parameters

### **LiDAR Settings**
| Parameter | Description | Default |
|------------|-------------|----------|
| `Lines` | Number of beams (16–64) | 16 |
| `laserAngles` | Field of view (°) | 48 |
| `Frequency` | Emission rate (Hz) | 1 |
| `Distance` | Maximum detection distance | 40 |

### **Flight Parameters**
| Parameter | Description | Default |
|------------|-------------|----------|
| `FlightSpeed` | Platform flight speed | 3 |
| `Height` | Flight altitude | 10 |
| `AttackAngle` | Pitch angle | 0 |
| `FlightAngle` | Yaw deviation | 0 |
| `Startdistance` / `Stopdistance` | Start & stop range | 10 / 10 |

### **Target Parameters**
| Parameter | Description | Default |
|------------|-------------|----------|
| `TargetAngle` | Target rotation angle | 0 |
| `MissDist` / `MissDirect` | Miss distance and direction | 8 / 90 |

---

## 💾 5. Data Output

| Output Type | Format | Description |
|--------------|---------|-------------|
| **Point Cloud** | `.txt` | XYZ + range for each beam |
| **Range Matrix** | `.csv` | Distance matrix `[Lines × ScanIndex]` |
| **Storage** | `/PointClouds/<timestamp>/` | Automatically created per session |

Example:
```

/PointClouds/
 ├── 20251018_142300/
 │   ├── PointCloud_Speed10.0_Height10.0_MissDir0.0.txt
 │   └── RangeMatrix_64x250.csv

```
📸 *(Figure 4 Placeholder: Example data folder structure)*

---

## 🚀 6. Usage Guide

1. Launch Unity and open the simulation scene.  
2. Configure LiDAR and flight parameters via the **menu panel**.  
3. Select target type using the dropdown (Target1 / 2 / 3).  
4. Click **Confirm Parameters** to validate input.  
5. Click **Start Simulation** to begin scanning.  
6. When the flight ends, point cloud and range matrix data are automatically saved.

---

## 📈 7. Visualization & Analysis

- Import `.txt` point clouds into **CloudCompare**, **Open3D**, or **Matlab** for visualization.  
- Use `.csv` range matrices for **heatmap visualization** or **depth analysis**.  
- The exported data format is fully compatible with common 3D point cloud processing tools.
While this simulator shares similar concepts with generic LiDAR environments (e.g., autonomous driving simulators， *Blensor* or *Gazebo*-based robotic frameworks),  
it is **specifically tailored for airborne push-broom and intersection-scanning configurations** —  
allowing easy modification of flight trajectory, scanning geometry, and LiDAR–target intersection parameters.

> 🛩️ This makes it particularly suitable for airborne sensor experiments, aerial photogrammetry research, and LiDAR data synthesis under controlled flight dynamics.
![image](https://github.com/user-attachments/assets/ef20bc07-d93f-40b7-945b-9af4894e65cc)

---

## 🧩 8. Future Work

- Target motion and dynamic environment modeling  
- Noise model and reflectivity simulation  
- PLY/PCD point cloud export support  
- Integrated in-Unity visualization panel  

---

## 📜 License

This project is open-sourced under the **MIT License**.  
You are free to use and modify it for research and educational purposes with proper attribution.

---

> ✨ _Placeholder figures (1–4) can later be replaced with architecture, FOV, UML, and data output diagrams for publication use._
## 🇨🇳 中文简介
这是一个基于 Unity 的机载/弹载激光雷达仿真项目，用于指定交会姿态下的点云数据生成与分析。
支持多线束扫描、参数化设置与自动导出。
