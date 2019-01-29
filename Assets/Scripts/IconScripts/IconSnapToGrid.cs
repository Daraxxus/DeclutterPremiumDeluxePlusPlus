using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconSnapToGrid : MonoBehaviour {
    public Grid grid { get; private set; }
    public Vector3 CurrentSnapPosition { get; private set; }

	// Use this for initialization
	public void Init () {
        if (grid == null) grid = FindObjectOfType<Grid>();
        if (grid == null) throw new System.ArgumentNullException("Grid not found, please ensure grid is in the scene.");
        CurrentSnapPosition = Vector3.zero;
	}

    public bool TrySnapIcon ()
    {
        Vector3 positionToSnap = grid.GetNearestPointOnGrid(transform.position);
        if (grid.TryRegisterIconToPosition(gameObject, positionToSnap))
        {
            transform.position = positionToSnap;
            CurrentSnapPosition = positionToSnap;
            return true;
        }

        return false;
    }
}
