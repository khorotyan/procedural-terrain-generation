using UnityEngine;
using System.Collections;

public class CamMovement : MonoBehaviour
{

	void Awake ()
    {
        //transform.position = new Vector3(Vars.terWidth / 4, 75, -Vars.terHeight / 4);
	}
	
	void Update ()
    {
        transform.RotateAround(Vector3.zero, Vector3.up, 10 * Time.deltaTime);
	}
}
