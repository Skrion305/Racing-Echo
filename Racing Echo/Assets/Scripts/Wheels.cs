using UnityEngine;
using Bhaptics.SDK2;

public class Wheels : MonoBehaviour
{
    [SerializeField] Rigidbody carRigidbody;
    bool isGrounded = true;
    void Start()
    {
    }
    void Update()
    {
        CheckSuspensionVibration();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Road"))
        {
            isGrounded = true;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Road"))
        {
            isGrounded = false;
        }
    }
    void CheckSuspensionVibration()
    {
        float speed = carRigidbody.linearVelocity.magnitude;
        if (isGrounded && (speed > 2f))
        {
            BhapticsLibrary.Play(eventId: "landing_impact", startMillis: 0, intensity: Mathf.Clamp01(speed / 20f), duration: 300, angleX: 0, offsetY: 0);
            Debug.Log("1");
        }
    }
}
