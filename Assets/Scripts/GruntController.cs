using UnityEngine;

public class GruntController : MonoBehaviour
{
    GameObject _audioManager;
    GameController _controller;

    //Object to follow
    public Transform target;

    public float speed = 7.0f;
    //Distance to start moving
    public float minDistance = 0.09f;

    //Rigidbody to move
    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _audioManager = GameObject.FindGameObjectWithTag("AudioManager");
        _controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        target = GameObject.Find("Player").transform;
    }

    void FixedUpdate()
    {
        //Find direction
        Vector3 dir = (target.transform.position - _rb.transform.position).normalized;
        //Check if we need to follow object then do so 
        if (Vector3.Distance(target.transform.position, _rb.transform.position) > minDistance)
        {
            _rb.MovePosition(_rb.transform.position + dir * speed * Time.fixedDeltaTime);
        }

        _rb.transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime);

        //Debug.Log(_rb.velocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _audioManager.GetComponent<AudioManager>().Play("Enemy_Death");
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Trigger enter");
        if(other.CompareTag("Projectile"))
        {
            _controller.IncreaseScore("Enemy");
            _audioManager.GetComponent<AudioManager>().Play("Enemy_Death");
            Destroy(this.gameObject);
        }
    }
}
