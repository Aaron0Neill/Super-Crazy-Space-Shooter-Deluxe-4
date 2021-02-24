using System.Collections;
using UnityEngine;

public class DuckController : MonoBehaviour
{
    public float speed;
    // The point the cube will rotate around
    // They represent the middle point of each 4 bottom edges of the cube
    Vector3 forwardRotationPoint;
    Vector3 backRotationPoint;
    Vector3 leftRotationPoint;
    Vector3 rightRotationPoint;
    Bounds bounds;
    bool rolling;
    Rigidbody body;

    void Start()
    {
        bounds = GetComponent<MeshRenderer>().bounds;
        // Compute the rotation points
        forwardRotationPoint = new Vector3(0, bounds.extents.y - (bounds.size.y / 2), bounds.extents.z);
        backRotationPoint = new Vector3(0, bounds.extents.y - (bounds.size.y / 2), -bounds.extents.z);
        leftRotationPoint = new Vector3(-bounds.extents.x, bounds.extents.y - (bounds.size.y / 2), 0);
        rightRotationPoint = new Vector3(bounds.extents.x, bounds.extents.y - (bounds.size.y / 2), 0);
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Make sure you are not already rolling / moving
        if (rolling)
            return;

        int num = Random.Range(0, 3);
        // Rotate around forward point when pressing the up button
        if (num == 0)
            StartCoroutine(Roll(forwardRotationPoint));
        // Rotate around back point when pressing the down button
        else if (num == 1)
            StartCoroutine(Roll(backRotationPoint));
        // Rotate around left point when pressing the left button
        else if (num == 2)
            StartCoroutine(Roll(leftRotationPoint));
        // Rotate around right point when pressing the right button
        else if (num == 3)
            StartCoroutine(Roll(rightRotationPoint));
    }

    // Make the cube roll around given rotation point
    private IEnumerator Roll(Vector3 rotationPoint)
    {
        // Compute the real rotation point according to current position
        Vector3 point = transform.position + rotationPoint;
        // Compute an axis to rotate in the correct direction
        Vector3 axis = Vector3.Cross(Vector3.up, rotationPoint).normalized;
        float angle = 180;
        float a = 0;
        // Prevent the user from rolling since we already are
        rolling = true;

        while (angle > 0)
        {
            // Compute the angle and rotate the cube around the point
            a = Time.deltaTime * speed;
            transform.RotateAround(point, axis, a);
            // Keep track of the remaining angle
            angle -= a;
            yield return null;
        }
        // Adjust the rotation to make sure the cube rotates **exactly** 90°
        transform.RotateAround(point, axis, angle);

        // Allow the user to roll in a new direction
        rolling = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            Destroy(this.gameObject);
        }
    }
}