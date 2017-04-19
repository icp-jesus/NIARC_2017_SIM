using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtentionMethod
{
    public static T LastElement<T>(this List<T> list)
    {
        if (list.Count > 0)
        {
            return list[list.Count - 1];
        }
        return default(T);
    }
}


public class Path : MonoBehaviour {

    public List<GameObject> waypoints;
    public float pathIncrement;
    private List<Vector3> pathPoints;
    private List<GameObject> renderPoints;
	// Use this for initialization
	void Start () {
        pathPoints = new List<Vector3>();
        InterpolatePoints();
        SpawnPath();
	}
	
    public List<Vector3> PathPoints
    {
        get
        {
            return pathPoints;
        }
    }

    public List<GameObject> RenderPoints
    {
        get
        {
            return renderPoints;
        }
    }

    void SpawnPath()
    {
        renderPoints = new List<GameObject>();
        foreach(Vector3 point in pathPoints)
        {
            GameObject newPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newPoint.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            newPoint.transform.position = point;
            Destroy(newPoint.GetComponent<SphereCollider>());
            renderPoints.Add(newPoint);
        }
    }

    void InterpolatePoints()
    {
        int lastIndex = waypoints.Count - 1;
        for (int i=0; i<lastIndex; i++)
        {
            float oposite = waypoints[i + 1].transform.position.z - waypoints[i].transform.position.z;
            float adjacent = waypoints[i + 1].transform.position.x - waypoints[i].transform.position.x;
            float theta = Mathf.Atan2(oposite, adjacent);
            Vector3 step = new Vector3 (
                pathIncrement * Mathf.Cos(theta), 
                0.0f, 
                pathIncrement*Mathf.Sin(theta));
            pathPoints.Add(waypoints[i].transform.position);
            while (distToNext(pathPoints.LastElement(), waypoints[i+1].transform.position) > (pathIncrement/2.0f))
            {
                pathPoints.Add(pathPoints.LastElement() + step);
            }
        }
        SpawnPath();
    }

    float distToNext(Vector3 v1, Vector3 v2)
    {
        return Mathf.Sqrt(Mathf.Pow(v1.x - v2.x, 2) + Mathf.Pow(v1.z - v2.z, 2));
    }

    // Update is called once per frame
    void Update () {
		
	}
}
