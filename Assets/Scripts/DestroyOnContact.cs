using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    [Tooltip("How long should this object survive after collision (to allow for deathFX)")]
    public float postContactSurvivalTime;

    public ParticleSystem deathVFX;

    public void OnTriggerEnter(Collider t_collider)
    {
        print("Colliding with: " + t_collider.gameObject.name);
        Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(this.gameObject, postContactSurvivalTime);
    }
}