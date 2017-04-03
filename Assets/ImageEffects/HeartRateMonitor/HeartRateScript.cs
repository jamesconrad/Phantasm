using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeartRateScript : MonoBehaviour 
{
	

	RawImage image;
	Material material;

	Health agentHealth;

	
	[System.SerializableAttribute]
	public class HeartRateArray
	{
		public Color color;
		public Vector4 uvMult;
	}

	public HeartRateArray[] healthArray;

	// Use this for initialization
	void Start () 
	{
		agentHealth = FindObjectOfType<NetworkedMovement>().GetComponent<Health>();
		image = GetComponent<RawImage>();
		material = image.material;
	}
	
	// Update is called once per frame
	void Update () 
	{
		int arrayChoice = (int)(healthArray.Length * Mathf.InverseLerp(0.0f, agentHealth.health, agentHealth.currentHealth));
		arrayChoice = Mathf.Clamp(arrayChoice, 0, healthArray.Length - 1);
		print(arrayChoice);
		material.SetColor("_Color", healthArray[arrayChoice].color);
		material.SetVector("_UVMult", healthArray[arrayChoice].uvMult);
	}
}

