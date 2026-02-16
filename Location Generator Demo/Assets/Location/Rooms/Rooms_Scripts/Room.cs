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


    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag != "GameController") return;

        OnPlayerEntered?.Invoke();
        Debug.Log("Гравець зайшов в кімнату: " + name);
    }

    //Замінює стіну на двері
    public Vector3 MakeDoor(Vector3 dir)
    {
        Transform wall = null;

        if (dir == Vector3.forward) wall = forwardWall;
        else if (dir == Vector3.back) wall = backWall;
        else if (dir == Vector3.right) wall = rightWall;
        else if (dir == Vector3.left) wall = leftWall;

        if (wall == null) return Vector3.zero;

        var door = Instantiate(doorPref, wall.position, wall.rotation, transform);
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
