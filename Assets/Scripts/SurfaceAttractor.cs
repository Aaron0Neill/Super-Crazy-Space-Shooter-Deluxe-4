using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceAttractor : MonoBehaviour
{
    public bool _isPlane = false;
    public float gravity = -10.0f;

    public void Attract(Transform t_entityTransform, float t_strength)
    {
        if (_isPlane)
            return;

        // Get the normal vector from the surface through the entity
        Vector3 normal = SurfaceNormal(t_entityTransform.position);
        Vector3 localUp = t_entityTransform.up;

        // Apply gravity force towards our object
        t_entityTransform.GetComponent<Rigidbody>().AddForce(normal * gravity * t_strength);

        // Determine the rotation such that the entity is flat to our surface
        Quaternion targetRotation = Quaternion.FromToRotation(localUp, normal) * t_entityTransform.rotation;

        // Lerp between the current rotation and our target rotation
        t_entityTransform.rotation = Quaternion.Slerp(t_entityTransform.rotation, targetRotation, 50.0f * Time.deltaTime);
    }

    public Vector3 ClosestPoint(Vector3 entityPos)
    {
        // Returns the closest point on our surface to the entity
        return this.GetComponent<Collider>().ClosestPoint(entityPos);
    }

    public Vector3 SurfaceNormal(Vector3 entityPos)
    {
        // Returns the surface normal at the entity's current position
        return (ClosestPoint(entityPos) - transform.position).normalized;
    }

    public Vector3 GetCenter()
    {
        return transform.position;
    }

    public float GetRadius()
    {
        return transform.localScale.y;
    }

    public bool isPlane()
    {
        return _isPlane;
    }
}