using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public enum MoveType
{
    Still, Walk, Run
}


public class TestMovement : MonoBehaviour
{
    public static TestMovement currentSelection;

    Vector3 startingPosition;
    Quaternion startRotation;


    NavMeshPath path;

    public float runSpeed;
    public float walkSpeed;


    public List<Vector3> flags = new List<Vector3>();
    List<MoveType> flagSpeeds = new List<MoveType>();

    public List<Vector3> allCorners = new List<Vector3>();
    List<MoveType> cornerSpeeds = new List<MoveType>();
    public List<int> pauses = new List<int>();
    public Dictionary<int, Vector3> shoot = new Dictionary<int, Vector3>();

    public SpriteRenderer selectionRenderer;

    Character c;

    //NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        startingPosition = transform.position;
        startRotation = transform.rotation;
        c = GetComponent<Character>();
    }



    // Update is called once per frame
    void Update()
    {
        if (currentSelection == this)
        {
            selectionRenderer.enabled = true;

            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo))
                {
                    if (hitInfo.collider.TryGetComponent<TestMovement>(out TestMovement move))
                    { 
                        //we're swapping!
                    }
                    else if (NavMesh.SamplePosition(hitInfo.point, out NavMeshHit nHit, 1f, NavMesh.AllAreas))
                    {
                        //agent.SetDestination(nHit.position);
                        flags.Add(nHit.position);
                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            flagSpeeds.Add(MoveType.Walk);
                        }
                        else
                        {
                            flagSpeeds.Add(MoveType.Run);
                        }
                        
                        RecalculateFlags();
                        TimeController.RequestUpdate();

                        //NavMesh.CalculatePath(transform.position, nHit.position,NavMesh.AllAreas, path); //Saves the path in the path variable.
                        //Vector3[] corners = path.corners;
                        //lineRenderer.positionCount += corners.Length;
                        //lineRenderer.SetPositions(corners);
                        //lineRenderer.positionCount++;
                        //lineRenderer.SetPosition(corners.Length, nHit.position);
                    }
                }
            }

            if (Input.GetMouseButtonDown(1) && flags.Count > 0)
            {
                flags.RemoveAt(flags.Count - 1);
                flagSpeeds.RemoveAt(flagSpeeds.Count - 1);
                RecalculateFlags();
                TimeController.RequestUpdate();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                bool o =TryAddPause(TimeController.currentInterval);
                TimeController.RequestUpdate();
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo))
                {
                    Vector3 point = hitInfo.point;
                    point = new Vector3(point.x, point.y, point.z);
                    TryAddShoting(TimeController.currentInterval, point);

                    TimeController.RequestUpdate();
                }
                    
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                shoot.Remove(TimeController.currentInterval);
                pauses.Remove(TimeController.currentInterval);
                TimeController.RequestUpdate();
            }


        }
        else
        {
            selectionRenderer.enabled = false;
        }

    }

    public void UpdateEverything(int interval, float offset, bool doActions)
    {
        transform.position = FindLocation(interval, offset, out MoveType movetype);
        transform.rotation = FindRotation(interval, offset);
        Vector3? shootVector = IsShooting(interval, offset);
        if (shootVector.HasValue && doActions)
        {
            c.Fire(shootVector.Value, movetype);
        }
    }

    public void StartTest()
    {
        
    }

    public void EndTest()
    {

    }

    void RecalculateFlags()
    {
        Vector3 currentPos = startingPosition;
        allCorners.Clear();
        cornerSpeeds.Clear();

        int flagI = 0;
        foreach (Vector3 flag in flags)
        {
            path.ClearCorners();
            NavMesh.CalculatePath(currentPos, flag, NavMesh.AllAreas, path);
            Vector3[] corners = path.corners;
            allCorners.AddRange(corners);
            for (int i =0; i < corners.Count(); i++)
            {
                cornerSpeeds.Add(flagSpeeds[flagI]);
            }
            currentPos = flag;
            flagI++;
        }
        RemovePathDupes();
        
    }

    public bool TryAddPause(int startTime)
    {
        if (pauses.Contains(startTime))
            return false;
        pauses.Add(startTime);
        return true;
    }
    public bool TryAddShoting(int startTime, Vector3 point)
    {
        shoot[startTime] = point;
        return true;
    }

    void RemovePathDupes()
    {
        for (int i = 1; i < allCorners.Count; i++)
        {
            if (allCorners[i] == allCorners[i - 1])
            {
                allCorners.RemoveAt(i);
                cornerSpeeds.RemoveAt(i);
                i--;
            }
        }
    }

    public Vector3 FindLocation(int interval, float offset)
    {
        return FindLocation(interval, offset, out Vector3 pCorner, out Vector3 nCorner, out MoveType moveType);
    }
    Vector3 FindLocation(int interval, float offset, out MoveType moveType)
    {
        return FindLocation(interval, offset, out Vector3 pCorner, out Vector3 nCorner, out moveType);
    }
    Vector3 FindLocation(int interval, float offset, out Vector3 previousCorner, out Vector3 nextCorner)
    {
        return FindLocation(interval, offset, out previousCorner, out nextCorner, out MoveType moveType);
    }

    Vector3 FindLocation(int interval, float offset, out Vector3 previousCorner, out Vector3 nextCorner, out MoveType moveType )
    {
        float time = TimeController.ConvertToTime(interval, offset);

        previousCorner = startingPosition;
        nextCorner = startingPosition;
        moveType = MoveType.Still;
        if (time == 0f)
        {
            return startingPosition;
        }
        if (!(offset == 0f && pauses.Contains(interval) && !pauses.Contains(interval-1))
            && IsPaused(interval, offset) != -1f)
        {
            Vector3 loc = FindLocation(IsPaused(interval, offset), 0f, out previousCorner, out nextCorner, out moveType);
            moveType = MoveType.Still;
            return loc;
        }
        time -= SumPauseTime(interval,offset);


        moveType = MoveType.Still;
        if (allCorners.Count == 0)
        {
            
            return startingPosition;
        }
            

        float sumTime = 0f;
        Vector3 lastPoint = allCorners[0];
        for (int i =0; i < allCorners.Count; i++)
        {
            Vector3 point = allCorners[i];
            float speed = cornerSpeeds[i] == MoveType.Walk ? walkSpeed : runSpeed;
            moveType = cornerSpeeds[i];

            if (offset < .03f && pauses.Contains(interval - 1))
            {
                moveType = MoveType.Still;
            }

            float nextTime = Vector3.Distance(lastPoint, point) / speed;

            if (i > 0)
            {
                previousCorner = allCorners[i - 1];
            }
            nextCorner = allCorners[i];

            if (nextTime + sumTime > time)
            {
                return lastPoint + (point - lastPoint).normalized * (speed *  (time - sumTime));
            }
            sumTime += nextTime;
            lastPoint = point;
        }
        moveType = MoveType.Still;
        return lastPoint;
    }

    int IsPaused(int interval, float offset)
    {
        int currentInterval = interval;
        while (pauses.Contains(currentInterval))
        {
            currentInterval--;
        }
        if (currentInterval == interval)
            return -1; //no pause
        if (currentInterval < 0)
        {
            return 0;//paused at the beginning
        }
        return currentInterval + 1;

    }


    Vector3? IsShooting(int interval, float offset)
    {
        if (shoot.ContainsKey(interval))
        {
            return shoot[interval];
        }
        return null;
    }


    float SumPauseTime(int interval, float offset)
    {
        float sum = 0f;
        foreach (int t in pauses)
        {
            if (t < interval)
            {
                sum += TimeController.TIME_INTERVAL;
            }
        }
        return sum;
    }

    
    Quaternion FindRotation(int interval, float offset)
    {
        float time = TimeController.ConvertToTime(interval, offset);

        if (time == 0f)
        {
            return startRotation;
        }

        Vector3 point = FindLocation(interval, offset);
        Vector3 nextPoint = FindLocation(interval, offset + .1f);

        if (point == nextPoint)
        {
            FindLocation(interval,offset, out point, out nextPoint);
        }

        Vector3 pointDirection = nextPoint - point;
        if (IsShooting(interval, offset) != null)
        {
            pointDirection = IsShooting(interval, offset).Value - transform.position;
        }
        
        if (pointDirection.sqrMagnitude == 0f)
        {
            return startRotation;
        }
        Quaternion lookRotation = Quaternion.LookRotation(pointDirection, Vector3.up);
        
        lookRotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);
        return lookRotation;
    }
}
