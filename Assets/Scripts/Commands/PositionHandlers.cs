using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Commands;
using SceneProvider;
using System.Linq;
using System;

public class PositionHandlers : MonoBehaviour
{
    Vector3 MyDir;
    public string handlesLayerName = "handles";
    Vector3 StartPos;
    Dictionary<int, Vector3> ObjStartPos = new Dictionary<int, Vector3>();
    Vector3 EndPos;
    Vector3 StartMousePos;
    bool selected = false;
    void Update()
    {
        transform.localScale = Vector3.one * (transform.position - (new Plane(Camera.main.transform.forward, Camera.main.transform.position).ClosestPointOnPlane(transform.position))).magnitude;
        if (!selected)
        {
            if (SceneData.Targets.Count == 0) {
                //Hide
                //to do:         normal hide
                transform.position = new Vector3(-1000, -1000, -100000);
            }
            else
            {
                //Show
                Vector3 pos = Vector3.zero;
                foreach(var obj in SceneData.Targets)
                {
                    pos += obj.transform.position;
                }
                pos /= SceneData.Targets.Count;
                transform.position = pos;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (selected)
            {
                Release();
            }
        }
        if (Input.GetMouseButtonDown(0)) {
            RayCast();
        }
        if (selected) {
            foreach (var obj in SceneData.Targets)
            {
                obj.transform.position = ObjStartPos[Convert.ToInt32(obj.name)] + GetMousePoint() - StartMousePos;
            }
            transform.position = StartPos + GetMousePoint() - StartMousePos;
        }
    }

    void RayCast() {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray, 1000, 1 << 3);
        if (hits.Length == 0)
        {
            return;
        }

        var target = hits
            .OrderBy(h => Vector3.Distance(Camera.main.transform.position, h.point))
            .First().collider;
        if (target.name == "X" || target.name == "Y" || target.name == "Z")
        {
            if (target.name == "X")
                MyDir = Vector3.right;
            if (target.name == "Y")
                MyDir = Vector3.up;
            if (target.name == "Z")
                MyDir = Vector3.forward;
            StartPos = transform.position;
            selected = true;
            StartMousePos = GetMousePoint();
            foreach (var obj in SceneData.Targets)
            {
                ObjStartPos[Convert.ToInt32(obj.name)] = obj.transform.position;
            }
        }
    }

    Vector3 GetMousePoint()
    {
        Vector3 Normal1 = new Vector3(MyDir.y, MyDir.z, MyDir.x);
        Vector3 Normal2 = new Vector3(MyDir.z, MyDir.x, MyDir.y);
        Plane P1 = new Plane(Normal1, transform.position);
        Plane P2 = new Plane(Normal2, transform.position);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (P1.Raycast(ray, out float hit))
        {
            Vector3 NewPos = ray.origin + ray.direction * hit;
            NewPos = P2.ClosestPointOnPlane(NewPos);
            return NewPos;
        }
        else
        {
            return StartPos;
        }

    }

    void Release()
    {
        foreach (var obj in SceneData.Targets)
        {
            obj.transform.position = ObjStartPos[Convert.ToInt32(obj.name)];
        }
        selected = false;
        EndPos = transform.position;
        SceneData.RequestQueue.Enqueue(new TransformCommand(SceneData.Targets, EndPos - StartPos));
    }
}
