using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossibleThing : MonoBehaviour
{
    public float chance = 1f;
    public float jigglePosition;
    public float jiggleRotation;
    public List<GameObject> prefabs = new();
    

    // Start is called before the first frame update
    void Start()
    {
        if (Random.value < chance)
        {
            Vector3 jiggle = Random.insideUnitCircle * jigglePosition;
            Vector3 point = transform.position + new Vector3(jiggle.x, 0f, jiggle.y);
            Quaternion rotation = transform.rotation;
            float jiggleTurn = Random.Range(-jiggleRotation /2f, jiggleRotation /2f);
            rotation = Quaternion.Euler(0f, rotation.eulerAngles.y + jiggleTurn, 0f);
            GameObject obj = Instantiate(prefabs[Random.Range(0, prefabs.Count)], point, rotation); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
