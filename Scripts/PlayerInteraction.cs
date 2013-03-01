using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour {
    public Transform hand;
    public Transform corpseLeg;
    PlayerControll playerControll;
    FixedJoint joint;

    public List<GameObject> items;
    List<Collider> colliders;

    bool drop = false;

	// Use this for initialization
	void Start () 
    {
        items = new List<GameObject>();
        colliders = new List<Collider>();
        hand = HelpMethods.GetTransform(transform, "Joint17");
        playerControll = GetComponentInChildren<PlayerControll>();
        StartCoroutine(DropCorpse());
	}

    IEnumerator DropCorpse()
    {
        while (true)
        {
            if (corpseLeg != null && !drop)
            {
                yield return new WaitForSeconds(0.5f);
                drop = true;
            }
            if (corpseLeg == null)
                drop = false;
            yield return null;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        if (corpseLeg != null)
        {
            
            Vector3 handDir = hand.position - corpseLeg.position;
            corpseLeg.rigidbody.velocity = handDir * 100;
            Debug.DrawRay(corpseLeg.position, handDir * 100, Color.blue);
        }       
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            Interact();
        if (corpseLeg != null)
        {
            if (playerControll.move.x != 0 || playerControll.move.z != 0)
                BroadcastMessage("DragCorpse");
            else
                BroadcastMessage("IdleWithCorpse");
        }       
    }

    void OnTriggerEnter(Collider hit)
    {
        colliders.Add(hit);
    }

    void OnTriggerExit(Collider hit)
    {
        colliders.Remove(hit);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "Default")
        {
            Vector3 objectDir = hit.transform.position - transform.position;
            hit.rigidbody.AddForce(objectDir.normalized * 100);
        }
    }

    void Interact()
    {
        foreach (Collider coll in colliders)
        {
            print(coll);
            if (coll == null)
                continue;

            if (coll.tag == "Corpse" && corpseLeg == null && !drop)
            {
                IgnoreRagDollCollision(coll);
                BroadcastMessage("PickUpFromFloor");
                corpseLeg = HelpMethods.GetTransform(coll.transform, "Joint20");
            }
            else if(corpseLeg != null && drop)
                corpseLeg = null;

            if (coll.tag == "item")
            {
                BroadcastMessage("PickUpFromFloor");
                items.Add(coll.gameObject);
                Destroy(coll.gameObject, 0.5f);
            }
        }
    }

    void IgnoreRagDollCollision(Collider hit)
    {
        Collider[] colliders = hit.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            Physics.IgnoreCollision(collider, col);
        }
    }
}
