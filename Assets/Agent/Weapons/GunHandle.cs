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

    public AudioSource gunShotSound;

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

            Camera.main.transform.parent = gunReference.transform.parent;
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        
        Physics.Raycast(gunReference.transform.position + gunReference.transform.rotation * weaponSettings.barrelOffset, gunReference.transform.forward, out raycastResult);
        //if (raycastResult.collider)
        //{
        //    Camera.main.transform.rotation = Quaternion.LookRotation((raycastResult.point - Camera.main.transform.position).normalized, Vector3.up);// Quaternion.Slerp(Camera.main.transform.rotation, Quaternion.LookRotation((raycastResult.point - Camera.main.transform.position).normalized, Vector3.up), Time.deltaTime * 2f);
        //}
        //else
        {
            Camera.main.transform.rotation = Quaternion.LookRotation(gunReference.transform.forward, Vector3.up);// Quaternion.Slerp(Camera.main.transform.rotation, Quaternion.LookRotation(gunReference.transform.forward, Vector3.up), Time.deltaTime * 2f);
        }

        if ((Input.GetButtonDown("GamePad Fire") || Input.GetButtonDown("Fire1")) && weaponSettings.bulletPrefab != null && weaponSettings.currentNumberOfRounds > 0)
        {
            CmdFireWeapon(gunReference.transform.position + gunReference.transform.rotation * weaponSettings.barrelOffset, gunReference.transform.rotation);
            GetComponent<Agent>().SetAmmoCount(weaponSettings.currentNumberOfRounds);

            if (weaponSettings.Hitscan)
            {
                {
                    //Add in some tag related collision stuff here.

                }
            }

            gunShotSound.Play();
        }
        if (Input.GetButtonDown("Reload") || Input.GetButtonDown("GamePad Reload"))
        {
            if (weaponSettings.currentNumberOfClips > 0)
            {
                weaponSettings.currentNumberOfClips--;
                weaponSettings.currentNumberOfRounds = weaponSettings.ammoSettings.maxClipSize;
                GetComponent<Agent>().SetAmmoCount(weaponSettings.currentNumberOfRounds);
            }
        }

        if (Input.GetMouseButtonDown(2))
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
        weaponSettings.currentNumberOfRounds--;
        NetworkServer.Spawn(tempBullet);

    }

    // This function is called when the MonoBehaviour will be destroyed
    public void OnDestroy()
    {
        if (gunReference)
        {
            Destroy(gunReference.gameObject);
        }
        GameState.StaticEndGame();
    }
}
