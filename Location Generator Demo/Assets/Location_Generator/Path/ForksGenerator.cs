using System.Collections;
using UnityEngine;

public class ForksGenerator : MonoBehaviour
{
    [Header("Forks config")]

    public int forkProbability;
    public int maxForkLength;

    MainPathGenerator mainPath;

    private void Start()
    {
        mainPath = GetComponent<MainPathGenerator>();

        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
        MakeForks();
    }

    void MakeForks()
    {
        for (int i = 1; i < mainPath.roomGrid.GetLength(0) - 1; i++)
        {
            // Decides whether to create a fork
            if (Random.Range(0, 100) > forkProbability) continue;

            // Decides the length of the fork
            int newforkLength = Random.Range(1, maxForkLength);

            for (int j = 0; j < newforkLength; j++)
            {
                // Finds a free position for a new room
                Vector3 newForkPlace =
                    mainPath.FindPlaceForRoom(mainPath.roomGrid[i, j].transform.position);

                // Creates the room
                mainPath.roomGrid[i, j + 1] =
                    Instantiate(mainPath.GetRandomRoom(), newForkPlace, Quaternion.identity);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (mainPath == null || mainPath.roomGrid == null) return;

        Gizmos.color = Color.red;

        for (int i = 0; i < mainPath.roomGrid.GetLength(0); i++)
        {
            // Looks for a fork in the grid
            if (mainPath.roomGrid[i, 1] == null) continue;

            int j = 0;
            while (mainPath.roomGrid[i, j + 1] != null)
            {
                Gizmos.DrawLine(
                    mainPath.roomGrid[i, j].transform.position,
                    mainPath.roomGrid[i, j + 1].transform.position
                );

                j++;
            }
        }
    }
}
