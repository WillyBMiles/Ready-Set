using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector]
    public Vector3 target;
    public Character origin;
    Vector3 startLocation;
    
    public float lifeTime;

    public float speed;
    public GameObject explosion;
    public AudioClip clip;


    float time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        startLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > lifeTime)
        {
            Explode();
        }
        transform.position += (target - startLocation).normalized * speed * Time.deltaTime;

    }

    void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(clip, transform.position);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (origin == null)
            return;
        if (origin.gameObject == collision.gameObject)
            return;
        if (collision.gameObject.layer != 10)
            Explode();
    }
}
