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

    const float timeToWaitTillSearch = 10.0f;
    float timeSinceLastSearch = timeToWaitTillSearch;

    void Update () 
	{

        timeSinceLastSearch += Time.deltaTime;
        if (timeSinceLastSearch > timeToWaitTillSearch)
        {
            GOPhantom = GameObject.FindGameObjectsWithTag("Enemy");
            timeSinceLastSearch = 0.0f;
        }

        
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
					Renderer[] fuck = GOPhantom[i].GetComponentsInChildren<Renderer>();
					for(int k = 0; k < fuck.Length; ++k)
					{
						fuck[i].material = phantomMaterials.normal;
					}
				}
				else
				{
					Renderer[] fuck = GOPhantom[i].GetComponentsInChildren<Renderer>();
					for(int k = 0; k < fuck.Length; ++k)
					{
						fuck[i].material = phantomMaterials.camera;
					}
				}
			}
			else
			{		
				for(int k = 0; k < GOPhantom[i].transform.childCount; ++k )
				{
					GOPhantom[i].transform.GetChild(k).gameObject.layer = 9;
					GOPhantom[i].layer = 9;
				}
			}
		}
	}
}
