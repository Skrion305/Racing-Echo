using System;
using System.Collections;
using _2DOF;
using UnityEngine;
using LogitechG29.Sample.Input;

public class CarTelemetryHandler : MonoBehaviour
{
    private const float WAIT_TIME = SendingData.WAIT_TIME / 1000f;

    [SerializeField] private Transform vehicleTransform;
    [SerializeField] private Rigidbody rigidbody;

    private ObjectTelemetryData _telemetryDataData;
    private SendingData _sendingData;
    Vector3 lastVelocity;
    [SerializeField] InputControllerReader inputControllerReader;
    private void Awake()
    {
        _sendingData = new SendingData();
        _telemetryDataData = _sendingData.ObjectTelemetryData;
        lastVelocity = Vector3.zero;
    }

    public void OnEnable()
    {
        StartCoroutine(TelemetryHandler());
        _sendingData.SendingStart();
    }

    public void OnDisable()
    {
        StopAllCoroutines();
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
        Vector3 currentVelocity = rigidbody.linearVelocity;
        Vector3 acceleration = (currentVelocity - lastVelocity) / Time.deltaTime;
        Vector3 angles = new Vector3(Mathf.Clamp(acceleration.z, -15f, 15f), 0f, Mathf.Clamp(-inputControllerReader.Steering * 2f, -15f, 15f));
        _telemetryDataData.Angles = angles;
        lastVelocity = currentVelocity;
    }
    public void TriggerSharpTurn(float intensity)
    {
        StartCoroutine(SharpTurn(intensity));
    }
    public void TriggerAcceleration(float intensity)
    {
        StartCoroutine(Acceleration(intensity));
    }
    public void TriggerBraking(float intensity)
    {
        StartCoroutine(Braking(intensity));
    }
    IEnumerator SharpTurn(float intensity)
    {
        Vector3 originalAngles = _telemetryDataData.Angles;
        _telemetryDataData.Angles = new Vector3(originalAngles.x, 0, Mathf.Clamp(intensity * 10f, -15f, 15f));
        yield return new WaitForSeconds(0.5f);
        _telemetryDataData.Angles = originalAngles;
    }
    IEnumerator Acceleration(float intensity)
    {
        Vector3 originalAngles = _telemetryDataData.Angles;
        _telemetryDataData.Angles = new Vector3(Mathf.Clamp(intensity * 8f, -15f, 15f), 0, originalAngles.z);
        yield return new WaitForSeconds(0.3f);
        _telemetryDataData.Angles = originalAngles;
    }
    private IEnumerator Braking(float intensity)
    {
        Vector3 originalAngles = _telemetryDataData.Angles;
        _telemetryDataData.Angles = new Vector3(Mathf.Clamp(-intensity * 8f, -15f, 15f), 0, originalAngles.z);
        yield return new WaitForSeconds(0.3f);
        _telemetryDataData.Angles = originalAngles;
    }
}