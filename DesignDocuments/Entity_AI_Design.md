# 🧠 Entity & AI Design Document

## 🎮 Player
- **Control**: Currently controlled via **gamepad** (see `TargetController.cs`)
- **Movement**: Physics-based movement; direction aligned with gamepad input
- **Shield or Directional Facing**: Under development; possibly linked to mouse or right-stick

## 👁️ Hunters (Chasers)
- **Vision Cone**: 60° field of view; rotates with movement direction
- **Line of Sight**: Requires unobstructed view to see player
- **Behavior States**:
  - **Patrolling**: Default behavior
  - **Chasing**: Triggered when player enters FOV and is visible
  - **Searching**: Triggered when player is lost (was visible but moved out of range)
  - **Coordinating** (Planned): Share information, fan out, corner

## 🔮 Planned Entity Behaviors
- **Scouts**: Fast, poor fighters, alert others
- **Sentries**: Stationary, wide FOV, trigger alarms
- **Lurkers**: Wait in hiding, react to sound/motion
- **Bosses**: Unique mechanics, scripted encounters

## 🔗 Systems to Develop
- Memory: Hunters remember player location briefly after LOS lost
- Communication: Allow AI to alert nearby allies
- Hearing: Trigger behavior when player makes noise
- Stealth: Light level, line of sight, and sound affect detectability

---

## ✅ Source Reference
`TargetController.cs` (Player Control)  
https://github.com/dieNachteule/perry/blob/main/Assets/scripts/TargetController.cs

### Notes:
- Uses `Gamepad.current.leftStick.ReadValue()` for movement
- Movement vector is applied to `Rigidbody2D` using `MovePosition`
- No mouse input used yet for facing/shield
- Could support hybrid input (e.g., right stick for aim/facing)
