using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {

    public Vector3 origin;
    public GameObject arena;
    public int teleportRate;
    public bool logData;
    private int frameCount;
    private float collisionRadius;
    //private BoxCollider collider;

	// Use this for initialization
	void Start () {
        frameCount = 0;
        Random.InitState((int)Input.mousePosition.x);
        collisionRadius = Mathf.Max(new float[] { gameObject.GetComponent<BoxCollider>().bounds.size.x,
                                gameObject.GetComponent<BoxCollider>().bounds.size.z});
        //collider = gameObject.GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {

        if ((frameCount > teleportRate) && (teleportRate > 0))
        {
            //teleport
            float randTheta = Random.Range(0.0f, 359.0f);
            
            gameObject.transform.position = GetRandomLocation();
            gameObject.transform.Rotate(new Vector3(0.0f, randTheta, 0.0f));

            //LOG DATA!
            if (logData) {
                //LogData()
            };

            frameCount = 0;
        }
        frameCount++;
    }

    private Vector3 GetRandomLocation()
    {
        int ignoreFloorMask = ~(1 << 9);
        float randX = Random.Range(0.2f, 5.8f);
        float randZ = Random.Range(0.2f, 3.8f);
        float trueY = gameObject.transform.position.y;
        Vector3 newPos = new Vector3(randX, trueY, randZ);
        if(Physics.OverlapSphere(newPos, collisionRadius, ignoreFloorMask).Length > 0)
        {
            return GetRandomLocation();
        }
        else
        {
            return newPos;
        }
    }
}
