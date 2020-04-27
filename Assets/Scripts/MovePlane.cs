using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MovePlane : MonoBehaviour
{
    private float planeSpeed = 150;
    public float rotationSpeed = 50;
    public float speedCap = 10;
    public int enemyDamage = 5;
    public int toolHeal = 20;
    private Rigidbody planeRb;
    private GameManager gm;
    private AudioSource audioSource;
    public AudioClip explosion;
    public AudioClip item;
    private Transform pivot;
    [ReadOnly(true)]
    public float distanceToPivot;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        pivot = GameObject.Find("Pivot").transform;
        audioSource = GetComponent<AudioSource>();
        planeRb = GetComponent<Rigidbody>();
        planeRb.AddForce(Vector3.forward * planeSpeed, ForceMode.Impulse);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gm.isGameOver && !gm.isMenu)
        {
            planeRb.AddRelativeForce(Vector3.forward * planeSpeed * Time.deltaTime * (speedCap - planeRb.velocity.magnitude), ForceMode.Impulse);
            var rotationMagnitude = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
            planeRb.AddRelativeTorque(new Vector3(0, rotationMagnitude, 0), ForceMode.Impulse);
            if(planeRb.angularVelocity.magnitude > 1)
            {
                planeRb.angularVelocity = planeRb.angularVelocity.normalized;
            }
        }
        else
        {
            planeRb.velocity = Vector3.zero;
            planeRb.angularVelocity = Vector3.zero;
        }
        distanceToPivot = (pivot.position - transform.position).magnitude;
        if (distanceToPivot > 120)
        {
            gm.ShowDistanceWarning(true);
        }
        else
        {
            gm.ShowDistanceWarning(false);
        }

        if (distanceToPivot > 160)
        {
            gm.DecreaseLife(100);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Plane -" + other.tag);
        if(other.tag == "Bullet")
        {
            other.gameObject.GetComponent<BulletController>().DestroyBullet();
            audioSource.PlayOneShot(explosion);
            gm.DecreaseLife(enemyDamage);
        }

        if(other.tag == "Tool")
        {
            Destroy(other.gameObject);
            gm.IncreaseLife(toolHeal);
            audioSource.PlayOneShot(item);
        }
    }

}
