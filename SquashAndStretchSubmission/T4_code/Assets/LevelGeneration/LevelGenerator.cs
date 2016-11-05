using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour
{

    public GameObject StartingRoom;
    public GameObject[] LargeRooms;
    public GameObject[] SmallRooms;
    public GameObject[] Hallways;
    public GameObject DoorFill;

    public int LevelSize;
    public int LevelSizeVariance;
    private int currentLevelSize = 0;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);
        StartingRoom = Instantiate(StartingRoom, transform) as GameObject;

        Door[] currentDoors = StartingRoom.GetComponentsInChildren<Door>(true);
        for (int i = 0; i < currentDoors.Length; i++)
        {
            currentDoors[i].gameObject.SetActive(false);
        }
        
        for (int i = 0; i < LargeRooms.Length; i++)
        {
            currentDoors = LargeRooms[i].GetComponentsInChildren<Door>(true);
            for (int j = 0; j < currentDoors.Length; j++)
            {
                currentDoors[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < Hallways.Length; i++)
        {
            currentDoors = Hallways[i].GetComponentsInChildren<Door>(true);
            for (int j = 0; j < currentDoors.Length; j++)
            {
                currentDoors[i].gameObject.SetActive(false);
            }
        }
        Random.InitState(53);
        //TODO: Move this into an on server start function and get the seed from the server.

        GenerateLevel(StartingRoom);

        LevelSize += Random.Range(-LevelSizeVariance, LevelSizeVariance);
    }

    void GenerateLevel(GameObject currentRoom)
    {
        //Look up the documentation on the unity's random class

        Door[] doors = currentRoom.GetComponentsInChildren<Door>(true);
        GameObject currentGenRoom;

        Quaternion oneEighty = Quaternion.AngleAxis(180f, Vector3.up);

        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i].gameObject.activeInHierarchy == true)
            {
                continue;
            }
            float selection = Random.Range(0.0f, 1.0f);
            if (selection > 0.4f)
            {
                currentGenRoom = Instantiate(Hallways[Random.Range(0, Hallways.Length)], currentRoom.transform) as GameObject;
            }
            else
            {
                currentGenRoom = Instantiate(LargeRooms[Random.Range(0, LargeRooms.Length)], currentRoom.transform) as GameObject;
            }

            Door[] genRoomDoors = currentGenRoom.GetComponentsInChildren<Door>(true);
            Door genRoomDoor = genRoomDoors[Random.Range(0, genRoomDoors.Length)];

            //Set the rotation of the room so that the door can rotate correctly.
            currentGenRoom.transform.rotation *= Quaternion.FromToRotation(genRoomDoor.transform.forward, doors[i].transform.forward) * Quaternion.AngleAxis(180f, Vector3.up);
            if (currentGenRoom.transform.up != Vector3.up)
            {
                currentGenRoom.transform.rotation *= Quaternion.AngleAxis(180, currentGenRoom.transform.forward);
            }

            currentGenRoom.transform.position = doors[i].transform.position;
            currentGenRoom.transform.position -= (genRoomDoor.transform.position - doors[i].transform.position);

            doors[i].gameObject.SetActive(true);
            genRoomDoor.gameObject.SetActive(true);

            currentLevelSize++;

            if (currentLevelSize < LevelSize)
            {
                GenerateLevel(currentGenRoom);
            }
            else
            {
                for (int j = 0; j < genRoomDoors.Length; j++)
                {
                    if (!genRoomDoors[j].gameObject.activeInHierarchy)
                    {
                        genRoomDoors[j].gameObject.SetActive(true);
                        Instantiate(DoorFill, genRoomDoors[j].transform.position, genRoomDoors[j].transform.rotation, genRoomDoors[j].transform);
                    }
                }
                break;
            }
        }

        if (currentLevelSize >= LevelSize)
        {
            for (int i = 0; i < doors.Length; i++)
            {
                if (!doors[i].gameObject.activeInHierarchy)
                {
                    doors[i].gameObject.SetActive(true);
                    Instantiate(DoorFill, doors[i].transform.position, doors[i].transform.rotation, doors[i].transform);
                }
            }
        }
    }
}
