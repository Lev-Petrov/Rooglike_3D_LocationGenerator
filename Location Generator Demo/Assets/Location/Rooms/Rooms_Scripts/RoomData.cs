using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName = "Scriptable Objects/RoomData")]
public class RoomData : ScriptableObject
{
    [Header("Battle room")]

    public GameObject[] enemies;
    public int minEnemiesCount;
    public int maxEnemiesCount;
}
