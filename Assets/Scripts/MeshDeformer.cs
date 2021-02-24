using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/////////////////////////////////////////////////////////////////
// CREDIT: This script was inspired by the tutorial found here //
// https://catlikecoding.com/unity/tutorials/mesh-deformation/ //
/////////////////////////////////////////////////////////////////

public class MeshDeformer : MonoBehaviour
{
    Mesh _mesh;
    Vector3[] originalVertices, displacedVertices;
    Vector3[] vertexVelocities;

    public float forceOffset = 0.1f;
    public float springForce = 20.0f;
    public float damping = 5.0f;

    float uniformScale = 1.0f;

    void Start()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = _mesh.vertices;
        displacedVertices = _mesh.vertices;
        vertexVelocities = new Vector3[_mesh.vertices.Length];
    }

///////////////////////////////////////////////////////////////


    public void Deform(Vector3 t_point, float t_force)
    {
        // Move the point out from our surface so that we push vertices in rather than apart
        t_point += (t_point - transform.position).normalized * forceOffset;

        // Transform the position from world space to local space
        t_point = transform.InverseTransformPoint(t_point);

        for (int i = 0; i < displacedVertices.Length; ++i)
        {
            AddForceToVertex(i, t_point, t_force);
        }
    }

    void AddForceToVertex(int i, Vector3 t_point, float t_force)
    {
        // The distance from the point of the original force to this vertex
        Vector3 pointToVertex = displacedVertices[i] - t_point;
        pointToVertex *= uniformScale;

        // We use the inverse square law to determine the force dropoff at this point
        float attenuatedForce = t_force / (1f + pointToVertex.sqrMagnitude);

        // dV = FdT
        float velocity = attenuatedForce * Time.deltaTime;

        // Apply the velocity in the direction of the delta pos
        vertexVelocities[i] += pointToVertex.normalized * velocity;
    }

    void Update()
    {
        uniformScale = transform.localScale.x;

        for (int i = 0; i < displacedVertices.Length; ++i)
        {
            UpdateVertex(i);
        }

        _mesh.vertices = displacedVertices;
        _mesh.RecalculateNormals();
    }

    void UpdateVertex(int i)
    {
        Vector3 velocity = vertexVelocities[i];
        Vector3 displacement = displacedVertices[i] - originalVertices[i];
        displacement *= uniformScale;
        velocity -= displacement * springForce * Time.deltaTime;
        velocity *= 1f - damping * Time.deltaTime;
        vertexVelocities[i] = velocity;
        displacedVertices[i] += velocity * (Time.deltaTime / uniformScale);
    }
}