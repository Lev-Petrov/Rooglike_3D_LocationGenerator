using UnityEngine;

public class RandomController : MonoBehaviour
{
    [Header("Random config")]
    [SerializeField] private int seed;
    [SerializeField] private bool useRandomSeed;

    void Awake()
    {
        if (useRandomSeed)
        {
            // Generates a random seed
            seed = Random.Range(0, 100000); // or Random.Range(0, 10000)
        }

        // Initializes the random state
        Random.InitState(seed);

        Debug.Log("Seed used: " + seed);
    }
}
