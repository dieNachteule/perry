# 🗺️ Room & Level Design Document

## 🧩 Definition of a Room
- A **room** is a large area—often much larger than the current camera viewport.
- The player can **freely move and scroll** within the room.
- Rooms connect to each other via edges, tunnels, doors, or triggers.
- Designed for **modularity**: either hand-crafted or procedurally generated.

## 🧱 Current Implementation

### 🏗️ Room Prefabs
- Contain tilemap or terrain
- Include entities, spawn points, camera bounds
- Have trigger-based transitions at edges or defined zones

### 🧠 Room Manager
- Loads and unloads room prefabs dynamically
- Moves player to spawn point upon transition
- Sets up camera confiner boundaries and zoom
- May preload adjacent areas or stream in content later

### 🎥 Camera Behavior
- Uses **CinemachineVirtualCamera** with `Confiner2D`
- Camera is locked to each room’s defined bounds
- Zoom level can vary per room or situation
- Smooth transitions between zoom levels

## 🔮 Planned Features
- Area metadata: name, theme, ambient track, danger level
- Minimap with fog-of-war support
- Room Editor Tool (external or Unity Editor plugin)
- Room graph with adjacency logic
- Interactive props and environmental hazards
- Spawn zones for AI based on difficulty or alert state

## 📂 Suggested Directory Structure
```text
Assets/ 
├── Rooms/ # Prefabs and tilemaps 
├── Data/ 
│ ├── Rooms/ # Room metadata (ScriptableObjects or JSON) 
│ └── Entities/ # Entity templates and definitions 
├── Scripts/ 
│ ├── RoomSystem/ # RoomManager, TransitionZones, Load logic 
│ └── EntityAI/ # Entity logic and controllers 
| DesignDocuments/ 
| ├── Room_Level_Design.md 
| └── Entity_AI_Design.md
```

## ✅ Notes
- Design supports both authored and procedural generation
- Each room can define **camera zoom level** and **transition directions**
- Level designer tools will evolve to visualize and link rooms more easily
