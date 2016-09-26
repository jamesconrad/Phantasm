using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GunHandle : NetworkBehaviour
{
    public GunSettings weaponSettings;
     
    private RaycastHit raycastResult;

    private Transform playerTransform;
    
    // Use this for initialization
    void Start()
    {
        
    }

    // Called on clients for player objects for the local client (only)
    public override void OnStartLocalPlayer()
    {
        playerTransform = GetComponent<Transform>();

        weaponSettings.currentNumberOfClips = weaponSettings.ammoSettings.startingNumberOfClips;
        weaponSettings.currentNumberOfRounds = weaponSettings.ammoSettings.startingNumberOfRounds;

        Cursor.lockState = CursorLockMode.Locked;

        if (GetComponentInChildren<Gun>())
        {
            weaponSettings = GetComponentInChildren<Gun>().weaponSettings;

            //temp solution. I'd like to get this automated.
            weaponSettings.currentNumberOfClips = GetComponentInChildren<Gun>().weaponSettings.ammoSettings.startingNumberOfClips;
            weaponSettings.currentNumberOfRounds = GetComponentInChildren<Gun>().weaponSettings.ammoSettings.startingNumberOfRounds;
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }


        Physics.Raycast(playerTransform.position + playerTransform.rotation * weaponSettings.barrelOffset, playerTransform.forward, out raycastResult);

        if ((Input.GetButtonDown("GamePad Fire") || Input.GetButtonDown("Fire1")) && weaponSettings.bulletPrefab != null && weaponSettings.currentNumberOfRounds > 0)
        {
            Quaternion tempQuat = Quaternion.identity;
            if (raycastResult.collider)
            {
                GetComponentInChildren<Transform>().rotation = Quaternion.LookRotation(raycastResult.point - transform.position);
                tempQuat = Quaternion.LookRotation((raycastResult.point - (transform.position + transform.rotation * weaponSettings.barrelOffset)).normalized);
                CmdFireWeapon(transform.position + transform.rotation * weaponSettings.barrelOffset, tempQuat);
            }
            else
            {
                GetComponentInChildren<Transform>().rotation = Quaternion.LookRotation(transform.forward);
                tempQuat = Quaternion.LookRotation(transform.forward);
                CmdFireWeapon(transform.position + transform.rotation * weaponSettings.barrelOffset, tempQuat);
            }
            weaponSettings.currentNumberOfRounds--;

            if (weaponSettings.Hitscan)
            {

                {
                    //Add in some tag related collision stuff here.

                }
            }
        }
        if (Input.GetButtonDown("Reload") || Input.GetButtonDown("GamePad Reload"))
        {
            if (weaponSettings.currentNumberOfClips > 0)
            {
                weaponSettings.currentNumberOfClips--;
                weaponSettings.currentNumberOfRounds = weaponSettings.ammoSettings.maxClipSize;
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
        GameObject tempBullet = (GameObject)Instantiate(weaponSettings.bulletPrefab, spawnPosition, spawnRotation);
        NetworkServer.Spawn(tempBullet);
        
    }

}
