using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform trackedObject;
	public float updateSpeed = 6;
	public Vector2 trackingOffset;
	private Vector3 offset;
    private Vector3 newPos;

	void Start()
	{
		offset = (Vector3) trackingOffset;
		offset.z = transform.position.z - trackedObject.position.z;
	}

	void LateUpdate()
	{   
        newPos = new Vector3(trackedObject.position.x, 0, 0);
	    transform.position = Vector3.MoveTowards(transform.position, newPos + offset, updateSpeed);
	}
}