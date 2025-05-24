using System.Collections.Generic;
using UnityEngine;

public class RoomStateBuilding : RoomState
{
    private List<GameObject> cubes = new List<GameObject>();

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        { 
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 30))
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.position = hit.point;
                cubes.Add(go);
            }
        }
    }

    public override void Destruct()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            Destroy(cubes[i]);
        }

        cubes.Clear();
    }
}
