using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodDoor : MonoBehaviour {

    public int swingDir = 1;
    public float swingSpeed = 0.8f;
    public float openLimit = 90f;
    public Transform hinge;
    public Transform handle;
    public int currentState;
    private DoorSwingState state = new Shut();
    private bool inputSpamming = false;
    private int prevstate;
    private float baseRot;

    public GameObject LockObject;
    private GameObject LockReference;
    public bool locked = false;
    public string code = "CHEATER";

    [Header("Room Number to be used by speakers")]
    [Tooltip("0 is null\n1 is tutorial room")]
    public int roomNumber = 0;

    [Header("Exit Door to be used by final speaker")]
    [Tooltip("0 is null\n")]
    public int exitNumber = 0;

    bool active = false;

    public bool isActive()
    {
        return active;
    }

    public void SetCode(string input)
    {
        locked = true;
        code = input;
        ElectricBarrier();
    }

    public string GetCode()
    {
        return code;
    }

    void Start()
    {
        ElectricBarrier();
        baseRot = transform.localEulerAngles.y;
        active = false; //This may be something of a problem. Not sure why this started happening.
    }

    public bool Unlock(string input)
    {
        if(locked)
        {
            //Debug.Log(code + " vs. " + input);
            if(input.Contains(code))
            {
                locked = false;
                ElectricBarrier();
                return true;
            }
        }
        return false;
    }

    void Activate()
    {
        active = !active;
    }  
    
    void ElectricBarrier()
    {
        if(locked)
        {
            if(LockReference == null)
            {
                Debug.Log("Creating Lock");
                LockReference = Instantiate(LockObject, this.transform.position, this.transform.rotation);
                LockReference.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
        }
        else
        {
            if(LockReference != null)
            {
                Debug.Log("Releasing Lock");
                LockReference.SetActive(false);
                Destroy(LockReference);
            }
        }
    }

	// Update is called once per frame
	void Update ()
    {
        currentState = state.state();
        if (active && !locked && Input.GetKeyDown("e"))
        {
            if (!inputSpamming)
            {
                inputSpamming = true;
                if (currentState % 2 == 1)
                    state = new SwingingShut(swingDir, swingSpeed, openLimit, hinge, transform, baseRot);
                else
                    state = new SwingingOpen(swingDir, swingSpeed, openLimit, hinge, transform, baseRot);
            }

            //ik right hand to handle
        }
        else
            inputSpamming = false;

        if (currentState <= 2 || active)
        {
            int nextstate = state.update();

            if (prevstate != nextstate)
            {
                switch (nextstate)
                {
                    case 1:
                        state = new SwingingOpen(swingDir, swingSpeed, openLimit, hinge, transform, baseRot);
                        break;
                    case 2:
                        state = new SwingingShut(swingDir, swingSpeed, openLimit, hinge, transform, baseRot);
                        break;
                    case 3:
                        state = new Open();
                        break;
                    case 4:
                        state = new Shut();
                        break;
                    default:
                        break;
                }
            }
            prevstate = nextstate;
        }
    }


    private class DoorSwingState
    {
        public virtual int update() { return -1; }
        public virtual int state() { return -1; }
        public DoorSwingState(int swingD, float swingS, float openL, Transform h, Transform d, float bR)
        {
            swingDir = swingD;
            swingSpeed = swingS;
            openLimit = openL;
            hinge = h;
            door = d;
            baseRot = bR;
        }
        public DoorSwingState() { }
        //vars
        protected int swingDir;
        protected float swingSpeed;
        protected float openLimit;
        protected Transform hinge;
        protected Transform door;
        protected float baseRot;
    }

    private class SwingingOpen : DoorSwingState
    {
        public override int update()
        {
            door.RotateAround(hinge.position, hinge.up, swingDir * swingSpeed * Time.deltaTime);
            if (swingDir > 0 ? door.localRotation.eulerAngles.y - baseRot >= openLimit : door.localRotation.eulerAngles.y - baseRot <= openLimit)
            {
                return 3;
            }
            return 0;
        }
        public override int state() { return 1; }
        public SwingingOpen(int swingD, float swingS, float openL, Transform h, Transform d, float bR)
        {
            swingDir = swingD;
            swingSpeed = swingS;
            openLimit = openL;
            hinge = h;
            door = d;
            baseRot = bR;
        }
    }

    private class SwingingShut : DoorSwingState
    {
        public override int update()
        {
            door.RotateAround(hinge.position, hinge.up, -swingDir * swingSpeed * Time.deltaTime);
            if (door.localRotation.eulerAngles.y >= baseRot - 1 && door.localRotation.eulerAngles.y <= baseRot + 1)
            {
                return 4;
            }
            return 0;
        }
        public override int state() { return 2; }
        public SwingingShut(int swingD, float swingS, float openL, Transform h, Transform d, float bR)
        {
            swingDir = swingD;
            swingSpeed = swingS;
            openLimit = openL;
            hinge = h;
            door = d;
            baseRot = bR;
        }
    }

    private class Open : DoorSwingState
    {
        public override int update()
        {
            return 0;
        }
        public override int state() { return 3; }
    }

    private class Shut : DoorSwingState
    {
        public override int update()
        {
            return 0;
        }
        public override int state() { return 4; }
    }
}
