using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PhantomAnimParam : MonoBehaviour {

        public Animator anim;
        Vector3 last;
        Vector3 d;

        // Use this for initialization
        void Start () {
                anim = GetComponentInChildren<Animator>();
                last = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }

        // Update is called once per frame
        void Update () {
                d = last - transform.position;
                anim.SetFloat("mov", d.magnitude);

                last.x = transform.position.x;
                last.y = transform.position.y;
                last.z = transform.position.z;
        }
}
