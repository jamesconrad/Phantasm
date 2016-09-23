using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GenericGun : NetworkBehaviour
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
        
    }

    // Called on clients for player objects for the local client (only)
    public override void OnStartLocalPlayer()
    {
        playerTransform = GetComponent<Transform>();

        currentNumberOfClips = ammoSettings.startingNumberOfClips;
        currentNumberOfRounds = ammoSettings.startingNumberOfRounds;

        Cursor.lockState = CursorLockMode.Locked;
    }



    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }


        Physics.Raycast(playerTransform.position + playerTransform.rotation * barrelOffset, playerTransform.forward, out raycastResult);

        if ((Input.GetButtonDown("GamePad Fire") || Input.GetButtonDown("Fire1")) && bulletPrefab != null && currentNumberOfRounds > 0)
        {
            Quaternion tempQuat = Quaternion.identity;
            if (raycastResult.collider)
            {
                GetComponentInChildren<Transform>().rotation = Quaternion.LookRotation(raycastResult.point - transform.position);
                tempQuat = Quaternion.LookRotation((raycastResult.point - (transform.position + transform.rotation * barrelOffset)).normalized);
                CmdFireWeapon(transform.position + transform.rotation * barrelOffset, tempQuat);
            }
            else
            {
                GetComponentInChildren<Transform>().rotation = Quaternion.LookRotation(transform.forward);
                tempQuat = Quaternion.LookRotation(transform.forward);
                CmdFireWeapon(transform.position + transform.rotation * barrelOffset, tempQuat);
            }
            currentNumberOfRounds--;

            if (Hitscan)
            {

                {
                    //Add in some tag related collision stuff here.

                }
            }
        }
        if (Input.GetButtonDown("Reload") || Input.GetButtonDown("GamePad Reload"))
        {
            if (currentNumberOfClips > 0)
            {
                currentNumberOfClips--;
                currentNumberOfRounds = ammoSettings.maxClipSize;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    [Command]
    public void CmdFireWeapon(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject tempBullet = (GameObject)Instantiate(bulletPrefab, spawnPosition, spawnRotation);
        NetworkServer.Spawn(tempBullet);
        
    }

}
