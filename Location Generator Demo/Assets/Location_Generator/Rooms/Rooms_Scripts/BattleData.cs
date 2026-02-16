
using UnityEngine;

[CreateAssetMenu(fileName = "BattleData", menuName = "Scriptable Objects/BattleData")]
public class BattleData : ScriptableObject
{
    public GameObject[] enemies;
    public int minEnemiesCount;
    public int maxEnemiesCount;
}
