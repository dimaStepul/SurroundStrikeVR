using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject); // This will destroy the weapon after collision
            CubeSpawnAreaScript.DecreaseCubeCount();
        }
        // if (collision.gameObject.CompareTag("Enemy"))
        // {
        //     Destroy(collision.gameObject, 2.0f);
        //     Destroy(gameObject, 2.0f); // This will destroy the weapon after collision
        // }
    }
}