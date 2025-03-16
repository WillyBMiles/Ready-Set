using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [HideInInspector]
    public List<int> shootTimes = new List<int>();
    public int maxShoots = 3;
    Character c;

    // Start is called before the first frame update
    void Start()
    {
        c = GetComponent<Character>();
        shootTimes.Clear();
        int shoots = Random.Range(1, maxShoots + 1);
        for (int i = 0; i < shoots; i++)
        {
            shootTimes.Add(Random.Range(5, (int)(TimeController.TIME_TOTAL / TimeController.TIME_INTERVAL)));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateEverything(int interval, float offset, bool doActions)
    {
        var p = NearestPlayer();
        if (p.HasValue)
        {
            Quaternion rotation = Quaternion.LookRotation(p.Value - transform.position);
            rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
            transform.rotation = rotation;
            if (doActions && shootTimes.Contains(interval))
            {
                c.Fire(p.Value, MoveType.Still);
            }
        }
    }

    Vector3? NearestPlayer()
    {
        Character closest = null;
        float dist = float.PositiveInfinity;
        foreach (Character c in TimeController.instance.allCharacters)
        {
            float newDist = Vector3.Distance(c.transform.position, transform.position);
            if (!c.S_body.enabled && c.playerTeam && newDist < dist && HasLOS(c.transform.position+Vector3.up))
            {
                closest = c;
                dist = newDist;
            }
        }
        if (closest)
            return closest.transform.position;
        return null;
    }

    bool HasLOS(Vector3 pos)
    {
        LayerMask mask = LayerMask.GetMask("Default");
        return !Physics.Raycast(transform.position, pos - transform.position, Vector3.Distance(transform.position, pos), mask);
    }
}
