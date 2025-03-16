using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public float delay;
    float time = 0f;
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > delay)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - Time.deltaTime);
        }
        
    }
}
