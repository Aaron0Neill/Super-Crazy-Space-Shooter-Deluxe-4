using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    SurfaceAttractor _surface;
    void Start()
    {
        if (GameObject.Find("Sphere"))
            _surface = GameObject.Find("Sphere").GetComponent<SurfaceAttractor>();
    }

///////////////////////////////////////////////////////////////

    void FixedUpdate()
    {
        if (_surface)
        {
            // Increase gravity force as we get further from the center to lock us into orbit
            float gravityFactor = ((_surface.GetCenter() - transform.position).magnitude - _surface.GetRadius());

            // Apply gravity
            _surface.Attract(transform, gravityFactor);
        }     
    }
}