using UnityEngine;
using System.Collections;

public class IKHandler : MonoBehaviour {

    Animator anim;
    Transform me;
    Transform gun;

    private Transform lIKTar;
    private Transform rIKTar;
    private float ikWeight = 1;

    //public Transform lHint;
    //public Transform rHint;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        me = GetComponent<Transform>();
        gun = GetComponentInParent<GunHandle>().gunReference.transform;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnAnimatorIK()
    {
        lIKTar = gun;
        rIKTar = gun;
        
        me.eulerAngles = new Vector3(me.eulerAngles.x, me.eulerAngles.y, gun.eulerAngles.z);

        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikWeight);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeight);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, ikWeight);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, ikWeight);

        //anim.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, ikWeight);
        //anim.SetIKHintPositionWeight(AvatarIKHint.RightElbow, ikWeight);

        anim.SetIKPosition(AvatarIKGoal.LeftHand, lIKTar.position);
        anim.SetIKPosition(AvatarIKGoal.RightHand, rIKTar.position);
        anim.SetIKRotation(AvatarIKGoal.LeftHand, lIKTar.rotation * Quaternion.Euler(0, 0, 90));
        anim.SetIKRotation(AvatarIKGoal.RightHand, rIKTar.rotation * Quaternion.Euler(0, 0, -90));

        //anim.SetIKHintPosition(AvatarIKHint.LeftElbow, lHint.position);
        //anim.SetIKHintPosition(AvatarIKHint.RightElbow, rHint.position);
    }
}
