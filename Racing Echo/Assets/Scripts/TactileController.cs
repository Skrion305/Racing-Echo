using UnityEngine;
using Bhaptics.SDK2;
using LogitechG29.Sample.Input;

public class TactileController : MonoBehaviour
{
    [SerializeField] InputControllerReader inputControllerReader;
    Rigidbody carRigidbody;
    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
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
    void OnDestroy()
    {
        BhapticsLibrary.StopAll();
    }
}
