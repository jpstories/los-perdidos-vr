using UnityEngine;

public class Magazine : MonoBehaviour
{
    [Header("Data Reference")]
    public WeaponDefinition weaponData;

    [Header("State")]
    public int currentAmmo;

    private void Awake()
    {
        if (weaponData != null)
        {
            currentAmmo = weaponData.maxAmmoInMagazine;
        }
    }
}