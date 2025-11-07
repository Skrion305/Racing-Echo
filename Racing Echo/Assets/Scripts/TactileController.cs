using UnityEngine;
using Bhaptics.SDK2;
using LogitechG29.Sample.Input;

public class TactileController : MonoBehaviour
{
    [SerializeField] InputControllerReader inputControllerReader;
    Rigidbody carRigidbody;
    bool isGrounded = true;
    [SerializeField] CarTelemetryHandler carTelemetryHandler;
    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        CheckEngineVibration();
        CheckSuspensionVibration();
        CheckBrakingFeedback();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            PlayCarAccident();
        }
        else
        {
            PlayCollisionImpact();
        }
        carTelemetryHandler.TriggerBraking(2);
        Debug.Log("3");
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Road"))
        {
            isGrounded = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Road"))
        {
            isGrounded = false;
        }
        else if (other.CompareTag("Springboard"))
        {
            PlayJumpLaunch();
        }
    }
    void CheckEngineVibration()
    {
        float engineIntensity;
        float throttleInput = Mathf.Abs(inputControllerReader.Throttle);
        float brakeInput = Mathf.Abs(inputControllerReader.Brake);
        if (throttleInput > brakeInput)
        {
            engineIntensity = Mathf.Clamp01(inputControllerReader.Throttle);
        }
        else
        {
            engineIntensity = Mathf.Clamp01(inputControllerReader.Brake);
        }
        if (engineIntensity > 0.1f)
        {
            BhapticsLibrary.Play(eventId: "engine_vibration", startMillis: 0, intensity: engineIntensity * 0.7f, duration: 100, angleX: 0, offsetY: 0);
        }
    }
    void CheckSuspensionVibration()
    {
        float speed = carRigidbody.linearVelocity.magnitude;
        if (isGrounded && (speed > 3f))
        {
            BhapticsLibrary.Play(eventId: "landing_impact", startMillis: 0, intensity: Mathf.Clamp01(speed / 20f), duration: 300, angleX: 0, offsetY: 0);
        }
    }
    void CheckBrakingFeedback()
    {
        if (inputControllerReader.Brake > 0.8f)
        {
            BhapticsLibrary.Play(eventId: "brake_feedback", startMillis: 0, intensity: 1, duration: 200, angleX: 0, offsetY: 0);
        }
    }
    void PlayCollisionImpact()
    {
        BhapticsLibrary.Play(eventId: "collision_impact", startMillis: 0, intensity: 1, duration: 400, angleX: 0, offsetY: 0);
    }
    public void PlayUIButtonClick()
    {
        BhapticsLibrary.Play(eventId: "ui_button_click", startMillis: 0, intensity: 0.6f, duration: 150, angleX: 0, offsetY: 0);
    }
    void PlayCarAccident()
    {
        BhapticsLibrary.Play(eventId: "car_accident", startMillis: 0, intensity: 1, duration: 600, angleX: 0, offsetY: 0);
    }
    public void PlayJumpLaunch()
    {
        BhapticsLibrary.Play(eventId: "jump_liftoff", startMillis: 100, intensity: 0.7f, duration: 300, angleX: 0, offsetY: 0);
    }
    void OnDestroy()
    {
        BhapticsLibrary.StopAll();
    }
}
