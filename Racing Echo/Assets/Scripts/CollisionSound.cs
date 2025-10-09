using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    [Header("Аудио")]
    [SerializeField] AudioSource collisionAudioSource;
    [SerializeField] AudioClip collisionClip;
    [Header("Параметры")]
    [SerializeField] float minVolume = 0.1f;
    [SerializeField] float maxVolume = 1.0f;
    [SerializeField] float minForce = 2.0f;
    [SerializeField] float maxForce = 10.0f;
    Rigidbody carRigidbody;
    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
    }
    void OnCollisionEnter(Collision collision)
    {
        float collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        if (collisionForce > minForce)
        {
            float volume = Mathf.Clamp01((collisionForce - minForce) / (maxForce - minForce));
            volume = Mathf.Clamp(volume, minVolume, maxVolume);
            collisionAudioSource.PlayOneShot(collisionClip, volume);
        }
    }
}
