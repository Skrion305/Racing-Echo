#region

using System;
using System.Collections.Generic;
using LogitechG29.Sample.Input;
using UnityEngine;
using static CarControllerSample;

#endregion

public class CarControllerSample : MonoBehaviour
{
    [SerializeField] private InputControllerReader inputControllerReader;
    [SerializeField] private List<AxleInfo> axleInfos; // информация о каждой отдельной оси

    [SerializeField]
    private float maxMotorTorque; // максимальный крутящий момент, который двигатель может приложить к колесу

    [SerializeField] private float maxSteeringAngle; // максимальный угол поворота, который может иметь колесо

    public void FixedUpdate()
    {
        var speed = 0f;
        if (inputControllerReader.Throttle != 0)
        {
            speed = -inputControllerReader.Throttle;
        }
        else if (inputControllerReader.Brake != 0)
        {
            speed = inputControllerReader.Brake;
        }

        var motor = maxMotorTorque * speed;
        var steering = maxSteeringAngle * inputControllerReader.Steering;

        foreach (var axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }

            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            VisualWheel(axleInfo.leftWheel);
            VisualWheel(axleInfo.rightWheel);
        }
    }
    public void VisualWheel(WheelCollider collider)
    {
        Transform visualWheel = collider.transform.GetChild(0);
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    [Serializable]
    public class AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool motor; // это колесо прикреплено к мотору?
        public bool steering; // применяет ли это колесо угол поворота?
    }
}