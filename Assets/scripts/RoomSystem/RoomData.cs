using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RoomData", menuName = "Level/Room Data")]
public class RoomData : ScriptableObject {
    public string roomID;
    public GameObject prefab;
    public Vector2 defaultSpawnPosition;
    public float defaultZoom = 5.0f;
    public Collider2D cameraBounds;

    [System.Serializable]
    public class RoomExit {
        public string direction;      // e.g., "left", "right"
        public string targetRoomID;   // e.g., "room_B2"
    }

    public List<RoomExit> exits = new();
}