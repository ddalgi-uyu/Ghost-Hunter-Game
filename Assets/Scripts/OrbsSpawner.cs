using UnityEngine;
using Meta.XR.MRUtilityKit;
using System.Collections.Generic;
using System.Collections;

public class OrbsSpawner : MonoBehaviour
{
    public int numberOfOrbsToSpawn = 5;
    public GameObject orbPrebab;
    public float height;

    public List<GameObject> spawnedOrbs;

    public int maxNumberOfTry = 100;
    public int currentNumberOfTry = 0;

    public static OrbsSpawner instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MRUK.Instance.RegisterSceneLoadedCallback(SpawnOrbs);
    }

    public void DestroyOrb(GameObject orb)
    {
        spawnedOrbs.Remove(orb);
        Destroy(orb);

        if(spawnedOrbs.Count == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    public void SpawnOrbs()
    {
        for (int i = 0; i < numberOfOrbsToSpawn; i++)
        {
            Vector3 randomPosition = Vector3.zero;

            MRUKRoom room = MRUK.Instance.GetCurrentRoom();

            while(currentNumberOfTry < maxNumberOfTry)
            {
                bool hasFound = room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.FACING_UP, 1, new LabelFilter(MRUKAnchor.SceneLabels.FLOOR), out randomPosition, out Vector3 n);

                if (hasFound)
                    break;

                currentNumberOfTry++;
            }

            randomPosition.y = height;

            GameObject spwned = Instantiate(orbPrebab, randomPosition, Quaternion.identity);

            spawnedOrbs.Add(spwned);
        }
    }

}
