using System;
using System.Collections;
using _2DOF;
using UnityEngine;

public class CarTelemetryHandler : MonoBehaviour
{
    private const float WAIT_TIME = SendingData.WAIT_TIME / 1000f;

    [SerializeField] private Transform vehicleTransform;
    [SerializeField] private Rigidbody rigidbody;

    private ObjectTelemetryData _telemetryDataData;
    private SendingData _sendingData;
    Vector3 currentTiltAngles;
    Vector3 targetTiltAngles;

    private void Awake()
    {
        _sendingData = new SendingData();
        _telemetryDataData = _sendingData.ObjectTelemetryData;
    }

    public void OnEnable()
    {
        StartCoroutine(TelemetryHandler());
        _sendingData.SendingStart();
    }

    public void OnDisable()
    {
        StopCoroutine(TelemetryHandler());
        _sendingData.SendingStop();
    }

    private IEnumerator TelemetryHandler()
    {
        while (true)
        {
            if (_telemetryDataData == null)
            {
                yield return new WaitForSeconds(WAIT_TIME * 10f);
                continue;
            }

            UpdateAngles();
            UpdateVelocity();
            UpdatePlatformTilt();

            //Debug.Log(_telemetryDataData.ToString());

            yield return new WaitForSeconds(WAIT_TIME);
        }
    }

    private void UpdateVelocity()
    {
        _telemetryDataData.Velocity = rigidbody.linearVelocity;
    }

    private void UpdateAngles()
    {
        var euler = vehicleTransform.eulerAngles;

        euler.x = Mathf.Approximately(euler.x, 180) ? 0 : euler.x;
        euler.z = Mathf.Approximately(euler.z, 180) ? 0 : euler.z;
        euler.y = Mathf.Approximately(euler.y, 180) ? 0 : euler.y;

        euler.x = euler.x > 180 ? euler.x - 360 : euler.x;
        euler.z = euler.z > 180 ? euler.z - 360 : euler.z;
        euler.y = euler.y > 180 ? euler.y - 360 : euler.y;

        _telemetryDataData.Angles = euler;
    }
    void UpdatePlatformTilt()
    {
        Vector3 acceleration = Acceleration();
        float targetTiltX = acceleration.z * 2f;
        targetTiltX = Mathf.Clamp(targetTiltX, -15f, 15f);
        targetTiltAngles = new Vector3(targetTiltX, 0f, 0f);
        currentTiltAngles = Vector3.Lerp(currentTiltAngles, targetTiltAngles, 20f * Time.deltaTime);
        Vector3 finalAngles = CurrentVehicleAngles() + currentTiltAngles;
        _telemetryDataData.Angles = finalAngles;
    }
    Vector3 Acceleration()
    {
        Vector3 velocity = rigidbody.linearVelocity;
        Vector3 acceleration = (velocity - _telemetryDataData.Velocity) / Time.deltaTime;
        return vehicleTransform.InverseTransformDirection(acceleration);
    }
    Vector3 CurrentVehicleAngles()
    {
        var euler = vehicleTransform.eulerAngles;
        euler.x = Mathf.Approximately(euler.x, 180) ? 0 : euler.x;
        euler.z = Mathf.Approximately(euler.z, 180) ? 0 : euler.z;
        euler.y = Mathf.Approximately(euler.y, 180) ? 0 : euler.y;
        euler.x = euler.x > 180 ? euler.x - 360 : euler.x;
        euler.z = euler.z > 180 ? euler.z - 360 : euler.z;
        euler.y = euler.y > 180 ? euler.y - 360 : euler.y;
        return euler;
    }
}