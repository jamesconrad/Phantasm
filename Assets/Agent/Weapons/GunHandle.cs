using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.Networking.Match;

public class GunHandle : NetworkBehaviour
{
    public GunSettings weaponSettings;

    public GameObject gunReference;
    
    public GunLaserScript laser;

    private RaycastHit raycastResult;

    private Transform playerTransform;

    public AudioSource gunShotShootSound;
    public AudioSource gunShotReloadSound;
    public AudioSource gunShotEmptySound;

    private float timeSinceFired = 10000.0f;
    private float shootSpeed = 0.25f;

    private Vector2 gunShotAngle;
    private float gunShotAdd = 0.0f;
    private bool reloading = false;

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
        GetComponent<FirstPersonCamera>().AddCameraRotation(gunShotAngle * gunShotAdd);
        gunShotAdd = Mathf.Max(gunShotAdd - 10.0f * Time.deltaTime, 0.0f);

        //if(timeSinceFired > 0.0f && timeSinceFired < 2.0f)
        //{
        //    gunShotAdd = Random.Range(0.15f, 0.25f);
        //}
        
        timeSinceFired += Time.deltaTime;

        if (timeSinceFired > 0.0f)
        {
            if(weaponSettings.bulletPrefab != null && weaponSettings.currentNumberOfRounds > 0)
            {
                laser.active = true; 
            }
            else
            {
                laser.active = false;
            }
            reloading = false;
        }
        else
        {

        }



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

        if ((Input.GetButton("GamePad Fire") || Input.GetButton("Fire1")) && weaponSettings.bulletPrefab != null)
        {
            if (weaponSettings.currentNumberOfRounds > 0 && timeSinceFired > shootSpeed)
            {
                gunShotAngle = new Vector2(Random.Range(-2.0f, 4.0f), Random.Range(4.0f, 6.0f));
                gunShotAdd = 1.0f;
                timeSinceFired = 0.0f;

                CmdFireWeapon(gunReference.transform.position + gunReference.transform.rotation * weaponSettings.barrelOffset, gunReference.transform.rotation);
                GetComponent<Agent>().SetAmmoCount(weaponSettings.currentNumberOfRounds);

                if (weaponSettings.Hitscan)
                {
                    {
                        //Add in some tag related collision stuff here.
                    }
                }

                gunShotShootSound.pitch = Random.Range(0.90f, 1.10f);
                gunShotShootSound.Play();
            }
            else if (!gunShotEmptySound.isPlaying && weaponSettings.currentNumberOfRounds == 0)
            {
                gunShotEmptySound.pitch = Random.Range(0.90f, 1.10f);
                gunShotEmptySound.Play();
            }
        }
        if (Input.GetButtonDown("Reload") || Input.GetButtonDown("GamePad Reload"))
        {
            if (weaponSettings.currentNumberOfClips > 0 && timeSinceFired > shootSpeed)
            {
                reloading = true;
                laser.active = !reloading;

                weaponSettings.currentNumberOfClips--;
                weaponSettings.currentNumberOfRounds = weaponSettings.ammoSettings.maxClipSize;
                GetComponent<Agent>().SetAmmoCount(weaponSettings.currentNumberOfRounds);


                gunShotReloadSound.pitch = Random.Range(0.90f, 1.10f);
                gunShotReloadSound.Play();

                timeSinceFired = -1.0f;
            }
        }

        if (timeSinceFired < 0.0f)
        {
            if (timeSinceFired < -0.5f)
            {
                //gunReference.transform.rotation *= Quaternion.Euler(5.0f, 0.0f, 0.0f);
                gunReference.transform.localPosition -= new Vector3(0.0f, Time.deltaTime * 0.75f, 0.0f);
            }
            else
            {
                gunReference.transform.localPosition += new Vector3(0.0f, Time.deltaTime * 0.75f, 0.0f);
                //gunReference.transform.rotation *= Quaternion.Euler(-5.0f, 0.0f, 0.0f);
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
