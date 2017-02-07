using UnityEngine;
using System.Collections;

[System.Serializable]
public struct GunSettings
{
    public GameObject bulletPrefab;

    [Tooltip("Offset of the barrel from the player's transform.")]
    public Vector3 barrelOffset;

    [Tooltip("Change between object spawning and raytrace. If true, just have a particle system as the prefab, to create some effect")]
    public bool Hitscan;
    [System.Serializable]
    public struct AmmoSettings
    {
        public int maxClipSize;
        public int startingNumberOfRounds;
        public int maxClips;
        public int startingNumberOfClips;
    }

    public AmmoSettings ammoSettings;

    public int currentNumberOfRounds;
    public int currentNumberOfClips;


}

public class Gun : MonoBehaviour
{
    public GunSettings weaponSettings;

    // Start is called just before any of the Update methods is called the first time
    public void Start()
    {
        weaponSettings.currentNumberOfRounds = weaponSettings.ammoSettings.startingNumberOfRounds;
        weaponSettings.currentNumberOfClips = weaponSettings.ammoSettings.startingNumberOfClips;
    }
}
