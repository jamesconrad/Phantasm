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

    bool active = false;


    void Activate()
    {
        active = !active;
    }
    	
	// Update is called once per frame
	void Update () {
        int curstate = state.state();
        if (active && Input.GetKeyDown("e"))
        {
            if (!inputSpamming)
            {
                inputSpamming = true;
                if (curstate % 2 == 1)
                    state = new SwingingShut(swingDir, swingSpeed, openLimit, hinge, transform);
                else
                    state = new SwingingOpen(swingDir, swingSpeed, openLimit, hinge, transform);
            }

            //ik right hand to handle
        }
        else
            inputSpamming = false;

        if (curstate <= 2 || active)
        {
            int nextstate = state.update();

            if (prevstate != nextstate)
            {
                switch (nextstate)
                {
                    case 1:
                        state = new SwingingOpen(swingDir, swingSpeed, openLimit, hinge, transform);
                        break;
                    case 2:
                        state = new SwingingShut(swingDir, swingSpeed, openLimit, hinge, transform);
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
        public DoorSwingState(int swingD, float swingS, float openL, Transform h, Transform d)
        {
            swingDir = swingD;
            swingSpeed = swingS;
            openLimit = openL;
            hinge = h;
            door = d;
        }
        public DoorSwingState() { }
        //vars
        protected int swingDir;
        protected float swingSpeed;
        protected float openLimit;
        protected Transform hinge;
        protected Transform door;
    }

    private class SwingingOpen : DoorSwingState
    {
        public override int update()
        {
            door.RotateAround(hinge.position, hinge.up, swingDir * swingSpeed * Time.deltaTime);
            if (door.localRotation.eulerAngles.y >= openLimit)
            {
                return 3;
            }
            return 0;
        }
        public override int state() { return 1; }
        public SwingingOpen(int swingD, float swingS, float openL, Transform h, Transform d)
        {
            swingDir = swingD;
            swingSpeed = swingS;
            openLimit = openL;
            hinge = h;
            door = d;
        }
    }

    private class SwingingShut : DoorSwingState
    {
        public override int update()
        {
            door.RotateAround(hinge.position, hinge.up, -swingDir * swingSpeed * Time.deltaTime);
            if (door.localRotation.eulerAngles.y >= 315 && door.localRotation.eulerAngles.y <= 360)
            {
                return 4;
            }
            return 0;
        }
        public override int state() { return 2; }
        public SwingingShut(int swingD, float swingS, float openL, Transform h, Transform d)
        {
            swingDir = swingD;
            swingSpeed = swingS;
            openLimit = openL;
            hinge = h;
            door = d;
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
