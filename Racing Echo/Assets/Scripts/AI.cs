using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] Transform[] checkpoints1;
    [SerializeField] Transform[] checkpoints2;
    [SerializeField] Transform[] checkpoints3;
    Transform[] waypoints;
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;
    Transform target;
    Vector3 direction;
    float angle;
    float steer;
    public int currentPoint = 0;
    public int currentLap = 1;
    Rigidbody rb;
    float brakeForce;
    float currentMotorTorque;
    float timer = 0f;
    bool obstacle = false;
    Vector3 lookAheadPos;
    Vector3 lookAheadDir;
    float dist;
    public int racingPosition;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (GameData.raceTrack == 1)
        {
            waypoints = checkpoints1;
        }
        if (GameData.raceTrack == 2)
        {
            waypoints = checkpoints2;
        }
        if (GameData.raceTrack == 3)
        {
            waypoints = checkpoints3;
        }
    }
    void FixedUpdate()
    {
        if (obstacle)
        {
            if (timer < 1f)
            {
                frontLeftWheel.motorTorque = 500f;
                frontRightWheel.motorTorque = 500f;
                rearLeftWheel.motorTorque = 500f;
                rearRightWheel.motorTorque = 500f;
                frontLeftWheel.steerAngle = 0;
                frontRightWheel.steerAngle = 0;
                timer += Time.fixedDeltaTime;
            }
            else
            {
                timer = 0f;
                obstacle = false;
            }
        }
        else
        {
            if (waypoints.Length == 0)
            {
                return;
            }
            target = waypoints[currentPoint];
            direction = (target.position - transform.position).normalized;
            angle = Vector3.SignedAngle(-transform.forward, direction, Vector3.up);
            steer = Mathf.Clamp(angle / 30f, -1f, 1f);
            frontLeftWheel.steerAngle = 30 * steer;
            frontRightWheel.steerAngle = 30 * steer;
            currentMotorTorque = 500f;
            brakeForce = 0f;
            if (currentPoint + 1 < waypoints.Length)
            {
                lookAheadPos = waypoints[currentPoint + 1].position;
                lookAheadDir = (lookAheadPos - target.position).normalized;
                float lookAheadAngle = Vector3.Angle(direction, lookAheadDir);
                if (lookAheadAngle > 20f)
                {
                    dist = Vector3.Distance(transform.position, target.position);
                    if (dist < 10f)
                    {
                        currentMotorTorque *= 0.7f;
                    }
                }
            }
            if (Mathf.Abs(steer) > 0.3f)
            {
                currentMotorTorque = Mathf.Lerp(500f, 300f, Mathf.Abs(steer));
                if (Mathf.Abs(steer) == 1f)
                {
                    brakeForce = 0;
                }
                else if (Mathf.Abs(steer) > 0.7f)
                {
                    brakeForce = 300f * Mathf.Abs(steer);
                }
            }
            frontLeftWheel.brakeTorque = brakeForce;
            frontRightWheel.brakeTorque = brakeForce;
            rearLeftWheel.brakeTorque = brakeForce;
            rearRightWheel.brakeTorque = brakeForce;
            frontLeftWheel.motorTorque = -currentMotorTorque;
            frontRightWheel.motorTorque = -currentMotorTorque;
            rearLeftWheel.motorTorque = -currentMotorTorque;
            rearRightWheel.motorTorque = -currentMotorTorque;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (((other.CompareTag("Checkpoint")) || (other.CompareTag("Finish"))) && (other.gameObject == waypoints[currentPoint].gameObject))
        {
            currentPoint++;
            if (currentPoint >= waypoints.Length)
            {
                currentPoint = 0;
                currentLap++;
            }
            if ((other.CompareTag("Finish")) && (currentLap > 3))
            {
                frontLeftWheel.motorTorque = 0;
                frontRightWheel.motorTorque = 0;
                rearLeftWheel.motorTorque = 0;
                rearRightWheel.motorTorque = 0;
                frontLeftWheel.brakeTorque = 1000f;
                frontRightWheel.brakeTorque = 1000f;
                rearLeftWheel.brakeTorque = 1000f;
                rearRightWheel.brakeTorque = 1000f;
                enabled = false;
            }
        }
    }
    void OnCollisionEnter()
    {
        timer = 0f;
        obstacle = true;
    }
}
