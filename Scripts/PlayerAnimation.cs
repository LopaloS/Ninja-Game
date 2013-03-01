using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

    Transform mixTransform;

	// Initialization

	void Start() 
    {
     
        GetJoint();

        animation["walk"].layer = 2;
        animation["walk"].speed = 1.0f;

		animation["swipeSword"].blendMode = AnimationBlendMode.Additive;
		animation["swipeSword"].layer = 1;
		animation["swipeSword"].speed = 0.8f;
		
		animation["jump"].layer = 2;
		animation["jump"].speed = 0.5f;
		
		animation["landing"].layer = 2;
		animation["landing"].speed = 0.5f;
		
		animation["idle"].layer = 2;
        animation["idle"].speed = 0.5f;

        animation["StealthWalk"].layer = 2;
        animation["StealthWalk"].speed = - 0.7f;
        animation["Drag"].AddMixingTransform(mixTransform);
        animation["Drag"].layer = 3;
        animation["Drag"].speed = 0.1f;

        animation["PickUp"].layer = 3;
        animation["PickUp"].speed = 0.5f;

        animation["test"].speed = 0.1f;
        animation["test"].layer = 6;
        
		animation.Stop();
	}
	
	void WalkForward() 
    {
        if (animation.IsPlaying("Drag"))
            animation.CrossFade("walk", 0.3f, PlayMode.StopAll);
        else
            animation.CrossFade("walk");
	}
	
	void JumpAnim() 
    {
		animation.Play("jump");
	}
	
	void Landing() 
    {
		animation.Play("landing");
	}
	
	void Idle()
    {
        if (animation.IsPlaying("Drag"))
            animation.CrossFade("idle", 0.3f, PlayMode.StopAll);
        else
            animation.CrossFade("idle");        
	}

    void IdleWithCorpse()
    {
        animation.CrossFade("idle");
        animation.CrossFade("Drag");
    }
	void SwipeSwordAnim() 
    {
		animation.Play("swipeSword");
	}

    void DragCorpse()
    {
        animation.CrossFade("StealthWalk");
        animation.CrossFade("Drag");
    }

    void PickUpFromFloor()
    {
        animation.Play("PickUp");
    }

    void TestAnimation()
    {
        animation.Play("test");
    }

    void GetJoint()
    {
        Transform[] objets = GetComponentsInChildren<Transform>();
        foreach (Transform joint in objets)
        {
            if (joint.name == "Joint3")
                mixTransform = joint;
        }
    }
}
