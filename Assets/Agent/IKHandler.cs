using UnityEngine;
using System.Collections;

public class IKHandler : MonoBehaviour {

    Animator anim;
    Transform me;
    Transform gun;
    GameObject player;
    Rigidbody agent;

    public float threshold;

	public bool shittyFix = true;

    private Transform lIKTar;
    private Transform rIKTar;
    private float ikWeight = 1;
    Vector3 prevFramePos;

    //public Transform lHint;
    //public Transform rHint;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        me = GetComponent<Transform>();
        gun = GetComponentInParent<GunHandle>().gunReference.transform;
        
        player = transform.parent.gameObject;
        agent = GetComponentInParent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(agent != null)
        {
            Vector3 worldMoveDir = agent.transform.position - prevFramePos;
            Vector3 localMoveDir = transform.InverseTransformDirection(worldMoveDir);
            Vector3 lookDir = agent.transform.forward;

            float theta = Vector2.Angle(new Vector2(lookDir.x, lookDir.z), new Vector2(0, 1));
            theta *= (lookDir.x > 0 ? 1.0f : -1.0f);
        
            Vector3 localVelocity = Quaternion.AngleAxis(theta, new Vector3(0,1,0)) * localMoveDir;
        
            //print(localVelocity.magnitude + " @ X:" + localVelocity.x + " Z:" + localVelocity.z);
            localVelocity = localVelocity.magnitude < threshold ? Vector3.zero : localVelocity;
            anim.SetFloat("movX",localVelocity.x * 10);
            anim.SetFloat("movY",localVelocity.z * 10 * -1);
            anim.SetFloat("velocity", localVelocity.magnitude * 125);
		    //print(anim.GetFloat("velocity") + " @ X:" + anim.GetFloat("movX") + " Y:" + anim.GetFloat("movY") + " 0: " + theta);
            prevFramePos = agent.transform.position;
        
		    if (shittyFix)
		    	transform.localPosition = Vector3.zero;
            
        }
    }

    public void LateUpdate()
    {
        //override the animations spine
        FirstPersonCamera fpc = transform.parent.GetComponent<FirstPersonCamera>();
        if(fpc != null)
        fpc.lookRotationBone.rotation = fpc.rotclamp;
        else
        Debug.Log("FirstPersonCamera not found!");
    }

    void OnAnimatorIK()
    {
        //lIKTar = gun;
        //rIKTar = gun;
        //
        ////me.eulerAngles = new Vector3(me.eulerAngles.x, me.eulerAngles.y, gun.eulerAngles.z);
        //
        //anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikWeight);
        //anim.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeight);
        //anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, ikWeight);
        //anim.SetIKRotationWeight(AvatarIKGoal.RightHand, ikWeight);
        //
        ////anim.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, ikWeight);
        ////anim.SetIKHintPositionWeight(AvatarIKHint.RightElbow, ikWeight);
        //
        //anim.SetIKPosition(AvatarIKGoal.LeftHand, lIKTar.position);
        //anim.SetIKPosition(AvatarIKGoal.RightHand, rIKTar.position);
        //anim.SetIKRotation(AvatarIKGoal.LeftHand, lIKTar.rotation * Quaternion.Euler(0, 0, 90));
        //anim.SetIKRotation(AvatarIKGoal.RightHand, rIKTar.rotation * Quaternion.Euler(0, 0, -90));
        //
        ////anim.SetIKHintPosition(AvatarIKHint.LeftElbow, lHint.position);
        ////anim.SetIKHintPosition(AvatarIKHint.RightElbow, rHint.position);
    }
}
