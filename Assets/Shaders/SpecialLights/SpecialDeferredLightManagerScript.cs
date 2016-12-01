using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;

public class SpecialLightSystem
{
	// Singleton
	static SpecialLightSystem _Instance;
	static public SpecialLightSystem instance
	{
		get
		{
			if (_Instance == null)
				_Instance = new SpecialLightSystem();
			return _Instance;
		}
	}

	// List of lights
	internal HashSet<SpecialLight> _Lights = new HashSet<SpecialLight>();

	public void Add(SpecialLight o)
	{
		// Make sure the light doesn't already exist
		Remove(o);
		_Lights.Add(o);
	}
	public void Remove(SpecialLight o)
	{
		_Lights.Remove(o);
	}
}

[ExecuteInEditMode]
public class SpecialDeferredLightManagerScript : MonoBehaviour
{
	public Shader _LightShader;
	private Material _LightMaterial;

	public Mesh _CubeMesh;
	public Mesh _SphereMesh;



	private struct CommandBufferEntry
	{
		public CommandBuffer _AfterLighting;
		public CommandBuffer _BeforeAlpha;
	}

	// Gotta have dat list of cameras
	private Dictionary<Camera, CommandBufferEntry> _Cameras = new Dictionary<Camera, CommandBufferEntry>();


	public void OnDisable()
	{
		// Loop through all dem cameras
		foreach (var cam in _Cameras)
		{
			if (cam.Key)
			{
				cam.Key.RemoveCommandBuffer(CameraEvent.AfterLighting, cam.Value._AfterLighting);
				cam.Key.RemoveCommandBuffer(CameraEvent.BeforeForwardAlpha, cam.Value._BeforeAlpha);
			}
		}
		Object.DestroyImmediate(_LightMaterial);
		// Bye Bye
	}

	public void Update()
	{
		DrawLights();
	}

	public void OnWillRenderObject()
	{
		DrawLights();
	}

	public void DrawLights()
	{
		//print("Please work");

		var act = gameObject.activeInHierarchy && enabled;
		if (!act)
		{
			OnDisable();
			return;
		}

		var cam = Camera.current;
		if (!cam)
			return;

		// Create material to render lights
		if (!_LightMaterial)
		{
			_LightMaterial = new Material(_LightShader);
			_LightMaterial.hideFlags = HideFlags.HideAndDontSave;
		}

		CommandBufferEntry buf = new CommandBufferEntry();
		if (_Cameras.ContainsKey(cam))
		{
			// use existing command buffers: clear them
			buf = _Cameras[cam];
			buf._AfterLighting.Clear();
			buf._BeforeAlpha.Clear();
		}
		else
		{
			// create new command buffers
			buf._AfterLighting = new CommandBuffer();
			buf._AfterLighting.name = "Deferred custom lights";
			buf._BeforeAlpha = new CommandBuffer();
			buf._BeforeAlpha.name = "Draw light shapes";
			_Cameras[cam] = buf;

			cam.AddCommandBuffer(CameraEvent.AfterLighting, buf._AfterLighting);
			cam.AddCommandBuffer(CameraEvent.BeforeForwardAlpha, buf._BeforeAlpha);
		}

		//@TODO: in a real system should cull lights, and possibly only
		// recreate the command buffer when something has changed.

		var system = SpecialLightSystem.instance;

		var propParams = Shader.PropertyToID("_SpecialLightParams");
		var propColor = Shader.PropertyToID("_SpecialLightColor");
		Vector4 param = Vector4.zero;
		Matrix4x4 trs = Matrix4x4.identity;

		// construct command buffer to draw lights and compute illumination on the scene
		foreach (var o in system._Lights)
		{
			// light parameters we'll use in the shader
			param.x = o._Length;
			param.y = o._Radius;
			param.z = 1.0f / (o._Range * o._Range);
			param.w = (float)o._LightType;
			buf._AfterLighting.SetGlobalVector(propParams, param);
			// light color
			buf._AfterLighting.SetGlobalColor(propColor, o.GetLinearColor());

			// Draw sphere that covers light area, with shader
			// pass that computes illumination on the scene
			trs = Matrix4x4.TRS(o.transform.position, o.transform.rotation, new Vector3(o._Range * 2.0f, o._Range * 2.0f, o._Range * 2.0f));
			buf._AfterLighting.DrawMesh(_SphereMesh, trs, _LightMaterial, 0, 0);
		}

		// construct buffer to draw light shapes themselves as simple objects in the scene
		foreach (var o in system._Lights)
		{
			if (o._Draw)
			{
				// Get flat light color for alpha
				buf._BeforeAlpha.SetGlobalColor(propColor, o.GetLinearColor());

				// Draw the light itself as a small sphere/box (I might make the cylinder actually draw a tube later on)

				// TRS = Translation, Rotation, Scaling
				if (o._LightType == SpecialLight.LightType.Cylinder)
				{
					Vector3 rotationEuler = o.transform.rotation.eulerAngles;
					trs = Matrix4x4.TRS(o.transform.position, Quaternion.Euler(rotationEuler.x, rotationEuler.y, rotationEuler.z + 90.0f), new Vector3(o._Radius * 2.0f, o._Length * 1.0f, o._Radius * 2.0f));
					buf._BeforeAlpha.DrawMesh(_CubeMesh, trs, _LightMaterial, 0, 1);
				}
				else if (o._LightType == SpecialLight.LightType.Sphere)
				{
					trs = Matrix4x4.TRS(o.transform.position, o.transform.rotation, new Vector3(o._Radius * 2.0f, o._Radius * 2.0f, o._Radius * 2.0f));
					buf._BeforeAlpha.DrawMesh(_SphereMesh, trs, _LightMaterial, 0, 1);
				}
			}
		}
	}
}
