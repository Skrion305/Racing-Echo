using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

public class Hands : MonoBehaviour
{
    [SerializeField]
    XRInputValueReader<float> m_TriggerInput;
    [SerializeField]
    XRInputValueReader<float> m_GripInput;
    [SerializeField] Animator anim;
    void Update()
    {
        anim.SetFloat("Trigger",  m_TriggerInput.ReadValue());
        anim.SetFloat("Grip", m_GripInput.ReadValue());
    }
}
