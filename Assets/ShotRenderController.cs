using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotRenderController : MonoBehaviour
{
    public static ShotRenderController instance;
    public GameObject srPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RenderShot(Vector3 start, Vector3 end)
    {
        GameObject go = Instantiate(srPrefab);
        LineRenderer lr= go.GetComponent<LineRenderer>();
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}
