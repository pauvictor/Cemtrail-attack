using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimPlayer : MonoBehaviour
{
    public Transform player;
    public GameObject bullet;
    public Transform gun;
    public ParticleSystem shoot;
    public float speed = 2;
    public float timeRangeMin = 1;
    public float timeRangeMax = 5;
    private GameManager gm;
    private bool isFalling = false;
    private AudioSource audioSource;
    public AudioClip scream;
    public AudioClip explosion;
    private bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
        transform.LookAt(player);
        Invoke(nameof(CreateBullet), Random.Range(timeRangeMin, timeRangeMax));
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position.x > gm.xLimit || transform.position.x < -gm.xLimit ||
            transform.position.z > gm.zLimit || transform.position.z < -gm.zLimit) && !isFalling)
        {
            isFalling = true;
            audioSource.PlayOneShot(scream);
        }
        if (!isFalling)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            transform.LookAt(new Vector3(player.position.x, 0, player.position.z));
        }
        else
        {
            transform.Translate(Vector3.down * Time.deltaTime * speed);
            if(transform.position.y < -15)
            {
                Destroy(gameObject);
                gm.IncreaseScore(20);
            }

        }
        
    }

    private void CreateBullet()
    {
        if (!dead)
        {
            var position = new Vector3(gun.transform.position.x, gun.transform.position.y, gun.transform.position.z + 1);
            var bulletInstance = Instantiate(bullet, position, transform.rotation);
            Invoke(nameof(CreateBullet), Random.Range(timeRangeMin, timeRangeMax));
            shoot.Play();
            audioSource.PlayOneShot(explosion);
        }
    }
       
    private void OnParticleCollision(GameObject other)
    {
        //Debug.Log("Enemy - " + other.tag + " - ParticleCollision ");
        if (other.tag == "Smoke" && !dead)
        {
            //Debug.Log("Enemy - Smoke - ParticleCollision");
            audioSource.PlayOneShot(scream);
            dead = true;
            Invoke(nameof(KillSelf), 2);
            gm.IncreaseScore(20);
        }
    }

    private void KillSelf()
    {
        Destroy(gameObject);
    }

}
