using UnityEngine;
using System.Collections;

public class CamMovement : MonoBehaviour
{
    private float camSpeed = 45f;
    private float rotSpeed = 200f;
    private float scrollMult = 100f;

    private bool canMoveMouse = false;

    void Awake ()
    {
        //transform.position = new Vector3(Vars.terWidth / 4, 75, -Vars.terHeight / 4);
	}
	
	void Update ()
    {
        //transform.RotateAround(Vector3.zero, Vector3.up, 10 * Time.deltaTime);

        // If the key "Shift" is pressed, make the movement and rotation speeds faster
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            camSpeed *= 3;
            rotSpeed *= 3;
            scrollMult *= 3;
        }

        // If we let go the button "Shift", the movement and rotation speeds will get slower
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            camSpeed /= 3;
            rotSpeed /= 3;
            scrollMult /= 3;
        }

        // Move the camera
        transform.position += Input.GetAxis("Vertical") == 0 ? Vector3.zero : Input.GetAxis("Vertical") > 0 ? transform.rotation * Vector3.forward * camSpeed * Time.deltaTime : transform.rotation * Vector3.back * camSpeed * Time.deltaTime;
        transform.position += Input.GetAxis("Horizontal") == 0 ? Vector3.zero : Input.GetAxis("Horizontal") > 0 ? transform.rotation * Vector3.right * camSpeed * Time.deltaTime : transform.rotation * Vector3.left * camSpeed * Time.deltaTime;
        transform.position += new Vector3(0, -Input.GetAxis("Mouse ScrollWheel") * camSpeed * scrollMult * Time.deltaTime, 0);

        // Clamp the position values between some points to prevent the user from going to far away from the landscape
        float clampedXPos = Mathf.Clamp(transform.position.x, -100, 100);
        float clampedYPos = Mathf.Clamp(transform.position.y, 50, 200);
        float clampedZPos = Mathf.Clamp(transform.position.z, -100, 100);

        // Set the clamped values
        transform.position = new Vector3(clampedXPos, clampedYPos, clampedZPos);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            canMoveMouse = !canMoveMouse;
        }

        if (canMoveMouse == false)
        {
            if (Input.mousePosition.x < Screen.width * 7 / 10)
            {
                // Rotate the camera
                transform.eulerAngles += new Vector3(0, Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime, 0);
                transform.eulerAngles += new Vector3(-Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime, 0, 0);

                /*
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 10000))
                {
                    Debug.DrawLine(ray.origin, hit.point);
                    transform.rotation = Quaternion.LookRotation((hit.point - transform.position) * Time.deltaTime);
                }
                */

                //transform.rotation = Quaternion.LookRotation(Input.mousePosition);

                //transform.Rotate(transform.eulerAngles + new Vector3(0, Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime, 0));
                //transform.Rotate(transform.eulerAngles + new Vector3(-Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime, 0, 0));
            }
        }
        else
        {
            transform.eulerAngles += new Vector3(0, Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime, 0);
            transform.eulerAngles += new Vector3(-Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime, 0, 0);
  
        }
        
        

        //transform.rotation += Input.GetAxis("Mouse X") == 0 ? Vector3.zero : Input.GetAxis("Mouse X") > 0 ? Quaternion.Euler(0, -camSpeed * scrollMult, 0) * Time.deltaTime : new Vector3(0, camSpeed * scrollMult, 0) * Time.deltaTime;

    }
}
