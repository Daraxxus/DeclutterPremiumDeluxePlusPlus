using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private List<Vector3> gridSpaces = new List<Vector3>();

    [SerializeField] [Range(0, 5)] private float size = 1f;
    public float Size { get { return size; } }

    //Debug
    [SerializeField] [Range(10, 50)] private int numberOfDebugSpheresX = 10;
    [SerializeField] [Range(10, 50)] private int numberOfDebugSpheresY = 10;

    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;

        int xCount = Mathf.RoundToInt(position.x / size);
        int yCount = Mathf.RoundToInt(position.y / size);

        Vector3 result = new Vector3((float)xCount * size, (float)yCount * size, 1.0f);

        result += transform.position;
        return result.With(z: 1);
    }

    public bool CheckSpotAvailability(Vector3 position)
    {
        return !gridSpaces.Contains(GetNearestPointOnGrid(position));
    }

    public bool TryRegisterIconToPosition(GameObject gameObjectToRegister, Vector3 position)
    {
        if (!gridSpaces.Contains(position))
        {
            gridSpaces.Add(position);
            return true;
        }


        return false;
    }

    public bool TryUnregisterIconOnPosition(Vector3 position)
    {
        if (gridSpaces.Contains(position))
        {
            gridSpaces.Remove(position);
            return true;
        }

        return false;
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;

    //    for (float x = 0; x < numberOfDebugSpheresX; x += size)
    //    {
    //        for (float y = 0; y < numberOfDebugSpheresY; y += size)
    //        {
    //            Vector3 point = GetNearestPointOnGrid(new Vector3(x, y, 1));
    //            Gizmos.DrawSphere(transform.position + point, 0.1f);
    //        }
    //    }
    //}

}
