using UnityEngine;
using System.Collections;

public class PlayerControll : MonoBehaviour 
{
    public Joystick leftJoystick;

	public Vector3 move;
	public static CharacterController playerController;
    PlayerMotor playerMotor;
    PlayerInteraction playerInteraction;
	float verticalVelocity;
	
    float jumpSpeed = 1.5f;
	float jumpRange = 0.5f;
    float landingDeadband = 0.3f;
	float lastJump = 0;
	
    bool isGrounded = true;
	bool inAir = false;
	float gravity = 9.8f;
	
	
	float lastSwipe = 0;
	float swipeRange = 0.5f;
	

    void Start()
    {
        playerMotor = GetComponentInChildren<PlayerMotor>();
        playerController = GetComponent<CharacterController>();
        playerInteraction = GetComponentInChildren<PlayerInteraction>();
        try
        {
            if (leftJoystick == null)
                leftJoystick = GameObject.Find("Single Joystick").GetComponentInChildren<Joystick>();
            playerMotor.leftJoystick = leftJoystick;
        }
        catch
        {
            leftJoystick = null;
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        GetInput();
        InAir();
	    playerMotor.UpdateMotor(move);
	}
	
	
	private void GetInput() 
    {
        if (leftJoystick != null && leftJoystick.tapCount > 0)
        {
            leftJoystick.position.Normalize();
            move = new Vector3(leftJoystick.position.x, verticalVelocity,
                leftJoystick.position.y);
        }
        else if (leftJoystick == null)
        {
            move = new Vector3(Input.GetAxis("Horizontal"), verticalVelocity,
                Input.GetAxis("Vertical"));
        }
        else
        {
            move = Vector3.zero;
            move.y = verticalVelocity;
        }

        if (verticalVelocity <= 0 && playerInteraction.corpseLeg == null) 
        {
            if (move.x != 0 || move.z != 0)
                BroadcastMessage("WalkForward");
            else
                BroadcastMessage("Idle");
		
		}
		if (Input.GetButtonDown("Jump")) 
        {
            Jump();
		}

        if (leftJoystick == null)
        {
            if (Input.GetButton("Fire1"))
            {
                SwipeSword();
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
            BroadcastMessage("TestAnimation");
       
	}

    void Jump()
    {
        if (Time.time > lastJump + jumpRange)
        {
            BroadcastMessage("UpdatePosition", true);
            BroadcastMessage("JumpAnim");
            verticalVelocity = jumpSpeed;
            lastJump = Time.time;
        }
    }

    void SwipeSword()
    {
        if (Time.time > lastSwipe + swipeRange)
        {
            BroadcastMessage("SwipeSwordAnim");
            BroadcastMessage("SwipeSwordSound");
            lastSwipe = Time.time;
        }
    }

    void InAir()
    {
        if (!isGrounded)
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        isGrounded = Physics.Raycast(transform.position, new Vector3(0, -1, 0), landingDeadband);
        if (!isGrounded) inAir = true;
        if (isGrounded && inAir)
        {
            BroadcastMessage("Landing");
            inAir = false;
        }
    }
}
