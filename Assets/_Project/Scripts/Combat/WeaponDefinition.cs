using UnityEngine;

// Этот атрибут добавит удобную кнопку в меню Create для создания новых пушек
[CreateAssetMenu(fileName = "NewWeaponDefinition", menuName = "VR Project/Weapon Definition", order = 1)]
public class WeaponDefinition : ScriptableObject 
{
    [Header("Core Stats")]
    [Tooltip("Урон за одно попадание")]
    public float damage = 10f;
    [Tooltip("Максимальная дальность стрельбы (в метрах)")]
    public float range = 50f;
    [Tooltip("Задержка между выстрелами в секундах")]
    public float fireRate = 0.2f;

    [Header("Ammo Settings")]
    public int maxAmmoInMagazine = 15;
    public int maxReserveAmmo = 90;

    [Header("Feedback Data")]
    public float recoilForce = 1.5f;
    [Range(0f, 1f)]
    public float hapticIntensity = 0.7f;
    public float hapticDuration = 0.15f;
    public AudioClip fireSound;
}