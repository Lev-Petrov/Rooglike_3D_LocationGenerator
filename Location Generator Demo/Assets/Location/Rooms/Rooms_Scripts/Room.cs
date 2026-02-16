using System;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    protected event Action OnPlayerEntered;

    public event Action OnDoorsOpened;
    public event Action OnDoorsClosed;

    [Header("Walls config")]

    [SerializeField] private Transform forwardWall;
    [SerializeField] private Transform backWall;
    [SerializeField] private Transform rightWall;
    [SerializeField] private Transform leftWall;

    [Header("Door")]

    [SerializeField] private GameObject doorPref;
    private List<GameObject> doors = new List<GameObject>();

    [Header("Player")]
    [SerializeField] private string playerTag;


    private void OnTriggerEnter(Collider coll)
    {
        if (!coll.CompareTag(playerTag)) return;

        OnPlayerEntered?.Invoke();
        Debug.Log("Player entered the room: " + name);
    }

    // Replaces a wall with a door
    public Vector3 MakeDoor(Vector3 dir)
    {
        Transform wall = null;

        // Determines which wall should be replaced
        if (dir == Vector3.forward) wall = forwardWall;
        else if (dir == Vector3.back) wall = backWall;
        else if (dir == Vector3.right) wall = rightWall;
        else if (dir == Vector3.left) wall = leftWall;

        if (wall == null) return Vector3.zero;

        // Creates a door instead of the wall
        var door = Instantiate(doorPref, wall.position, wall.rotation, transform);

        // Passes a reference to the current room into the Door component
        door.GetComponent<Door>().room = this;

        doors.Add(door);
        Destroy(wall.gameObject);

        return door.transform.position;
    }


    protected void OpenDoors()
    {
        OnDoorsOpened?.Invoke();
    }

    protected void CloseDoors()
    {
        OnDoorsClosed?.Invoke();
    }
}
