using System.Collections;
using UnityEngine;

public class PassagesGenerator : MonoBehaviour
{
    [Header("Passages config")]

    [SerializeField] private GameObject[] passageSegments;

    MainPathGenerator path;

    private void Start()
    {
        path = GetComponent<MainPathGenerator>();

        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        // Wait a couple of frames to make sure rooms are generated
        for (int i = 0; i < 2; i++)
            yield return new WaitForEndOfFrame();

        GeneratePasseges();
    }

    void GeneratePasseges()
    {
        // Creates passages for the main path
        for (int i = 0; i < path.roomGrid.GetLength(0) - 1; i++)
        {
            PlacePasseg(path.roomGrid[i, 0], path.roomGrid[i + 1, 0]);
        }

        // Creates passages for the forks
        for (int i = 0; i < path.roomGrid.GetLength(0); i++)
        {
            // Looks for a fork in the grid
            if (path.roomGrid[i, 1] == null) continue;

            int j = 0;
            while (path.roomGrid[i, j + 1] != null)
            {
                PlacePasseg(path.roomGrid[i, j], path.roomGrid[i, j + 1]);
                j++;
            }
        }
    }

    void PlacePasseg(GameObject from, GameObject to)
    {
        Vector3 dir = (to.transform.position - from.transform.position).normalized;

        // Finds the start and end points of the passage
        Vector3 start = from.GetComponent<Room>().MakeDoor(dir);
        Vector3 finish = to.GetComponent<Room>().MakeDoor(-dir);

        // Calculates the passage length
        int passegLength = (int)Vector3.Distance(start, finish);

        // Determines the passage direction
        Quaternion turn = Quaternion.LookRotation(dir);

        for (int i = 1; i < passegLength; i++)
        {
            int index = Random.Range(0, passageSegments.Length - 1);
            Instantiate(passageSegments[index], start + dir * i, turn);
        }
    }
}
