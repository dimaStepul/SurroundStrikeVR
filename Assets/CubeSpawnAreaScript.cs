using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawnAreaScript : MonoBehaviour
{
    public GameObject cube;
    private int cubeCount = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (cubeCount < 2)
        {
            SpawnCube();
        }
    }

    void SpawnCube()
    {
        if (cubeCount > 1)
        {
            return;
        }

        GameObject newCube = Instantiate(cube, transform.position, transform.rotation);
        newCube.tag = "Cylinder";
        newCube.AddComponent<WeaponBehavior>();
        cubeCount++;
    }

    void OnTriggerExit(Collider other)
    {
        // When cube leaves
        if (other.gameObject.layer == 6)
        {
            SpawnCube();
        }
    }

    public void DecreaseCubeCount()
    {
        if (cubeCount > 0)
        {
            cubeCount--;
        }
    }
}