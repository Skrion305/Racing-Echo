using UnityEngine;
using LogitechG29.Sample.Input;

public class Platform : MonoBehaviour
{
    [SerializeField] CarTelemetryHandler carTelemetryHandler;
    [SerializeField] InputControllerReader inputControllerReader;
    Rigidbody rb;
    float currentSpeed = 0;
    float lastSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        lastSpeed = currentSpeed;
        currentSpeed = rb.linearVelocity.magnitude;
        if ((Mathf.Abs(inputControllerReader.Steering) == 1) && (currentSpeed > 10))
        {
            carTelemetryHandler.TriggerSharpTurn(1.5f);
        }
        if ((currentSpeed < 1) && (inputControllerReader.Throttle == 1))
        {
            carTelemetryHandler.TriggerAcceleration(2);
        }
        if ((currentSpeed > 5) && (currentSpeed <= lastSpeed) && (inputControllerReader.Brake == 1))
        {
            carTelemetryHandler.TriggerBraking(2);
        }
    }
}
