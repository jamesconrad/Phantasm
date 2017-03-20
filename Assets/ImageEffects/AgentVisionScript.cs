using UnityEngine;
using System.Collections;

public class AgentVisionScript : MonoBehaviour 
{
	private GameObject[] GOPhantom;
	private Phantom EntityPhantom;
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
		
        GOPhantom = GameObject.FindGameObjectsWithTag("Enemy");
		for(int i = 0; i < GOPhantom.Length; ++i)
		{
			EntityPhantom = GOPhantom[i].GetComponent<Phantom>();
			VisiblePhantom = EntityPhantom.visibility;

			//CameraSettings.cullingMask = (1 << 9) | defaultCameraLayersActive;
			if(VisiblePhantom.agent > 0)
			{
				GOPhantom[i].layer = 8;
				if(VisiblePhantom.agent == Plasma.SeenBy.Agent.Translucent)
				{
					GOPhantom[i].GetComponent<Renderer>().material = phantomMaterials.normal;
				}
				else
				{
					GOPhantom[i].GetComponent<Renderer>().material = phantomMaterials.camera;
				}
			}
			else
			{			
				GOPhantom[i].layer = 9;
			}
		}
	}
}
