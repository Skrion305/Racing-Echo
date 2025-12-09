using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] GameObject firstTrackCheckpoints;
    [SerializeField] GameObject secondTrackCheckpoints;
    [SerializeField] GameObject thirdTrackCheckpoints;
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
    [SerializeField] int currentPoint = 0;
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
        if (Settings.raceTrack == 1)
        {
            waypoints = checkpoints1;
            Destroy(secondTrackCheckpoints);
            Destroy(thirdTrackCheckpoints);
        }
        if (Settings.raceTrack == 2)
        {
            waypoints = checkpoints2;
            Destroy(firstTrackCheckpoints);
            Destroy(thirdTrackCheckpoints);
        }
        if (Settings.raceTrack == 3)
        {
            waypoints = checkpoints3;
            Destroy(firstTrackCheckpoints);
            Destroy(secondTrackCheckpoints);
        }
    }
    void FixedUpdate()
    {
        if (obstacle)
        {
            if (timer < 1f)
            {
                frontLeftWheel.motorTorque = 700f;
                frontRightWheel.motorTorque = 700f;
                rearLeftWheel.motorTorque = 700f;
                rearRightWheel.motorTorque = 700f;
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
            currentMotorTorque = 2000f;
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
                        currentMotorTorque *= 0.5f;
                    }
                }
            }
            if (Mathf.Abs(steer) > 0.3f)
            {
                currentMotorTorque = Mathf.Lerp(2000f, 500f, Mathf.Abs(steer));
                if (Mathf.Abs(steer) == 1f)
                {
                    brakeForce = 0;
                }
                else if (Mathf.Abs(steer) > 0.7f)
                {
                    brakeForce = 1000f * Mathf.Abs(steer);
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
                frontLeftWheel.brakeTorque = 5000f;
                frontRightWheel.brakeTorque = 5000f;
                rearLeftWheel.brakeTorque = 5000f;
                rearRightWheel.brakeTorque = 5000f;
                enabled = false;
            }
        }
    }
    void OnCollisionEnter()
    {
        if ((rb.linearVelocity.magnitude < 10f) || (currentMotorTorque < 700f))
        {
            timer = 0f;
            obstacle = true;
        }
    }
}
