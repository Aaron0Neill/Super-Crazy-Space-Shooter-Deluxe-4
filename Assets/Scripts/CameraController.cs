using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("This is the script attached to the current playing surface which we should point our camera at")]
    public SurfaceAttractor playSurface;

    [Tooltip("How much should the camera's position be offset towards the " +
    "secondary target (0.0f = on the player, 1.0f = on the target, 0.5f = middle")]
    public float cameraTargetBias = 0.2f;

    [Tooltip("How long does the camera take to get to it's new position")]
    public float cameraSmoothingTime = 0.3f;

    [Tooltip("Height of the camera above origin")]
    public float cameraHeight = 10.0f;

    [Tooltip("Is the play surface a plane rather than a 3D object? (simpler)")]
    public bool isPlane = false;

    // This is used only for internal calculations
    // by the Vector3 'SmoothDamp' function
    Vector3 _cameraSmoothingVelocity;

    Transform _playerTransform;

    Vector3 _target;

    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    ///////////////////////////////////////////////////////////////

    void FixedUpdate()
    {
        Vector3 playerPosition = _playerTransform.position;
        Vector3 mousePos = playerPosition;

        // We only want our ray to collide with layer 9
        int layerMask = 1 << 9;

        // Cast a ray out into the scene in the direction of the mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, layerMask))
            mousePos = hit.point;

        Vector3 distanceBetweenTargets = mousePos - playerPosition;

        // Get new camera position and clamp it's height
        _target = playerPosition + (distanceBetweenTargets * cameraTargetBias);

        if (isPlane)
        {
            // Clamp the camera's height height
            _target.y = cameraHeight;

            // Smoothly interp between the current position and our new calculated position
            transform.position = Vector3.SmoothDamp(transform.position, _target, ref _cameraSmoothingVelocity, cameraSmoothingTime);
        }
        else
        {
            // Our camera should align itself with the normal vector of the plane directly below us
            Vector3 newCameraPos = _target = playSurface.ClosestPoint(_target);
            newCameraPos += playSurface.SurfaceNormal(_target) * cameraHeight;

            // Smoothly interp between the current position and our new calculated position
            transform.position = Vector3.SmoothDamp(transform.position, newCameraPos, ref _cameraSmoothingVelocity, cameraSmoothingTime);

            // Keep our camera pointing at the center of the object, and oriented upwards (relative to ourselves)
            transform.rotation = Quaternion.LookRotation((playSurface.GetCenter() - transform.position).normalized, transform.up);
        }   
    }
}