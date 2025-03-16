using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static TimeController instance;
    public List<Character> allCharacters = new();
    public bool isTutorial = false;

    [SerializeField]
    LineRenderer lineRenderer;
    FlagPlacer flagPlacer;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        allCharacters.Clear();
        total_intervals = (int) (TIME_TOTAL / TIME_INTERVAL);
        currentInterval = 0;
        currentOffset = 0f;
        flagPlacer = GetComponent<FlagPlacer>();
    }
    int total_intervals;
    public const float TIME_INTERVAL = .2f;
    public const float TIME_TOTAL = 3f;
    public static int currentInterval = 0;
    public static float currentOffset = 0f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Return) && !going && ended == -1f)
        {
            StartTest();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentInterval -= 1;
            currentInterval = Mathf.Clamp(currentInterval, 0, total_intervals);
            SetTime(currentInterval, 0f);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentInterval += 1;
            currentInterval = Mathf.Clamp(currentInterval, 0, total_intervals);
            SetTime(currentInterval, 0f);
        }
        

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo))
            {
                if (hitInfo.collider.TryGetComponent<TestMovement>(out TestMovement move))
                {
                    TestMovement.currentSelection = move;
                    flagPlacer.PlaceFlags(move);
                    RequestUpdate();
                }
            }
        }

        if (TestMovement.currentSelection)
        {
            lineRenderer.positionCount = TestMovement.currentSelection.allCorners.Count;
            lineRenderer.SetPositions(TestMovement.currentSelection.allCorners.ToArray());
        }
        else
        {
            lineRenderer.positionCount = 0;
            flagPlacer.ClearFlags();
        }
    }

    public void SetTime(int interval, float offset)
    {
        foreach (Character c in allCharacters)
        {
            c.UpdateEveryting(interval, offset, false);
        }
    }

    public static void RequestUpdate()
    {
        instance.L_RequestUpdate();
    }
    void L_RequestUpdate()
    {
        SetTime(currentInterval, 0f);
        if (TestMovement.currentSelection != null)
        {
            flagPlacer.PlaceFlags(TestMovement.currentSelection);
        }
        else
        {
            flagPlacer.ClearFlags();
        }
        
    }

    bool going = false;
    private void FixedUpdate()
    {
        if (going)
        {
            TestMovement.currentSelection = null;
            foreach (Character c in allCharacters)
            {
                c.UpdateEveryting(currentInterval, currentOffset, true);
            }
            currentOffset += Time.fixedDeltaTime;
            if (currentOffset > TIME_INTERVAL)
            {
                currentInterval++;
                currentOffset -= TIME_INTERVAL;
                
            }

            if (currentInterval >= total_intervals)
            {
                StopTest();

            }
        }
        if (ended >= 0f)
        {
            ended += Time.fixedDeltaTime;
        }
    }

    void StartTest()
    {
        going = true;
        currentOffset = 0f;
        currentInterval = 0;
        foreach (Character c in allCharacters)
        {
            c.StartTest();
        }
    }
    public float ended = -1f;
    void StopTest()
    {
        going = false;
        //currentInterval = 0;
        //currentOffset = 0f;
        foreach (Character c in allCharacters)
        {
            if (!isTutorial)
                c.EndTest();
            if (isTutorial)
                c.ResetTest(); //TEMPORARY
        }
        ended = 0f;
        if (isTutorial)
            RequestUpdate();//TEMPORARY?
    }

    public static float ConvertToTime(int interval, float offset)
    {
        return interval * TIME_INTERVAL + offset;
    }
}
