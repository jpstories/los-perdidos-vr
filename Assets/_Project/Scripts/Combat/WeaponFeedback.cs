using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRGrabInteractable))]
public class WeaponFeedback : MonoBehaviour
{
    [Header("Audio Component")]
    public AudioSource audioSource;

    [Header("Visual Effects (Optional)")]
    public ParticleSystem muzzleFlash;

    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayFireFeedback(WeaponDefinition data)
    {
        if (data == null) return;

        // 1. Воспроизведение звука
        if (audioSource != null && data.fireSound != null)
        {
            audioSource.PlayOneShot(data.fireSound);
        }

        // 2. Вспышка выстрела
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // 3. Отдача (вибрация контроллера)
        SendHapticImpulse(data.hapticIntensity, data.hapticDuration);
    }

    private void SendHapticImpulse(float amplitude, float duration)
    {
        if (grabInteractable == null || amplitude <= 0f) return;

        foreach (var interactor in grabInteractable.interactorsSelecting)
        {
            // Канал 1: Через системы XRI
            if (interactor is XRDirectInteractor direct)
            {
                direct.SendHapticImpulse(amplitude, duration);
            }
            else if (interactor is XRRayInteractor ray)
            {
                ray.SendHapticImpulse(amplitude, duration);
            }

            // Канал 2: Прямая отправка импульса на железо Oculus/OpenXR (дублирующий вариант)
            XRNode handNode = interactor.transform.name.ToLower().Contains("left") ? XRNode.LeftHand : XRNode.RightHand;
            InputDevice device = InputDevices.GetDeviceAtXRNode(handNode);

            if (device.isValid && device.TryGetHapticCapabilities(out HapticCapabilities caps) && caps.supportsImpulse)
            {
                device.SendHapticImpulse(0, amplitude, duration);
            }
        }
    }
}