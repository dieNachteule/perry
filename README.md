## AI-Enhanced Path-Finding Experimentations in Unity

Exploring intelligent navigation techniques and pathfinding algorithms using Unity and AI frameworks.


# 🧠 Hunter AI Strategy

## 🎯 Goals
- Simulate natural hunter behavior with minimal scripting
- Avoid rigid paths and maximize reactivity
- Handle dynamic obstacles and tight terrain
- Create room for future states like search, alert, or cooperation

---

## 🤖 States

### 1. Patrol
- Default state when target is not visible.
- Hunter chooses a **new patrol point dynamically** after reaching the last one.
- Patrol target is:
  - Within a patrol radius from current position
  - Inside arena bounds
  - Not overlapping obstacles
- If hunter becomes vision-cone blocked, it skips the current patrol target.

### 2. Chase
- Activated when the target:
  - Is within vision distance
  - Lies within an 80° cone facing `currentDirection`
  - Is not blocked by walls or obstacles
- Hunter moves directly toward the target until it is lost from view.
- Reverts to Patrol state when target is no longer visible.

---

## 👁️ Vision System
- Hunter uses a cone-based field of view (FOV)
- Detection conditions:
  - Target within `viewDistance`
  - Angle to target within `viewAngle`
  - **Raycast line-of-sight** is unobstructed
- Uses Unity physics filtering:
  - Tags: `Target`, `Wall`
  - Layers: `Entities`, `VisionBlocker`
- A **visual mesh cone** is rendered at runtime to show the active vision area.

---

## 🧱 Cone Blockage Heuristic
- Prevents hunters from staying stuck while facing a wall or dead-end
- Logic:
  - Cast 5 rays spread across the cone
  - Count number of rays blocked by obstacles
  - Measure **furthest unblocked distance** among them
- If **≥90% of rays are blocked** and none reach beyond **50% of view distance**, the cone is considered blocked
- Hunter skips the current patrol target and chooses a new one

---

## 🔧 Arena Integration
- Arena bounds (`arenaWidth`, `arenaHeight`) are passed from `GameManager`
- Hunters use these to:
  - Clamp patrol targets
  - Validate spawn positions

---

## 🧠 Design Notes
- Patrol logic is randomized and light-weight
- Behavior feels dynamic but predictable
- Designed for extensibility with:
  - Search and Alert states
  - Coordinated AI (group behavior)
  - Pathfinding plug-ins

---

## 📌 Future Ideas
- Add pathfinding (A* or NavMesh) for obstacle-aware navigation
- Implement a **Search state** after chase ends (last seen position)
- Add an **Alert mode**: flashing cone, faster speed, aggressive pursuit
- Enable hunters to **communicate** or signal others
- Memory system: temporary persistence after losing sight
- Vision modifiers (fog, darkness, cones overlapping)

---

```mermaid
stateDiagram-v2
    [*] --> Patrol

    Patrol --> Chase : Target in cone & visible
    Chase --> Patrol : Lost sight of target

    Patrol : Pick random target
    Patrol : Move toward target
    Patrol --> Patrol : Reached point\n→ pick new one
    Patrol --> Patrol : Cone mostly blocked\n→ skip to new point

    Chase : Move toward target
    Chase : Check visibility

    state Patrol {
        [*] --> Moving
        Moving --> Waiting : Reached target
        Waiting --> Moving : Pause expired
    }
```

---
