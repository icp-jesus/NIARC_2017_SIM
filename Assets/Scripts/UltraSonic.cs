using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltraSonic: MonoBehaviour
{
    //private Vector3 _lidarCenter;
    public float range_m; //ToDo: Default values
    private float _data;
    private GameObject _pulse;
    public bool render_pulse;
    public Material pulseColor;

    // Use this for initialization
    void Start()
    {
        _pulse = new GameObject();
        render_pulse = true;

        _pulse.AddComponent<LineRenderer>();
        _pulse.GetComponent<LineRenderer>().numPositions = 2;
        _pulse.GetComponent<LineRenderer>().startWidth = 0.005f;
        _pulse.GetComponent<LineRenderer>().endWidth = 0.005f;
        _pulse.GetComponent<LineRenderer>().material = pulseColor;
    }

    private void RenderPulse(Vector3 origin, Vector3 destination)
    {
        _pulse.GetComponent<LineRenderer>().SetPosition(0, origin);
        _pulse.GetComponent<LineRenderer>().SetPosition(1, destination);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 center = gameObject.transform.position;
        Vector3 pulse_dir = gameObject.transform.forward;
        Ray ray = new Ray(center, pulse_dir);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range_m))
        {
            _data = hit.distance;
            if (render_pulse)
            {
                RenderPulse(center, hit.point);
            }
            else
            {
                RenderPulse(center, center);
            }
        }
        else
        {
            _data = range_m;
            RenderPulse(center, center);

        }
    }
}
