using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(AmmoState))]
[RequireComponent(typeof(WeaponFeedback))]
public class FirearmController : MonoBehaviour
{
    [Header("Data Reference")]
    public WeaponDefinition weaponData;

    [Header("Weapon Setup")]
    public Transform muzzleTransform;
    public LayerMask hitLayers = ~0;

    [Header("Reload Setup")]
    public XRSocketInteractor magazineSocket;

    private AmmoState ammoState;
    private WeaponFeedback feedback;
    private Magazine currentMagazine;
    private float lastFireTime;

    private void Awake()
    {
        ammoState = GetComponent<AmmoState>();
        feedback = GetComponent<WeaponFeedback>();
    }

    private void OnEnable()
    {
        if (magazineSocket != null)
        {
            magazineSocket.selectEntered.AddListener(OnMagazineInserted);
            magazineSocket.selectExited.AddListener(OnMagazineRemoved);
        }
    }

    private void OnDisable()
    {
        if (magazineSocket != null)
        {
            magazineSocket.selectEntered.RemoveListener(OnMagazineInserted);
            magazineSocket.selectExited.RemoveListener(OnMagazineRemoved);
        }
    }

    private void Start()
    {
        if (weaponData != null)
            ammoState.Initialize(weaponData);
    }

    private void OnMagazineInserted(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.TryGetComponent<Magazine>(out var mag))
        {
            currentMagazine = mag;
            ammoState.currentAmmo = mag.currentAmmo;
            Debug.Log($"<color=cyan>Магазин вставлен!</color> Патронов: {ammoState.currentAmmo}");
        }
    }

    private void OnMagazineRemoved(SelectExitEventArgs args)
    {
        if (currentMagazine != null)
        {
            // Сохраняем оставшиеся патроны в физический магазин
            currentMagazine.currentAmmo = ammoState.currentAmmo;
            currentMagazine = null;
            ammoState.currentAmmo = 0;
            Debug.Log("<color=yellow>Магазин извлечен!</color>");
        }
    }

    public void Fire()
    {
        if (Time.time >= lastFireTime + weaponData.fireRate)
        {
            if (ammoState.currentAmmo > 0)
            {
                PerformShot();
            }
            else
            {
                if (feedback != null) feedback.PlayFireFeedback(null); // Звук щелчка при желании
                Debug.Log("Щёлк! Нет патронов или не вставлен магазин.");
            }
        }
    }

    private void PerformShot()
    {
        ammoState.currentAmmo--;
        if (currentMagazine != null)
        {
            currentMagazine.currentAmmo = ammoState.currentAmmo;
        }

        lastFireTime = Time.time;

        if (feedback != null && weaponData != null)
        {
            feedback.PlayFireFeedback(weaponData);
        }

        Vector3 origin = muzzleTransform != null ? muzzleTransform.position : transform.position;
        Vector3 direction = muzzleTransform != null ? muzzleTransform.forward : transform.forward;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, weaponData.range, hitLayers))
        {
            Debug.Log($"<color=green>ПОПАДАНИЕ!</color> Объект: <b>{hit.collider.name}</b> | Урон: {weaponData.damage}");
            Debug.DrawLine(origin, hit.point, Color.red, 1.0f);
        }
        else
        {
            Debug.Log($"Выстрел в воздух. Патронов в магазине: {ammoState.currentAmmo}/{weaponData.maxAmmoInMagazine}");
            Debug.DrawRay(origin, direction * weaponData.range, Color.yellow, 1.0f);
        }
    }
}