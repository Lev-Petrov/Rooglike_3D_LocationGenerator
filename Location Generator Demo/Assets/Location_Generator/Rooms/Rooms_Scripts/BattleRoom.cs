using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class BattleRoom : Room
{
    [SerializeField] private BattleData battleData;

    // Flag to prevent the battle from starting multiple times
    private bool battleStarted;

    // List of all enemies spawned in the room
    public List<GameObject> enemiesInRoom = new List<GameObject>();

    private void Start()
    {
        OnPlayerEntered += StartBattle;
    }

    // Method that starts the battle
    void StartBattle()
    {
        if (battleStarted) return;

        battleStarted = true;

        Debug.Log("Battle started");

        CloseDoors();

        // Random number of enemies within the specified range
        int enemiesCount = Random.Range(battleData.minEnemiesCount, battleData.maxEnemiesCount);

        // Get the room size from the BoxCollider
        Vector3 roomSize = GetComponent<BoxCollider>().size;
        roomSize /= 2; // Divide by 2 to work with half extents

        // Spawn enemies
        for (int i = 0; i < enemiesCount; i++)
        {
            // Random position inside the room on X and Z axes
            float x = Random.Range(-roomSize.x, roomSize.x);
            float z = Random.Range(-roomSize.z, roomSize.z);

            Vector3 newEnemyPos = new Vector3(x, 0, z) + transform.position;

            // Pick a random enemy prefab
            int index = Random.Range(0, battleData.enemies.Length);

            // Create the enemy and add it to the list
            enemiesInRoom.Add(
                Instantiate(
                    battleData.enemies[index],
                    newEnemyPos,
                    Random.rotation
                )
            );
        }

        StartCoroutine(WaitForAllEnemiesToBeDefeated());
    }

    // Coroutine that waits until the battle is finished
    IEnumerator WaitForAllEnemiesToBeDefeated()
    {
        // Waits until all enemies in the list are null (destroyed)
        yield return new WaitUntil(() => enemiesInRoom.All(enemy => enemy == null));

        OpenDoors();

        Debug.Log("Fight ended");
    }
}
