using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lidar : MonoBehaviour
{
    //private Vector3 _lidarCenter;
    public float range_m; //ToDo: Default values
    public float sweep_deg;
    public float beam_count;
    public Vector3 lidar_offest;
    private float _step_deg;
    private Vector3 _lidar_center;
    private List<float> _scan_data;
    private List<GameObject> _beams;
    private float _first_beam;
    public bool render_beams;
    
    // Use this for initialization
    void Start()
    {
        _step_deg = sweep_deg / (beam_count-1);
        _scan_data = new List<float>();
        _beams = new List<GameObject>();
        render_beams = true;

        //InitLasers();
        _first_beam = -sweep_deg / 2.0f;
        for (int i = 0; i< beam_count; i++)
        {
            _beams.Add(GetNewLaserBeam());
        }
    }

    private GameObject GetNewLaserBeam()
    {
        GameObject newBeam = new GameObject();
        newBeam.AddComponent<LineRenderer>();
        newBeam.GetComponent<LineRenderer>().numPositions = 2;
        newBeam.GetComponent<LineRenderer>().startWidth = 0.005f;
        newBeam.GetComponent<LineRenderer>().endWidth = 0.005f;
        return newBeam;
    }

    private void RenderBeams(int beam_idx, Vector3 origin, Vector3 destination)
    {
        _beams[beam_idx].GetComponent<LineRenderer>().SetPosition(0, origin);
        _beams[beam_idx].GetComponent<LineRenderer>().SetPosition(1, destination);
    }

    // Update is called once per frame
    void Update()
    {
        _scan_data.Clear();
        for (int i=0; i<_beams.Count; i++)
        {
            _lidar_center = gameObject.transform.position + lidar_offest;
            Vector3 beam_dir = Quaternion.AngleAxis(_first_beam + (i * _step_deg), transform.up) * gameObject.transform.forward;
            Ray ray = new Ray(_lidar_center, beam_dir);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, range_m))
            {
                _scan_data.Add(hit.distance);
                if (render_beams)
                {
                    RenderBeams(i, _lidar_center, hit.point);
                }else
                {
                    RenderBeams(i, _lidar_center, _lidar_center);
                }
            }
            else
            {
                _scan_data.Add(range_m);
                RenderBeams(i, _lidar_center, _lidar_center);
                
            }
        }
    }
}
