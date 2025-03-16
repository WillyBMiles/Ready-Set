using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool playerTeam;
    public bool hostage;

    public GameObject shield;

    public List<Weapon> possibleWeapons = new();
    public Weapon weapon;
    public SpriteRenderer S_weapon;
    public SpriteRenderer S_muzzleFlare;
    public SpriteRenderer S_character;
    public SpriteRenderer S_body;
    public List<Sprite> S_characters = new();
    public List<AudioClip> clips = new();
    int gender;

    public List<SpriteRenderer> extraRecolors = new();

    public GameObject shotOrigin;

    public Color color;

    new Collider collider;
    TestMovement move;
    EnemyAI ai;
    // Start is called before the first frame update
    void Start()
    {
        gender = Random.Range(0, 2);
        if (possibleWeapons.Count > 0)
        {
            weapon = possibleWeapons[Random.Range(0, possibleWeapons.Count)];
        }
        collider = GetComponent<Collider>();
        UpdateSprite();
        move = GetComponent<TestMovement>();
        ai = GetComponent<EnemyAI>();

        TimeController.instance.allCharacters.Add(this);
    }

    int mFlare = 0;
    // Update is called once per frame
    void Update()
    {
        if (mFlare == 0)
            S_muzzleFlare.enabled = false;
        mFlare--;
    }
    void UpdateSprite()
    {
        S_character.sprite = S_characters[gender];
        S_character.color = color;
        S_body.color = color;

        foreach (var sr in extraRecolors)
        {
            sr.color = color;
        }
        if (weapon != null)
        {
            S_weapon.enabled = true;
            S_weapon.sprite = weapon.sprite;
            S_weapon.color = color;
            S_muzzleFlare.transform.localPosition = weapon.muzzleFlare;
            shotOrigin.transform.localPosition = weapon.shotOrigin;
            if (shield)
            shield.SetActive(weapon.enableShield);
        }
        else
        {
            S_weapon.enabled = false;
        }
        
    }
    float lastFireTime = 0f;
    int ammoUsed = 0;
    public void Fire(Vector3 position, MoveType move)
    {
        if (ammoUsed < weapon.ammo && weapon != null && lastFireTime + 1f / weapon.shotsPerSecond < Time.time)
        {
            weapon.Fire(this, position, move);
            lastFireTime = Time.time;
            ammoUsed++;
            S_muzzleFlare.enabled = true;
            mFlare = 10;
            if (ammoUsed == weapon.ammo)
            {
                S_weapon.sprite = weapon.emptySprite;
            }
        }
    }

    public void GetHit()
    {
        S_character.enabled = false;
        S_muzzleFlare.enabled = false;
        S_weapon.enabled = false;
        S_body.enabled = true;
        collider.enabled = false;
        if (shield)
            shield.SetActive(false);
        if (move != null)
            move.enabled = false;
        AudioSource.PlayClipAtPoint(clips[gender], transform.position);
    }
    void Revive()
    {
        
        S_character.enabled = true;
        S_muzzleFlare.enabled = false;
        if (weapon != null)
        {
            S_weapon.enabled = true;
            S_weapon.sprite = weapon.sprite;
            if (shield)
                shield.SetActive(weapon.enableShield);
        }
            
        S_body.enabled = false;
        collider.enabled = true;
        if (move != null)
            move.enabled = true;
    }

    public void StartTest()
    {
        ammoUsed = 0;
        lastFireTime = 0f;
        Revive();
        if (move)
        {
            move.StartTest();
        }
    }

    public void ResetTest()
    {
        ammoUsed = 0;
        lastFireTime = 0f;
        Revive();
    }

    public void EndTest()
    {
        if (move)
        {
            move.EndTest();
        }
    }


    public void UpdateEveryting(int interval,float offset, bool doActions)
    {
        if (S_body.enabled)
        {
            return;
        }

        if (move != null)
        {
            move.UpdateEverything(interval, offset, doActions);
        }
        if (ai != null)
        {
            ai.UpdateEverything(interval, offset, doActions);
        }
    }
}
