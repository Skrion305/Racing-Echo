using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;
    int currentPoint = 0;
    int currentLap = 1;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if (waypoints.Length == 0)
        {
            return;
        }
        Transform target = waypoints[currentPoint];
        Vector3 direction = (target.position - transform.position).normalized;
        float angle = Vector3.SignedAngle(-transform.forward, direction, Vector3.up);
        float steer = Mathf.Clamp(angle / 30f, -1f, 1f);
        frontLeftWheel.steerAngle = 30 * steer;
        frontRightWheel.steerAngle = 30 * steer;
        frontLeftWheel.motorTorque = -2000;
        frontRightWheel.motorTorque = -2000;
        rearLeftWheel.motorTorque = -2000;
        rearRightWheel.motorTorque = -2000;
        /*if (Vector3.Distance(transform.position, target.position) < 1f)
        {
            currentPoint++;
            if (currentPoint >= waypoints.Length)
            {
                currentPoint = 0;
                currentLap++;
                if (currentLap > 3)
                {
                    frontLeftWheel.motorTorque = 0;
                    frontRightWheel.motorTorque = 0;
                    rearLeftWheel.motorTorque = 0;
                    rearRightWheel.motorTorque = 0;
                    enabled = false;
                }
            }
        }*/
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            currentPoint++;
            if (currentPoint >= waypoints.Length)
            {
                currentPoint = 0;
                currentLap++;
                if (currentLap > 3)
                {
                    frontLeftWheel.motorTorque = 0;
                    frontRightWheel.motorTorque = 0;
                    rearLeftWheel.motorTorque = 0;
                    rearRightWheel.motorTorque = 0;
                    enabled = false;
                }
            }
        }
    }
}
