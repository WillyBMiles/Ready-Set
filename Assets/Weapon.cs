using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    [TextArea]
    public string description;
    public Sprite sprite;
    public Sprite emptySprite;
    public Vector3 muzzleFlare;
    public Vector3 shotOrigin;
    public int ammo;
    public AudioClip clip;
    public float volume = 1f;

    public GameObject projectile;

    public bool enableShield = false;

    [Tooltip("In degrees")]
    public float spread;
    [Tooltip("In degrees")]
    public float spreadWhileWalking;
    [Tooltip("In degrees")]
    public float spreadWhileRunning;
    public float shotsPerSecond;


    public void Fire(Character origin, Vector3 target, MoveType move)
    {
        if (clip !=null)
        AudioSource.PlayClipAtPoint(clip, origin.transform.position, volume);

        float thisSpread = 0f;
        switch (move)
        {
            case MoveType.Still:
                thisSpread = Random.Range(-spread / 2f, spread /2f);
                break;
            case MoveType.Walk:
                thisSpread = Random.Range(-spreadWhileWalking/2f, spreadWhileWalking/2f);
                break;
            case MoveType.Run:
                thisSpread = Random.Range(-spreadWhileRunning/2f, spreadWhileRunning/2f);
                break;
        }

        float angle = Quaternion.LookRotation(target - origin.transform.position, Vector3.up).eulerAngles.y;
        angle += thisSpread;
        Vector3 attackDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
        if (projectile == null)
        {
            
            if (Physics.Raycast(origin.shotOrigin.transform.position, attackDirection, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.TryGetComponent<Character>(out Character c))
                {
                    c.GetHit();
                }
                ShotRenderController.instance.RenderShot(origin.shotOrigin.transform.position, hitInfo.point);
            }
        }
        else
        {
            GameObject go = Instantiate(projectile, origin.shotOrigin.transform.position, Quaternion.LookRotation(attackDirection, Vector3.up));
            Projectile p = go.GetComponent<Projectile>();
            p.target = target;
            p.origin = origin;
            SpriteRenderer sr = p.GetComponentInChildren<SpriteRenderer>();
            sr.color = origin.color;
        }


    }
}
