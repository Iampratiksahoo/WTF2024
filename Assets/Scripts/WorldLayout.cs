using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WorldLayout : MonoBehaviour
{
    public Vector2 gridSize;
    public GameObject ground;

    [ContextMenu("Spawn")]
    void Spawn()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                Transform newObj = Instantiate(ground, transform).transform;
                newObj.position = new Vector3(i, 0, j) * newObj.localScale.x;
            }
        }
    }
}
