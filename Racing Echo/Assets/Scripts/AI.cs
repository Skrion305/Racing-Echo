using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;
    Transform target;
    Vector3 direction;
    float angle;
    float steer;
    int currentPoint = 0;
    int currentLap = 1;
    Rigidbody rb;
    float brakeForce;
    float currentMotorTorque;
    float timer = 0f;
    bool obstacle = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
            currentMotorTorque = 1300f;
            brakeForce = 0f;
            if (Mathf.Abs(steer) > 0.3f)
            {
                currentMotorTorque = Mathf.Lerp(1300f, 500f, Mathf.Abs(steer));
                if (Mathf.Abs(steer) == 1f)
                {
                    brakeForce = 0;
                }
                else if (Mathf.Abs(steer) > 0.7f)
                {
                    brakeForce = 700f * Mathf.Abs(steer);
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
            Debug.Log($"Точка: {currentPoint}, Поворот: {steer}, Тормоз: {brakeForce}, Скорость: {currentMotorTorque}");
        }
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
                    frontLeftWheel.brakeTorque = 5000f;
                    frontRightWheel.brakeTorque = 5000f;
                    rearLeftWheel.brakeTorque = 5000f;
                    rearRightWheel.brakeTorque = 5000f;
                    enabled = false;
                }
            }
        }
    }
    void OnCollisionEnter()
    {
        if (rb.linearVelocity.magnitude < 10f)
        {
            obstacle = true;
        }
    }
}
