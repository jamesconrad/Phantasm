using UnityEngine;
using System.Collections;

public class AgentVisionScript : MonoBehaviour 
{
	private GameObject GOPhantom;
	private Phantom	EntityPhantom;
	private Plasma.Visibility	VisiblePhantom;

	[Space(10)]
	public CameraMaterials phantomMaterials;

	
	[System.Serializable]
    public struct CameraMaterials
    {
		public Material normal;
		public Material camera;
		public Material thermal;
		public Material sonar;
	}
	
		
	public LayerMask defaultCameraLayersActive;

	void Start () 
	{
	
	}
	
	void Update () 
	{
		
        GOPhantom = GameObject.Find("Phantom(Clone)");
		EntityPhantom = GOPhantom.GetComponent<Phantom>();
		VisiblePhantom = EntityPhantom.visibility;

		//CameraSettings.cullingMask = (1 << 9) | defaultCameraLayersActive;
		if(VisiblePhantom.agent > 0)
		{
			GOPhantom.layer = 8;
			if(VisiblePhantom.agent == Plasma.SeenBy.Agent.Translucent)
			{
				GOPhantom.GetComponent<Renderer>().material = phantomMaterials.normal;
			}
			else
			{
				GOPhantom.GetComponent<Renderer>().material = phantomMaterials.camera;
			}
		}
		else
		{			
			GOPhantom.layer = 9;
		}
	}
}
