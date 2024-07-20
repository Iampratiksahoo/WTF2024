using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieCreator : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("hello running here...");
        var zombie = other.GetComponent<IZombie>();
        if (zombie != null) 
        {
            zombie.Turn();
        }
    }
}
