using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class NetworkedBehaviour : MonoBehaviour {
	public virtual void ReceiveBuffer(ref StringBuilder buffer)
	{
	}
}
