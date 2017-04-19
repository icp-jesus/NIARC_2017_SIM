using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowing : MonoBehaviour {

    public Path path;
    public int breadCrumbCount;
    public float deltaTime;
    public float errorFactor;
    public float speedFactor;
    public Material highlight;
    private List<int> breadCrumbIndexes = new List<int>();
    private int cnt = 1;
    private Transform robot;

	// Use this for initialization
	void Start() { 
        robot = gameObject.GetComponentInParent<Transform>();
        GameObject difDrive = GameObject.Find("DifferentialDrive");
        int minLookAheadincrements = (int)Mathf.Ceil(difDrive.GetComponent<DifferentialDrive>().MaxForwardVelocity * deltaTime / path.pathIncrement);
        if (breadCrumbCount < minLookAheadincrements) { breadCrumbCount = minLookAheadincrements; }
        for (int i = 0; i < breadCrumbCount; i++) { breadCrumbIndexes.Add(i); }
        Time.fixedDeltaTime = deltaTime;

        //Vector3 v1 = new Vector3(1.0f, 0.0f, 0.0f);
        //Vector3 v2 = new Vector3(1.0f, 0.0f, 1.0f);
        //Debug.LogError("Theta: " + findDeltaTheta(v1, v2));
        //Debug.LogError("Theta: " + findDeltaTheta(v2, v1));

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public float TurnCommand
    {
        get;
        set;
    }

    public float SpeedCommand
    {
        get;
        set;
    }

    private void FixedUpdate()
    {
        BuildBreadCrumbPath();
        float distToClosestCrumb = DistBetweenVectorsInXZPlane(path.PathPoints[breadCrumbIndexes[0]], gameObject.GetComponentInParent<Transform>().position);
        float pathCurvature = FindPathCurvature(); //speed control
        //float deviationFromPath = FindDeviationFromPath();
        float deltaTheta = findDeltaTheta();
        TurnCommand = errorFactor * deltaTheta;
        //TurnCommand = errorFactor * deviationFromPath;
        SpeedCommand = speedFactor * pathCurvature;
    }

    private float FindDeviationFromPath()
    {
        Vector3 closestPathPointInLocalSpace = WorldToLocal(path.PathPoints[breadCrumbIndexes[0]]);
        return closestPathPointInLocalSpace.x;
    }
    
    private float findDeltaTheta(Vector3 v1, Vector3 v2)
    {
        float ang = Mathf.Atan2(v2.z, v2.x) - Mathf.Atan2(v1.z, v1.x);
        ang = ang < 180 ? ang : 360 - ang;
        return ang;
    }

    private float findDeltaTheta()
    {
        //Vector3 vectFromBotToBreadCrumb = path.PathPoints.LastElement() - robot.transform.position;
        //GameObject closest = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //closest.transform.position = path.PathPoints[breadCrumbIndexes.LastElement()] + new Vector3(0.0f, 0.2f, 0.0f);
        //closest.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //closest.GetComponent<Renderer>().material = highlight;
        Vector3 goal = path.PathPoints[breadCrumbIndexes.LastElement()];
        float atan = Mathf.Atan2((goal.x - robot.transform.position.x), (goal.z - robot.transform.position.z));
        //atan = Mathf.Abs(atan);
        float dTheta = atan - (robot.transform.eulerAngles.y * Mathf.Deg2Rad);
        //Debug.Log("dTheta1: " + dTheta * Mathf.Rad2Deg);
        float sin_dTheta = Mathf.Sin(dTheta);
        float cos_dTheta = Mathf.Cos(dTheta);
        dTheta = Mathf.Atan2(sin_dTheta, cos_dTheta);
        //Debug.Log("Robot: " + robot.transform.eulerAngles.y);
        //Debug.Log("Goal: " + goal);

        
        cnt++;
        if (cnt % 10 == -1) {
            Debug.Log("gx: " + goal.x + ", gz: " + goal.z + ", rx: " + robot.transform.position.x + ", rz: " + robot.transform.position.z);
            Debug.Log("Heading: " + robot.transform.eulerAngles.y);
            Debug.Log("AngToG: " + atan*Mathf.Rad2Deg);
            Debug.Log("dTheta2: " + dTheta * Mathf.Rad2Deg);

        }
        return dTheta;
        //return findDeltaTheta(robot.forward, vectFromBotToBreadCrumb);
    }

    // This is NOT curvature in the strictest sense. It is the size of the offset of the mid-point of the
    // line joining the first and last breadcrumbs. The larger the offset the more curved the path.
    private float FindPathCurvature()
    {
        Vector3 startPoint = path.PathPoints[breadCrumbIndexes[0]];
        Vector3 midPoint =   path.PathPoints[breadCrumbIndexes[(int)(breadCrumbCount / 2)]];
        Vector3 endPoint =   path.PathPoints[breadCrumbIndexes[breadCrumbCount - 1]];
        //Pythagorus
        float side = DistBetweenVectorsInXZPlane(startPoint, endPoint) / 2.0f;
        float hype = DistBetweenVectorsInXZPlane(startPoint, midPoint);

        float curve = Mathf.Sqrt(Mathf.Abs(Mathf.Pow(hype, 2) - Mathf.Pow(side, 2)));
        if (side > 0.99 * hype){ curve = 0.0f; }
         
        if (float.IsNaN(curve))
        {
            //Debug.LogError("side: " + side);
            //Debug.LogError("hype: " + hype);
        }
        return curve;
    }

    private Vector3 WorldToLocal(Vector3 vecInWorldSpace)
    {
        return transform.InverseTransformPoint(vecInWorldSpace);
    }

    private int FindIndexOfClosestPointOnPath()
    {
        float distToClosestCrumb = Mathf.Infinity;
        int idxOfClosestCrumb = breadCrumbIndexes[0];
        foreach(int idx in breadCrumbIndexes)
        {
            float distToCrumb = DistBetweenVectorsInXZPlane(path.PathPoints[idx], gameObject.GetComponentInParent<Transform>().position);
            if (distToCrumb < distToClosestCrumb)
            {
                distToClosestCrumb = distToCrumb;
                idxOfClosestCrumb = idx;
            }
        }
        return idxOfClosestCrumb;
    }

    float DistBetweenVectorsInXZPlane(Vector3 v1, Vector3 v2)
    {
        return Mathf.Sqrt(Mathf.Pow(v1.x - v2.x, 2) + Mathf.Pow(v1.z - v2.z, 2));
    }

    private void BuildBreadCrumbPath()
    {
        breadCrumbIndexes[0] = FindIndexOfClosestPointOnPath();
        for(int i=0; i<breadCrumbIndexes.Count; i++)
        {
            breadCrumbIndexes[i] = breadCrumbIndexes[0] + i;
        }
    }
}
