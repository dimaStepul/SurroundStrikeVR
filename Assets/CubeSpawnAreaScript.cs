using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawnAreaScript : MonoBehaviour
{
    public GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerExit(Collider other)
    {
        // When cube leaves
        if (other.gameObject.layer == 6) {
            Instantiate(cube, transform.position, transform.rotation);
        }
    }
}
