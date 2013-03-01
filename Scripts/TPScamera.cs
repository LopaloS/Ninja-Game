using UnityEngine;
using System.Collections;

public class TPScamera : MonoBehaviour 
{
    public GUITexture leftJoystick;
    public MenuNGUI menu;

	public Transform target;
	public static TPScamera instance;
	
	//private GameObject cameraTarget;
	bool jump = false;
	
	public float mouseX = 0;
    public float mouseY = 25.0f;
	public float mouseSensitivity = 3.0f;
	
	RaycastHit hit;
	float distance = - 6.0f;
    float viewPointHeight = 1.65f;
	
	const float Min_Y_Limit = 3f;
	const float Max_Y_Limit = 30.0f;
	
	float yVelocity = 0.0f;
	float xVelocity = 0.0f;
	float xSmooth = 0.05f;
	float ySmooth = 0.1f;
	
	Vector3 desirePosition;
	
	// Use this for initialization
	void Start () 
    {
        mouseX = target.transform.eulerAngles.y;
		instance = this;
        menu = GameObject.Find("MenuNgui")
                .GetComponentInChildren<MenuNGUI>();
        mouseSensitivity = menu.touchSensitivity.sliderValue;
        try
        {
            if (leftJoystick == null)
                leftJoystick = GameObject.Find("Single Joystick")
                    .GetComponentInChildren<GUITexture>();
        }
        catch
        {
            leftJoystick = null;
        }
	}
	
	
	// Update is called once per frame
	void LateUpdate () 
    {
		PlayerInput();
		CalcDesirePosition();
		UpdatePosition(ref jump);
	}
	
	private void PlayerInput() {
        if (leftJoystick != null)
        {
            foreach (Touch touch in Input.touches)
            {
                if (Input.touchCount > 0 && touch.phase == TouchPhase.Moved)
                {
                    if (!leftJoystick.HitTest(touch.position))
                    {
                        mouseY += touch.deltaPosition.y * mouseSensitivity;
                        mouseX += touch.deltaPosition.x * mouseSensitivity;
                    }
                }
            }
            //mouseY += rightJoystick.position.y * mouseSensitivity;

        }
        else
        {
            mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;
            mouseY += Input.GetAxis("Mouse Y") * mouseSensitivity;
        }
		mouseY = Mathf.Clamp(mouseY, Min_Y_Limit, Max_Y_Limit);
		mouseX = HelpMethods.AngleClamp(mouseX);
	}
	
	void CalcDesirePosition() 
    {
        if (target == null)
            return;
        Debug.DrawLine(target.position, transform.position, Color.blue);
		if(Physics.Linecast(target.position, transform.position)) {
				distance = Mathf.Lerp(distance, -1, Time.deltaTime);
		 }
			else {
                distance = distance = Mathf.Lerp(distance, -6, Time.deltaTime); ;
		 }
        
		desirePosition = CalcPosition( mouseY, mouseX, distance );		
	}
	
	private Vector3 CalcPosition( float rotX, float rotY, float distance) 
    {
		Vector3 position = new Vector3(0, 0, distance);
		Quaternion rotation = Quaternion.Euler(rotX, rotY, 0);
		return target.position + rotation * position;
	}
	
	private void UpdatePosition(ref bool jump) 
    {
		float posX = Mathf.SmoothDampAngle(transform.position.x,
            desirePosition.x, ref xVelocity, xSmooth);
		float posY = Mathf.SmoothDampAngle(transform.position.y, 
            desirePosition.y, ref yVelocity, ySmooth);
        transform.position = new Vector3(posX, posY, desirePosition.z);
    
		//if(jump)
        //{
		//	jump = true;
		//	Vector3 targetDuringJump = new Vector3(target.position.x, 0, target.position.z);
		//	cameraTarget.transform.position = targetDuringJump;
		//	Debug.Log(targetDuringJump);
		//}
		//else 
        //{
			//cameraTarget.transform.position = target.transform.position;	
		//}	
		transform.LookAt(target);
	}
	
}


