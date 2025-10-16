# Airborne Lidar Simulation
This is a Unity3D simulation project for airborne push-broom LiDAR point cloud imaging and data generation.

## Project Overview

This simulation models an airborne LiDAR system that scans targets and generates point cloud data. The system allows parameter configuration for various scanning scenarios and exports the collected point cloud data for analysis and recognition.

<img width="854" height="681" alt="Airborne" src="https://github.com/user-attachments/assets/5b61f321-cc9c-4e87-8704-73547b957df3" />

## Key Features

- **Configurable LiDAR Parameters**: Adjustable beam count (16-64 beams), field of view, scanning frequency
- **Flight Parameter Control**: Platform speed, altitude, approach angle, scanning distance
- **Multiple Target Types**: Support for different target configurations
- **Point Cloud Export**: Automatic saving of point cloud data in text format
- **Range Matrix Data**: Export of distance matrices for analysis

## Simulation Parameters

**LiDAR Settings:**

- Beam Count (Lines): 16-64 beams
- Field of View: Configurable angle
- Scanning Frequency: Pulses per second
- Maximum Detection Range

**Platform Parameters:**

- Platform Speed
- Flight Altitude
- Start/Stop Distance
- Approach Angle
- Scanning Geometry
- ...

**Target Parameters:**

- Target Rotation Angle
- Target type

## Output Data

The simulation generates:

- Point cloud data files (XYZ coordinates + range)
- Range matrices in CSV format
- Organized by session ID with timestamp
- Saved in `PointClouds` folder in project directory

## Usage

1. Configure LiDAR and platform parameters in the menu
2. Select target type from dropdown
3. Click "Confirm Parameters" to validate settings
4. Start simulation to begin scanning
5. Point cloud data automatically exports when simulation completes

## Project Structure

The simulation uses Unity's physics engine for ray casting and implements realistic LiDAR scanning patterns with configurable beam distribution and scanning geometry. The system provides a flexible framework for airborne LiDAR simulation with extensible parameter systems and comprehensive data output capabilities.

## Note

This project simulates airborne LiDAR scanning patterns and is designed for research and educational purposes in point cloud data generation and analysis.

![image](https://github.com/user-attachments/assets/ef20bc07-d93f-40b7-945b-9af4894e65cc)

