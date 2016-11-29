#if UNITY_EDITOR && !UNITY_5

using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System;


public class AkWwiseMenu_Linux : MonoBehaviour {

	private static AkUnityPluginInstaller_Linux m_installer = new AkUnityPluginInstaller_Linux();

	// private static AkUnityIntegrationBuilder_Linux m_rebuilder = new AkUnityIntegrationBuilder_Linux();

	[MenuItem("Assets/Wwise/Install Plugins/Linux/x86/Debug", false, (int)AkWwiseMenuOrder.Linux32Debug)]
	public static void InstallPlugin_x86_Debug () {
		m_installer.InstallPluginByArchConfig("x86", "Debug");
	}

	[MenuItem("Assets/Wwise/Install Plugins/Linux/x86/Profile", false, (int)AkWwiseMenuOrder.Linux32Profile)]
	public static void InstallPlugin_x86_Profile () {
		m_installer.InstallPluginByArchConfig("x86", "Profile");
	}

	[MenuItem("Assets/Wwise/Install Plugins/Linux/x86/Release", false, (int)AkWwiseMenuOrder.Linux32Release)]
	public static void InstallPlugin_x86_Release () {
		m_installer.InstallPluginByArchConfig("x86", "Release");
	}

	[MenuItem("Assets/Wwise/Install Plugins/Linux/x86_64/Debug", false, (int)AkWwiseMenuOrder.Linux64Debug)]
	public static void InstallPlugin_x86_64_Debug () {
		m_installer.InstallPluginByArchConfig("x86_64", "Debug");
	}

	[MenuItem("Assets/Wwise/Install Plugins/Linux/x86_64/Profile", false, (int)AkWwiseMenuOrder.Linux64Profile)]
	public static void InstallPlugin_x86_64_Profile () {
		m_installer.InstallPluginByArchConfig("x86_64", "Profile");
	}

	[MenuItem("Assets/Wwise/Install Plugins/Linux/x86_64/Release", false, (int)AkWwiseMenuOrder.Linux64Release)]
	public static void InstallPlugin_x86_64_Release () {
		m_installer.InstallPluginByArchConfig("x86_64", "Release");
	}
}


public class AkUnityPluginInstaller_Linux : AkUnityPluginInstallerMultiArchBase
{
	public AkUnityPluginInstaller_Linux()
	{
		m_platform = "Linux";
		m_arches = new string[] {"x86", "x86_64"};
	} 

	protected override string GetPluginDestPath(string arch)
	{
		return Path.Combine(m_pluginDir, arch);
	}
	
}


public class AkUnityIntegrationBuilder_Linux : AkUnityIntegrationBuilderBase
{
	public AkUnityIntegrationBuilder_Linux()
	{
		m_platform = "Linux";
	}
}
#endif // #if UNITY_EDITOR