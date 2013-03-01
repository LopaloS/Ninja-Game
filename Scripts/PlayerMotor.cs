using UnityEngine;
using System.Collections;

public class PlayerMotor : MonoBehaviour {

	float speed = 5.0f;
	float acceleration = 2.0f;
	float moveVelocity = 0.0f;
    float maxMoveVelocity = 1.0f;
    float dragCorpseVelocity = 0.4f;
	
    float smoothRotationPlayer = 6.0f;
    public float rotationY = 0.0f;

    public Joystick leftJoystick;
	Quaternion rotation;
    PlayerInteraction playerInteraction;

    void Start()
    {
        rotationY = transform.eulerAngles.y;
        playerInteraction = GetComponentInChildren<PlayerInteraction>();
        try
        {
            if (leftJoystick == null)
                leftJoystick = GameObject.Find("Single Joystick")
                    .GetComponentInChildren<Joystick>();
        }
        catch
        {
            leftJoystick = null;
        }
    }
	
	public void UpdateMotor(Vector3 move) 
    {
        if (playerInteraction.corpseLeg != null)
        {
            move.x *= -1;
            move.z *= -1;
        }
	    MovePlayer(move);
        if (leftJoystick == null)
            MouseRotatePlayer(move);
        else
            JoyRotatePlayer(move);
        RotatePlayer();
	}
	
	void MovePlayer(Vector3 moveVector) 
    {
        if (playerInteraction.corpseLeg == null)
        {
            if (moveVelocity <= maxMoveVelocity && 
                (moveVector.x != 0 || moveVector.z != 0) )
                moveVelocity += acceleration * Time.deltaTime;
            else if (moveVector.x == 0 && moveVector.z == 0 &&
                moveVelocity >= 0)
                moveVelocity -= acceleration * Time.deltaTime;
        }
        else
        {
            if (moveVelocity >= -dragCorpseVelocity && 
                (moveVector.x != 0 || moveVector.z != 0))
                moveVelocity -= acceleration * Time.deltaTime;
            else if (moveVector.x == 0 && moveVector.z == 0 &&
                moveVelocity <= 0)
                moveVelocity += acceleration * Time.deltaTime;
        }            
    
		moveVector = new Vector3(0, moveVector.y, moveVelocity);
		moveVector = transform.TransformDirection(moveVector);
		moveVector *= speed;
		moveVector *= Time.deltaTime;
		PlayerControll.playerController.Move(moveVector);
	}
	
	void MouseRotatePlayer(Vector3 move) {
		if(move.z > 0) {
			rotationY = Camera.main.transform.eulerAngles.y;
		}
		if(move.z < 0){
			rotationY = Camera.main.transform.eulerAngles.y + 180;	
		}
		if(move.x > 0){
			rotationY = Camera.main.transform.eulerAngles.y + 90;
		}
		if(move.x < 0){
			rotationY = Camera.main.transform.eulerAngles.y - 90;
		}
		if(move.z > 0 && move.x < 0){
			rotationY = Camera.main.transform.eulerAngles.y - 45;
		}
		if(move.z > 0 && move.x > 0){
			rotationY = Camera.main.transform.eulerAngles.y + 45;
		}
		if(move.z < 0 && move.x < 0){
			rotationY = Camera.main.transform.eulerAngles.y - 135;
		}
		if(move.z < 0 && move.x > 0){
			rotationY = Camera.main.transform.eulerAngles.y + 135;
		}
    }
	
	void RotatePlayer()
    {
        Quaternion rotYquater = Quaternion.Euler(transform.eulerAngles.x,
            rotationY, transform.eulerAngles.z);
        rotation = Quaternion.Slerp(transform.rotation, rotYquater, 
            smoothRotationPlayer * Time.deltaTime);
		transform.rotation = rotation;
	}

    void JoyRotatePlayer(Vector3 move)
    {
        if (move.x != 0 && move.z != 0)
        {
            rotationY = Camera.main.transform.eulerAngles.y +
                Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            rotationY = HelpMethods.AngleClamp(rotationY);
        }
    }
}
