using UnityEngine;
using LogitechG29.Sample.Input;

public class Platform : MonoBehaviour
{
    [SerializeField] CarTelemetryHandler carTelemetryHandler;
    [SerializeField] InputControllerReader inputControllerReader;
    Rigidbody rb;
    float speed;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        speed = rb.linearVelocity.magnitude;
        if ((Mathf.Abs(inputControllerReader.Steering) == 1) && (speed > 10))
        {
            carTelemetryHandler.TriggerSharpTurn(1.5f);
            Debug.Log("1");
        }
        if ((speed < 1) && (inputControllerReader.Throttle == 1))
        {
            carTelemetryHandler.TriggerAcceleration(2);
            Debug.Log("2");
        }
        if ((speed > 5) && (inputControllerReader.Brake == 1))
        {
            carTelemetryHandler.TriggerBraking(2);
            Debug.Log("3");
        }
    }
}
