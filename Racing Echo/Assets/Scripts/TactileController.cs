using UnityEngine;
using Bhaptics.SDK2;
using LogitechG29.Sample.Input;

public class TactileController : MonoBehaviour
{
    [SerializeField] InputControllerReader inputControllerReader;
    Rigidbody carRigidbody;
    //bool wasGrounded = true;
    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        CheckEngineVibration();
        //CheckSuspensionVibration();
        CheckBrakingFeedback();
    }
    void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        if (impactForce > 5f)
        {
            PlayCollisionImpact(impactForce);
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
            BhapticsLibrary.Play(eventId: "engine_vibration", intensity: engineIntensity * 0.7f, duration: 100, angleX: 0, offsetY: 0);
        }
    }
    /*void CheckSuspensionVibration()
    {
        float speed = carRigidbody.linearVelocity.magnitude;
        bool isGrounded = CheckWheelGrounded();
        if (!isGrounded && wasGrounded && (speed > 2f))
        {
            BhapticsLibrary.Play(eventId: "landing_impact", startMillis: 0, intensity: Mathf.Clamp01(speed / 20f), duration: 300, angleX: 0, offsetY: 0);
        }
        wasGrounded = isGrounded;
    }*/
    void CheckBrakingFeedback()
    {
        if (inputControllerReader.Brake > 0.8f)
        {
            BhapticsLibrary.Play(eventId: "brake_feedback", startMillis: 0, intensity: 1f, duration: 200, angleX: 0, offsetY: 0);
        }
    }
    void PlayCollisionImpact(float force)
    {
        float intensity = Mathf.Clamp01(force / 20f);
        BhapticsLibrary.Play(eventId: "collision_impact", startMillis: 0, intensity: intensity, duration: 400, angleX: 0, offsetY: 0);
    }
    public void PlayUIButtonClick()
    {
        BhapticsLibrary.Play(eventId: "ui_button_click", startMillis: 0, intensity: 0.6f, duration: 150, angleX: 0, offsetY: 0);
    }
    void OnDestroy()
    {
        BhapticsLibrary.StopAll();
    }
}
