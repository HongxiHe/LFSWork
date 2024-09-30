using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathManager : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    public List<wayPoint> path;

    public GameObject prefab;
    int currentPointIndex = 0;

    public List<GameObject> prefabPoints;

    public wayPoint GetNextTarget()
    {
        int nextPointIndex = (currentPointIndex + 1) % (path.Count);
        currentPointIndex = nextPointIndex;
        return path[nextPointIndex];
    }

    public List<wayPoint> GetPath()
    {
        if (path == null)
            path = new List<wayPoint>();

        return path;
    }

    public void CreatAddPoint()
    {
        wayPoint go = new wayPoint();
        path.Add(go);
    }



    // Start is called before the first frame update
    void Start()
    {
        prefabPoints = new List<GameObject>();
        foreach (wayPoint p in path)
        {
            GameObject go = Instantiate(prefab);
            go.transform.position = p.pos;
            prefabPoints.Add(go);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < path.Count; i++)
        {
            wayPoint p = path[i];
            GameObject g = prefabPoints[i];
            g.transform.position = p.pos;
        }
    }
}
