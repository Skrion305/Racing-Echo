using UnityEngine;
using LogitechG29.Sample.Input;


public class CarSound : MonoBehaviour
{
    [SerializeField] InputControllerReader inputControllerReader;
    [Header("Аудио")]
    [SerializeField] AudioSource engineAudioSource;
    [SerializeField] AudioSource brakeAudioSource;
    [Header("Параметры")]
    [SerializeField] float minEnginePitch = 0.3f;
    [SerializeField] float maxEnginePitch = 1.5f;
    [SerializeField] float brakeSoundThreshold = 0.5f;
    Rigidbody carRigidbody;
    bool wasMovingForward = false;
    bool wasMovingBackward = false;
    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        engineAudioSource.Play();
    }
    void Update()
    {
        HandleEngineSound();
        HandleBrakeSound();
    }
    void HandleEngineSound()
    {
        float speed = carRigidbody.linearVelocity.magnitude;
        float pitch = Mathf.Lerp(minEnginePitch, maxEnginePitch, speed / 20f);
        engineAudioSource.pitch = pitch;
        float throttleInput = Mathf.Abs(inputControllerReader.Throttle);
        float brakeInput = Mathf.Abs(inputControllerReader.Brake);
        if (throttleInput > brakeInput)
        {
            engineAudioSource.volume = 0.3f + (throttleInput * 0.7f);
        }
        else
        {
            engineAudioSource.volume = 0.3f + (brakeInput * 0.7f);
        }
    }
    void HandleBrakeSound()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(carRigidbody.linearVelocity);
        bool isMovingForward = -localVelocity.z > brakeSoundThreshold;
        bool isMovingBackward = -localVelocity.z < -brakeSoundThreshold;
        bool isBraking = inputControllerReader.Brake > 0.1f;
        bool isReversing = inputControllerReader.Throttle > 0.1f;
        bool shouldPlayBrakeSound = (wasMovingForward && isBraking) || (wasMovingBackward && isReversing);
        if (shouldPlayBrakeSound && !brakeAudioSource.isPlaying)
        {
            brakeAudioSource.Play();
        }
        else if (!shouldPlayBrakeSound && brakeAudioSource.isPlaying)
        {
            brakeAudioSource.Stop();
        }
        wasMovingForward = isMovingForward;
        wasMovingBackward = isMovingBackward;
    }
}
