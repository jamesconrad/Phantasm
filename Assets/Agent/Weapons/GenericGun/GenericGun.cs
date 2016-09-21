using UnityEngine;
using System.Collections;

public class GenericGun : MonoBehaviour
{


    public GameObject bulletPrefab;

    [Tooltip("Offset of the barrel from the player's transform.")]
    public Vector3 barrelOffset;

    [Tooltip("Change between object spawning and raytrace. If true, just have a particle system as the prefab, to create some effect")]
    public bool Hitscan;
    private RaycastHit raycastResult;

    private Transform playerTransform;

    [System.Serializable]
    public struct AmmoSettings
    {
        public int maxClipSize;
        public int startingNumberOfRounds;
        public int maxClips;
        public int startingNumberOfClips;
    }

    private int currentNumberOfRounds;
    private int currentNumberOfClips;

    public AmmoSettings ammoSettings;


    // Use this for initialization
    void Start()
    {
        playerTransform = GetComponent<Transform>();

        currentNumberOfClips = ammoSettings.startingNumberOfClips;
        currentNumberOfRounds = ammoSettings.startingNumberOfRounds;
    }

    // Update is called once per frame
    void Update()
    {
		if ((Input.GetButtonDown("GamePad Fire") || Input.GetButtonDown("Fire1")) && bulletPrefab != null && currentNumberOfRounds > 0)
        {
            Instantiate(bulletPrefab, playerTransform.position + playerTransform.rotation * barrelOffset, playerTransform.rotation);
            currentNumberOfRounds--;
        }

		if (Input.GetButtonDown("Reload") || Input.GetButtonDown("GamePad Reload"))
        {
            if (currentNumberOfClips > 0)
            {
                currentNumberOfClips--;
                currentNumberOfRounds = ammoSettings.maxClipSize;
            }
        }

        if (Hitscan)
        {
            if (Physics.Raycast(playerTransform.position + playerTransform.rotation * barrelOffset, playerTransform.forward, out raycastResult))
            {
                //Add in some tag related collision stuff here.
               
            }
        }
    }


}
