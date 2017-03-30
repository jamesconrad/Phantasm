using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class PhanSkipeAPI : MonoBehaviour {


	[DllImport("PhanSkipe", EntryPoint="Startup", CallingConvention = CallingConvention.Cdecl)]
	public static extern System.IntPtr StartupMic(StringBuilder buffer);

	[DllImport("PhanSkipe", EntryPoint="GetBuffer", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GetAudioBuffer(System.IntPtr Microphone, StringBuilder buffer);

	[DllImport("PhanSkipe", EntryPoint="SetBuffer", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SetAudioBuffer(System.IntPtr Microphone, StringBuilder buffer, int size);
	
	[DllImport("PhanSkipe", EntryPoint="Shutdown", CallingConvention = CallingConvention.Cdecl)]
	public static extern System.IntPtr ShutdownMic(System.IntPtr microphone);	
}
