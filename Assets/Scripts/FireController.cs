using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    private GameObject _audioManager;

    [Tooltip("What should we fire?")]
    public GameObject projectile;

    [Tooltip("At what speed should our projectiles move at?")]
    public float projectileSpeed;

    [Tooltip("What's the minimum time (in seconds) we should wait between shots?")]
    public float fireDelay;

    // Track the time at which we fired our last shot
    float timeOfLastShot;

    // If we're on a plane, we lock the Y pos of our projectile
    public bool isPlane = false;

    private void Start()
    {
        _audioManager = GameObject.FindGameObjectWithTag("AudioManager");
    }

    public bool Fire(Vector3 t_target)
    {
        if (Time.time > (timeOfLastShot + fireDelay))
        {
            timeOfLastShot = Time.time;

            // Determine the vector this shot should follow
            Vector3 shotVector = (t_target - transform.position);
            if (isPlane) shotVector.y = 0.0f;
            shotVector.Normalize();

            // Instantiate our projectile
            GameObject p = Instantiate(projectile, transform.position, Quaternion.LookRotation(shotVector, Vector3.up));
            _audioManager.GetComponent<AudioManager>().Play("Player_Bullet");

            // Add force to our projectile
            p.GetComponent<Rigidbody>().AddForce(shotVector * projectileSpeed, ForceMode.Impulse);

            // Ignore collisions with the parent
            Physics.IgnoreCollision(p.GetComponent<Collider>(), GetComponent<Collider>());

            // Ignore the raycast layer
            Physics.IgnoreCollision(p.GetComponent<Collider>(), GameObject.Find("MouseRaycastCollider").GetComponent<Collider>());

            // Ignore collisions with the terrain
            GameObject[] terrain = GameObject.FindGameObjectsWithTag("PlaySurface");

            foreach(GameObject t in terrain)
                Physics.IgnoreCollision(p.GetComponent<Collider>(), t.GetComponent<Collider>());

            return true;
        }
        else
        {
            return false;
        }
    }
}