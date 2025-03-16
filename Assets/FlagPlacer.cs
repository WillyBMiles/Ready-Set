using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FlagPlacer : MonoBehaviour
{
    public GameObject shootFlag;
    public GameObject moveFlag;
    public GameObject pauseFlag;

    public List<GameObject> allFlags = new List<GameObject>();


    public GameObject uiShootFlag;
    public GameObject uiMoveFlag;
    public GameObject uiPauseFlag;
    public List<GameObject> uiFlags = new List<GameObject>();
    public Transform parent; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaceFlags(TestMovement tm)
    {
        ClearFlags();

        foreach (Vector3 loc in tm.flags)
        {
            PlaceFlag(loc, moveFlag, Color.green);
            
        }
        foreach (int pause in tm.pauses)
        {
            PlaceFlag(tm.FindLocation(pause, 0), pauseFlag, Color.red);
            PlaceUIFlag(pause, uiPauseFlag, Color.red, 1);
        }

        foreach (int shoot in tm.shoot.Keys)
        {
            PlaceFlag(tm.FindLocation(shoot, 0), shootFlag, Color.white);
            PlaceFlag(tm.shoot[shoot], shootFlag, Color.yellow);
            PlaceUIFlag(shoot, uiPauseFlag, Color.yellow, 2);
        }
    }
    public void ClearFlags()
    {
        while (allFlags.Count > 0)
        {
            Destroy(allFlags[0]);
            allFlags.RemoveAt(0);
        }
        while (uiFlags.Count > 0)
        {
            Destroy(uiFlags[0]);
            uiFlags.RemoveAt(0);
        }
    }

    void PlaceFlag(Vector3 position, GameObject prefab, Color color)
    {
        GameObject go = Instantiate(prefab, position, Quaternion.identity);
        go.GetComponentInChildren<SpriteRenderer>().color = color;
        allFlags.Add(go);
    }

    const float rowOffset = 10f;
    void PlaceUIFlag(int interval, GameObject prefab, Color color, int vertOffset)
    {
        GameObject go = Instantiate(prefab, parent);
       
        go.GetComponentInChildren<UnityEngine.UI.Image>().color = color;
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.anchoredPosition = TimelineHandle.GetAnchoredPosition(interval, 0f);
        rect.anchoredPosition += Vector2.up * vertOffset * rowOffset;
        uiFlags.Add(go);
    }
}
