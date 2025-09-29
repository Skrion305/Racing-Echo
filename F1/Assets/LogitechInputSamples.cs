using LogitechG29.Sample.Input;
using UnityEngine;

public class LogitechInputSamples : MonoBehaviour
{
    [Header("Компоненты")]
    public InputControllerReader inputControllerReader;
    Rigidbody carRigidbody;
    [Header("Колёса")]
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;
    [Header("Трансформы визуальных колёс")]
    public Transform frontLeftVisual;
    public Transform frontRightVisual;
    public Transform rearLeftVisual;
    public Transform rearRightVisual;
    [Header("Настройки автомобиля")]
    [SerializeField] float maxMotorTorque = 1500f;
    [SerializeField] float maxSteeringAngle = 30f;
    [SerializeField] float brakeTorque = 2000f;
    [Header("Физические настройки")]
    [SerializeField] float centerOfMassOffset = -0.5f;
    [SerializeField] float steeringSensitivity = 1.5f;
    float currentSteering;
    float currentThrottle;
    float currentBrake;
    float currentClutch;
    int currentGear = 1;
    float[] gearRatios = { 0f, 3.5f, 2.5f, 1.8f, 1.3f, 1.0f, 0.8f };
    float finalDriveRatio = 3.5f;
    float engineRPM;
    float maxEngineRPM = 7000f;
    float minEngineRPM = 1000f;
    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        carRigidbody.centerOfMass = new Vector3(0, centerOfMassOffset, 0);
        if (inputControllerReader != null)
        {
            SetupInputCallbacks();
        }
        else
        {
            Debug.LogError("InputControllerReader �� ��������!");
        }
    }
    void SetupInputCallbacks()
    {
        inputControllerReader.OnNorthButtonCallback += OnShiftUp;
        inputControllerReader.OnSouthButtonCallback += OnShiftDown;
        inputControllerReader.SetDebugMode(true);
    }
    void Update()
    {
        GetG29Input();
        UpdateWheelVisuals();
        HandleGearShift();
        CalculateEngineRPM();
    }
    void FixedUpdate()
    {
        ApplySteering();
        ApplyMotorTorque();
        ApplyBrakes();
    }
    void GetG29Input()
    {
        currentSteering = inputControllerReader.Steering * steeringSensitivity;
        currentThrottle = inputControllerReader.Throttle;
        currentBrake = inputControllerReader.Brake;
        currentClutch = inputControllerReader.Clutch;
        currentSteering = Mathf.Clamp(currentSteering, -1f, 1f);
    }
    void ApplySteering()
    {
        float steeringAngle = currentSteering * maxSteeringAngle;
        frontLeftWheel.steerAngle = steeringAngle;
        frontRightWheel.steerAngle = steeringAngle;
    }
    void ApplyMotorTorque()
    {
        float torque = 0f;
        if (currentGear != 0 && currentClutch < 0.5f)
        {
            float effectiveThrottle = currentThrottle * (1f - currentClutch);
            torque = effectiveThrottle * maxMotorTorque * gearRatios[Mathf.Abs(currentGear)] * finalDriveRatio;
            if (engineRPM > maxEngineRPM)
            {
                torque = 0f;
            }
        }
        rearLeftWheel.motorTorque = torque;
        rearRightWheel.motorTorque = torque;
    }
    void ApplyBrakes()
    {
        float brakeForce = currentBrake * brakeTorque;
        frontLeftWheel.brakeTorque = brakeForce;
        frontRightWheel.brakeTorque = brakeForce;
        rearLeftWheel.brakeTorque = brakeForce;
        rearRightWheel.brakeTorque = brakeForce;
    }
    void HandleGearShift()
    {
        if (currentClutch > 0.8f && currentGear != 0)
        {
            currentGear = 0;
            Debug.Log("����������� �� ��������");
        }
    }
    void OnShiftUp(bool pressed)
    {
        if (pressed && currentClutch > 0.5f)
        {
            if (currentGear < gearRatios.Length - 2)
            {
                currentGear++;
                Debug.Log($"����������� �� ��������: {currentGear}");
            }
        }
    }
    void OnShiftDown(bool pressed)
    {
        if (pressed && currentClutch > 0.5f)
        {
            if (currentGear > -1)
            {
                currentGear--;
                if (currentGear == 0) currentGear = -1;
                Debug.Log($"����������� �� ��������: {(currentGear == -1 ? "R" : currentGear.ToString())}");
            }
        }
    }
    void CalculateEngineRPM()
    {
        float wheelRPM = (rearLeftWheel.rpm + rearRightWheel.rpm) * 0.5f;
        if (currentGear != 0 && currentClutch < 0.3f)
        {
            engineRPM = Mathf.Abs(wheelRPM * gearRatios[Mathf.Abs(currentGear)] * finalDriveRatio);
        }
        else
        {
            engineRPM = Mathf.Lerp(engineRPM, minEngineRPM, Time.deltaTime);
        }
        engineRPM = Mathf.Clamp(engineRPM, 0, maxEngineRPM);
    }
    void UpdateWheelVisuals()
    {
        UpdateWheelTransform(frontLeftWheel, frontLeftVisual);
        UpdateWheelTransform(frontRightWheel, frontRightVisual);
        UpdateWheelTransform(rearLeftWheel, rearLeftVisual);
        UpdateWheelTransform(rearRightWheel, rearRightVisual);
    }
    void UpdateWheelTransform(WheelCollider collider, Transform visual)
    {
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        visual.position = position;
        visual.rotation = rotation;
    }
    public float GetSpeedKMH()
    {
        return carRigidbody.linearVelocity.magnitude * 3.6f;
    }
    public int GetCurrentGear()
    {
        return currentGear;
    }
    public float GetEngineRPM()
    {
        return engineRPM;
    }
    public float GetSteeringInput()
    {
        return currentSteering;
    }
    void OnDestroy()
    {
        if (inputControllerReader != null)
        {
            inputControllerReader.OnNorthButtonCallback -= OnShiftUp;
            inputControllerReader.OnSouthButtonCallback -= OnShiftDown;
        }
    }
}
