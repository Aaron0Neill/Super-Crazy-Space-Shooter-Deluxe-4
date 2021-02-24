using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private GameObject _audioManager;

    [Tooltip("A reference to the play surface that we should stick to")]
    public GameObject playSurface;
        
    private SurfaceAttractor _surfaceAttractor;
    private MeshDeformer _surfaceMesh;

    public float accelerationForce = 20.0f;
    private Vector3 _respawnPoint;
    public Material _burnMat;
    private Rigidbody _rb;
    private bool _alive = true;
    private Animator _anim;
    [SerializeField] private float _burn = -0.6f; // used to set the dissolve value of the shader on the player (-0.6 = invisible, 1 = visible)

    // Keep track of whether our play surface is a plane or a 3D object
    // (We can achieve more realistic movement on a plane)
    private bool _isPlane;

    ///////////////////////////////////////////////////////////////

    void Start()
    {
        _surfaceAttractor = playSurface.GetComponent<SurfaceAttractor>();
        _surfaceMesh = playSurface.GetComponent<MeshDeformer>();

        _audioManager = GameObject.FindGameObjectWithTag("AudioManager");

        _rb = this.GetComponent<Rigidbody>();

        _isPlane = _surfaceAttractor.isPlane();

        // Ignore the raycast layer
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("MouseRaycastCollider").GetComponent<Collider>());

        _anim = GetComponent<Animator>();
        _anim.SetBool("_Alive", _alive);
        _respawnPoint = new Vector3( 0, 0, 0 );
    }

    ///////////////////////////////////////////////////////////////

    void Update()
    {
        if (_alive)
        {
            if (Input.GetButton("Fire1"))
                Fire();
        }

        _burnMat.SetFloat("_DissolveValue", _burn);
    }

    void Fire()
    {
        Vector3 targetPos = new Vector3();

        // Cast a ray out into the scene in the direction of the mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // We only want our ray to collide with layer 9
        int layerMask = 1 << 9;

        if (Physics.Raycast(ray, out hit, layerMask))
            targetPos = (hit.point);

        GetComponent<FireController>().Fire(targetPos);

        // Add a deformation force to our mesh
        _surfaceMesh.Deform(transform.position, 200.0f);
    }

    ///////////////////////////////////////////////////////////////

    void FixedUpdate()
    {
        // Don't update movement if we're dead
        if (!_alive)
            return;

        // Determine the desired input vector
        Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        if (_isPlane)
        {
            _rb.AddForce(inputVector * accelerationForce, ForceMode.Acceleration);

            // Determine the signed angle between our velocity and our models forward axis on the XZ-plane
            float yaw = Vector3.SignedAngle(_rb.velocity, Vector3.forward, Vector3.down);

            // Determine the roll as a function of the difference between our desired direction and our current direction
            float roll = Vector3.SignedAngle(_rb.velocity.normalized, inputVector, Vector3.down);

            _rb.transform.eulerAngles = new Vector3(0.0f, yaw, roll);
        }
        else
        {
            // Apply gravity
            if (_surfaceAttractor)
                _surfaceAttractor.Attract(transform, 1.0f);

            // Map the input vector to the camera space (So that up always moves the player up from the point of view of the camera)
            Vector3 direction = ((Camera.main.transform.right * inputVector.x) + (Camera.main.transform.up * inputVector.z));

            // Apply the movement force to our rigidbody
            _rb.AddForce(direction * accelerationForce, ForceMode.Acceleration);

            // If the sqrMagnitude of our velocity is below 4, just look toward our previous forward vector
            Vector3 lookAt = _rb.velocity.sqrMagnitude > 5.0f ? _rb.velocity.normalized : transform.forward;

            // Lock our forward vector to our velocity, and our up vector to the surface normal
            transform.rotation = Quaternion.LookRotation(lookAt, _surfaceAttractor.SurfaceNormal(transform.position));
        }
    }

    ///////////////////////////////////////////////////////////////
    
    private void OnCollisionEnter(Collision collision)
    {
        // Don't check collisions if we're dead
        if (!_alive)
            return;

        if (collision.gameObject.CompareTag("Enemy"))
        {

            death();
        }
    }

    ///////////////////////////////////////////////////////////////

    void death()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().PlayerDied();
        _alive = false;
        _audioManager.GetComponent<AudioManager>().Play("Player_Death");
        _anim.SetBool("_Alive", _alive);
        StartCoroutine(AnimationCoroutine());
    }

    ///////////////////////////////////////////////////////////////

    IEnumerator AnimationCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        this.transform.position = _respawnPoint;
        yield return new WaitForSeconds(1.5f);
        _alive = true;
        _anim.SetBool("_Alive", _alive);
        Physics.SyncTransforms();
    }
}