using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorElectricityManager : MonoBehaviour
{
    public bool ElectricityActive = true;
    public GameObject[] ElectricLines;

	// Use this for initialization
	void Start ()
    {
        //for(int i = 0; i < transform.childCount; ++i)
        //{
        //    ElectricLines.Add(transform.GetChild(i).gameObject);
        //}
	}

    public void Activate()
    {
        ElectricityActive = true;
        for(int i = 0; i < ElectricLines.Length; ++i)
        {
            ElectricLines[i].SetActive(true);
        }
    }
	
    public void Deactivate()
    {
        ElectricityActive = false;
        for(int i = 0; i < ElectricLines.Length; ++i)
        {
            ElectricLines[i].SetActive(false);
        }
    }

	//// Update is called once per frame
	//void Update ()
    //{
	//	
	//}
}
