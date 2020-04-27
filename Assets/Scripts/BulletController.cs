using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject player;
    public GameObject explosion;
    public float speed = 5;
    public float timeUntilExplosion = 10;
    private Rigidbody bulletRb;
    
    // Start is called before the first frame update
    void Start()
    {
        bulletRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        Invoke(nameof(DestroyBullet), timeUntilExplosion);
        var lookDirection = (player.transform.position - transform.position).normalized;
        bulletRb.AddForce(lookDirection * speed, ForceMode.Impulse);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var lookDirection = (player.transform.position - transform.position).normalized;
        bulletRb.AddForce(lookDirection * speed);
        bulletRb.MovePosition(new Vector3(bulletRb.position.x, Mathf.Min(bulletRb.position.y, 10), bulletRb.position.z));
    }

    private void LateUpdate()
    {
       // transform.Translate(bulletRb.position.x, Mathf.Min(bulletRb.position.y, 10), bulletRb.position.z);
    }

    public void DestroyBullet()
    {
        var exp = Instantiate(explosion, this.transform.position, explosion.transform.rotation);
        Destroy(gameObject);
    }
}
