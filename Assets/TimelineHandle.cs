using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineHandle : MonoBehaviour
{
    RectTransform rect;
    public Vector3 StartPosition;
    static Vector3 sp;
    public Vector3 EndPosition;
    static Vector3 ep;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        sp = StartPosition;
        ep = EndPosition;
    }

    // Update is called once per frame
    void Update()
    {
        rect.anchoredPosition = GetAnchoredPosition(TimeController.currentInterval, TimeController.currentOffset);
    }

    public static Vector2 GetAnchoredPosition(int interval, float offset)
    {
        return Vector3.Lerp(sp, ep, TimeController.ConvertToTime(interval, offset) / TimeController.TIME_TOTAL);
    }
}
