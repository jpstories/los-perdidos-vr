// AmmoState.cs - хранит состояние кол-ва патронов прямо сейчас
using UnityEngine;

public class AmmoState : MonoBehaviour
{
    [Header("Current State")]
    public int currentAmmo;
    public int currentReserve;

    // Этот метод берет данные из нашего ScriptableObject и заряжает оружие при старте
    public void Initialize(WeaponDefinition weaponData)
    {
        currentAmmo = weaponData.maxAmmoInMagazine;
        currentReserve = weaponData.maxReserveAmmo;
    }
}
