using UnityEngine;
using System;
using System.Collections;

public class Attack : MonoBehaviour 
{
    Collider swordCollider;
    public float hitPoints = 25.0f;
    Animation anim;
    public GameObject splashBlood;
    public Transform splashBloodSpawn;
   
	// Use this for initialization
    void Start()
    {
        swordCollider = gameObject.GetComponentInChildren<BoxCollider>();
        anim = GetComponentInChildren<Animation>();
        Physics.IgnoreCollision(swordCollider, collider);  
    }
    void AttackSword(Collider hit)
    {
        if (anim.IsPlaying("swipeSword"))
        {
            if (hit.tag == "NPC" || hit.tag == "Player")
            {
                Instantiate(splashBlood, splashBloodSpawn.position,
                    splashBloodSpawn.rotation);
            }
            if (hit.collider.tag == "Default")
            {
                Vector3 objectDir = hit.transform.position - transform.position;
                hit.rigidbody.AddForce(objectDir.normalized * 100);
            }
            hit.BroadcastMessage("CauseDamage", hitPoints, 
                SendMessageOptions.DontRequireReceiver);
        }
    }
}

