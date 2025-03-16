using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotRenderer : MonoBehaviour
{
    LineRenderer lr;
    public float maxLifeTime = .2f;
    public float startingAlpha;
    float lifeTime;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lifeTime = maxLifeTime;
    }
    // Update is called once per frame
    void Update()
    {
        lr.startColor = new Color(lr.startColor.r, lr.startColor.g, lr.startColor.b, lifeTime / maxLifeTime * startingAlpha);
        lr.endColor = lr.startColor;
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
