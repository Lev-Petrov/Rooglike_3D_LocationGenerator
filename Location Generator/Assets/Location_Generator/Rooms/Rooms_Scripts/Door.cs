using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject barrier;

    [HideInInspector] public Room room;

    private void Start()
    {
        room.OnDoorsOpened += Open;
        room.OnDoorsClosed += Close;
    }

    private void Open()
    {
        barrier.SetActive(false);
    }

    private void Close()
    {
        barrier?.SetActive(true);
    }
}
