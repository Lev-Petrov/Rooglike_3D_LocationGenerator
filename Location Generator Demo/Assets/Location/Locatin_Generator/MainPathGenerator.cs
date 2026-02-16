using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPathGenerator : MonoBehaviour
{
    [Header("Rooms config")]

    [SerializeField] private GameObject startRoom;
    [SerializeField] private GameObject endRoom;

    [System.Serializable]
    public class Room
    {
        [SerializeField] public GameObject pref;
        [Range(0, 100)] public int probability;
    }
    [SerializeField] private Room[] rooms;

    public int roomsCount;

    [Header("Placement config")]

    [SerializeField] private int distanceBtwRooms;

    Vector3[] placesForRooms;
    HashSet<Vector3> busyPlayse = new HashSet<Vector3>();

    public GameObject[,] roomGrid;

    private void Start()
    {
        ChooseRooms();
    }

    void ChooseRooms()
    {
        List<GameObject> choosedRooms = new List<GameObject>();

        // Adds the start room
        choosedRooms.Add(startRoom);

        for (int i = 0; i < roomsCount; i++)
        {
            choosedRooms.Add(GetRandomRoom());
        }

        // Adds the final room
        choosedRooms.Add(endRoom);

        PlaceRooms(choosedRooms.ToArray());
    }

    void PlaceRooms(GameObject[] roomsToPlace)
    {
        // Initialize the array of room positions
        placesForRooms = new Vector3[roomsToPlace.Length];

        // Set the start room position
        placesForRooms[0] = Vector3.zero;
        busyPlayse.Add(Vector3.zero);

        // Find positions for the remaining rooms
        for (int i = 1; i < roomsToPlace.Length; i++)
        {
            Vector3 newRoomPos = FindPlaceForRoom(placesForRooms[i - 1]);
            placesForRooms[i] = newRoomPos;
        }

        roomGrid = new GameObject[roomsToPlace.Length, roomsToPlace.Length];

        // Instantiate rooms
        for (int i = 0; i < roomsToPlace.Length; i++)
        {
            roomGrid[i, 0] = Instantiate(roomsToPlace[i], placesForRooms[i], Quaternion.identity);
        }
    }

    // Selects a random battle room based on probability
    public GameObject GetRandomRoom()
    {
        int totalProbability = 0;

        foreach (var room in rooms)
            totalProbability += room.probability;

        int randomValue = Random.Range(0, totalProbability);
        int currentSum = 0;

        foreach (var room in rooms)
        {
            currentSum += room.probability;
            if (randomValue < currentSum)
                return room.pref;
        }

        return null;
    }

    // Finds a free position for a new room
    public Vector3 FindPlaceForRoom(Vector3 lastRoomPos)
    {
        for (int j = 0; j < 100; j++)
        {
            // Calculates a new possible position for the room
            Vector3 newRoomPos = lastRoomPos + GetRandomDir() * distanceBtwRooms;

            // Checks if the position is already occupied
            if (!busyPlayse.Contains(newRoomPos))
            {
                busyPlayse.Add(newRoomPos);
                return newRoomPos;
            }
        }

        Debug.LogError("Failed to find a free position for a room. Reloading the scene.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        return Vector3.zero;
    }

    // Generates a random direction for placing a new room
    public Vector3 GetRandomDir()
    {
        return Random.Range(0, 4) switch
        {
            0 => Vector3.forward,
            1 => Vector3.back,
            2 => Vector3.left,
            3 => Vector3.right,
            _ => Vector3.back
        };
    }

    private void OnDrawGizmos()
    {
        if (roomGrid == null) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < roomGrid.GetLength(0) - 1; i++)
        {
            Gizmos.DrawLine(
                roomGrid[i, 0].transform.position,
                roomGrid[i + 1, 0].transform.position
            );
        }
    }
}
