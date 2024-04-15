using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawnAreaScript : MonoBehaviour
{
    public GameObject weapon;
    private static int cubeCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        SpawnCube();

    }

    // Update is called once per frame
    void Update()
    {
        if (cubeCount < 1)
        {
            SpawnCube();
        }
    }

    void SpawnCube()
    {
        if (cubeCount >= 4)
        {
            return;
        }

        Vector3 spawnPosition = transform.position + new Vector3(0, cubeCount, 0);
        GameObject newWeapon = Instantiate(weapon, spawnPosition, transform.rotation);
        newWeapon.tag = "Cylinder";
        newWeapon.AddComponent<WeaponBehavior>();
        cubeCount++;
    }

    void OnTriggerExit(Collider other)
    {
        // When cube leaves
        // if (other.gameObject.layer == 6)
        // {
        //     SpawnCube();
        // }
    }

    public static void DecreaseCubeCount()
    {
        if (cubeCount > 0)
        {
            cubeCount--;
        }
    }

    void OnEnable()
    {
        cubeCount = 0;
    }
}