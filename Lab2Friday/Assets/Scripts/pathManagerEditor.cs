using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(pathManager))]
public class pathManagerEditor : Editor
{

    [SerializeField] pathManager pathManager;
    [SerializeField] List<wayPoint> ThePath;
    List<int> toDelete;

    wayPoint selectedPoint = null;
    bool doRepaint = true;


    private void OnSceneGUI()
    {
        ThePath = pathManager.GetPath();
        DrawPath(ThePath);
    }

    private void OnEnable()
    {
        pathManager = target as pathManager;
        toDelete = new List<int>();
    }

    //-------------

    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();
        ThePath = pathManager.GetPath();

        base.OnInspectorGUI();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Path");

        DrawGUIForPoints();

        if (GUILayout.Button("Add point to path"))
        {
            pathManager.CreatAddPoint();
        }

        EditorGUILayout.EndVertical();
        SceneView.RepaintAll();
    }

    void DrawGUIForPoints()
    {
        if (ThePath != null && ThePath.Count > 0)
        {
            for (int i = 0; i < ThePath.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                wayPoint p = ThePath[i];

                Color c = GUI.color;
                if (selectedPoint == p)
                {
                    GUI.color = Color.green;
                }
                Vector3 oldPos = p.GetPos();
                Vector3 newPos = EditorGUILayout.Vector3Field("", oldPos);

                if (EditorGUI.EndChangeCheck()) p.SetPos(newPos);

                if (GUILayout.Button("-", GUILayout.Width(25)))
                {
                    toDelete.Add(i);
                }

                GUI.color = c;
                EditorGUILayout.EndHorizontal();
            }
        }
        if (toDelete.Count > 0)
        {
            foreach (int i in toDelete)
                ThePath.RemoveAt(i);
            toDelete.Clear();
        }
    }

    //--------------------------------
    public void DrawPath(List<wayPoint> path)
    {      
        if (path != null)
        {
            int current = 0;
            foreach (wayPoint wp in path)
            {
                //draw current 
                doRepaint = DrawPoint(wp);
                int next = (current + 1) % path.Count;
                wayPoint wpnext = path[next];
                //connect this one to next
                DrawPathLine(wp, wpnext);
                current += 1;
            }
            if (doRepaint) Repaint();
        }       
    }

    public void DrawPathLine(wayPoint p1, wayPoint p2)
    {
        Color c = Handles.color;
        Handles.color = Color.gray;
        Handles.DrawLine(p1.GetPos(), p2.GetPos());
        Handles.color = c;
    }

    public bool DrawPoint(wayPoint p)
    {       
            bool isChanged = false;
            if (selectedPoint == p)
            {
                Color c = Handles.color;
                Handles.color = Color.green;   //color

                EditorGUI.BeginChangeCheck();
                Vector3 oldpos = p.GetPos();
                Vector3 newpos = Handles.PositionHandle(oldpos, Quaternion.identity);

                float handleSize = HandleUtility.GetHandleSize(newpos);
                Handles.SphereHandleCap(-1, newpos, Quaternion.identity, 0.4f * handleSize, EventType.Repaint);
                if (EditorGUI.EndChangeCheck())
                {
                    p.SetPos(newpos);
                }
                Handles.color = c;
            }
            else
            {
                Vector3 currPos = p.GetPos();
                float handleSize = HandleUtility.GetHandleSize(currPos);
                if (Handles.Button(currPos, Quaternion.identity, 0.25f * handleSize, 0.25f * handleSize, Handles.SphereHandleCap))
                {
                    isChanged = true;
                    selectedPoint = p;
                }
            }
            return isChanged;      
    }



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
