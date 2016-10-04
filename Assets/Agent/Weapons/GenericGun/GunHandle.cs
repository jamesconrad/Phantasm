using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.Networking.Match;

public class GunHandle : NetworkBehaviour
{
    public GunSettings weaponSettings;

	public GameObject gunReference;
     
    private RaycastHit raycastResult;

    private Transform playerTransform;

    // Use this for initialization
    void Start()
    {
        if (GetComponentInChildren<Gun>())
        {
            gunReference = GetComponentInChildren<Gun>().gameObject;
            weaponSettings = GetComponentInChildren<Gun>().weaponSettings;

            //temp solution. I'd like to get this automated.
            weaponSettings.currentNumberOfClips = GetComponentInChildren<Gun>().weaponSettings.ammoSettings.startingNumberOfClips;
            weaponSettings.currentNumberOfRounds = GetComponentInChildren<Gun>().weaponSettings.ammoSettings.startingNumberOfRounds;
            
        }
    }

    // Called on clients for player objects for the local client (only)
    public override void OnStartLocalPlayer()
    {
        playerTransform = GetComponent<Transform>();

        weaponSettings.currentNumberOfClips = weaponSettings.ammoSettings.startingNumberOfClips;
        weaponSettings.currentNumberOfRounds = weaponSettings.ammoSettings.startingNumberOfRounds;

        Cursor.lockState = CursorLockMode.Locked;

        //This needs to be seperated into it's own function call so that new weapons can be picked up automatically, in case we choose to have multiple weapons.
        if (GetComponentInChildren<Gun>())
        {
            gunReference = GetComponentInChildren<Gun>().gameObject;
            weaponSettings = GetComponentInChildren<Gun>().weaponSettings;

            //temp solution. I'd like to get this automated.
            weaponSettings.currentNumberOfClips = GetComponentInChildren<Gun>().weaponSettings.ammoSettings.startingNumberOfClips;
            weaponSettings.currentNumberOfRounds = GetComponentInChildren<Gun>().weaponSettings.ammoSettings.startingNumberOfRounds;

            gunReference.transform.parent = Camera.main.transform;
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

		#if UNITY_EDITOR
		if(!weaponSettings.Equals(gunReference.GetComponent<Gun>().weaponSettings))
		{
			weaponSettings = gunReference.GetComponent<Gun>().weaponSettings;
		}
		#endif

		Physics.Raycast(gunReference.transform.position + gunReference.transform.rotation * weaponSettings.barrelOffset, Camera.main.transform.forward, out raycastResult);

		if (raycastResult.collider)
		{
            gunReference.transform.rotation = Quaternion.LookRotation((raycastResult.point - (gunReference.transform.position + gunReference.transform.rotation * weaponSettings.barrelOffset)).normalized);// Quaternion.Slerp(gunReference.transform.rotation, Quaternion.LookRotation((raycastResult.point - (gunReference.transform.position + gunReference.transform.rotation * weaponSettings.barrelOffset)).normalized), 1.0f);
		}
		else
		{
            gunReference.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);// Quaternion.Slerp(gunReference.transform.rotation, Quaternion.LookRotation(Camera.main.transform.forward), 1.0f);
		}

        if ((Input.GetButtonDown("GamePad Fire") || Input.GetButtonDown("Fire1")) && weaponSettings.bulletPrefab != null && weaponSettings.currentNumberOfRounds > 0)
        {
            CmdFireWeapon(gunReference.transform.position + gunReference.transform.rotation * weaponSettings.barrelOffset, gunReference.transform.rotation);
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

		if (Input.GetMouseButtonDown(0))
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
        if (Input.GetMouseButtonDown(1))
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
