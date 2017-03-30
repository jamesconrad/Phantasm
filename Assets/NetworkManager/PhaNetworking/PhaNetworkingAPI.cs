using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Net;
using UnityEngine;

public class PhaNetworkingAPI : MonoBehaviour {
	

	public static System.IntPtr mainSocket;
	const int mainPort = 8889;
	public static IPAddress hostAddress = new IPAddress(000);
	public static StringBuilder targetIP = new StringBuilder("127.0.0.1");

	/// <summary>
	///Returns a socket pointer. Should be assigned to the mainSocket variable.>
	/// </summary>
	[DllImport("PhaNetworking", EntryPoint="Initialize", CallingConvention = CallingConvention.Cdecl)]
	public static extern System.IntPtr InitializeNetworking(int port = mainPort);

	[DllImport("PhaNetworking", EntryPoint="Send", CallingConvention = CallingConvention.Cdecl)]
	public static extern int SendTo(System.IntPtr givenSocket, StringBuilder buffer, int bufferLength, StringBuilder givenAddress, int givenPort = mainPort);
	
	[DllImport("PhaNetworking", EntryPoint="Receive", CallingConvention = CallingConvention.Cdecl)]
	public static extern int ReceiveFrom(System.IntPtr givenSocket, StringBuilder receiveBuffer, int bufferLength);
	
	[DllImport("PhaNetworking", EntryPoint="GetRemoteAddress", CallingConvention = CallingConvention.Cdecl)]
	public static extern void GetRemoteAddress(System.IntPtr givenSocket, StringBuilder buffer, int bufferLength);
	
	[DllImport("PhaNetworking", EntryPoint="CloseDown", CallingConvention = CallingConvention.Cdecl)]
	public static extern void ShutDownNetwork(System.IntPtr givenSocket);

}
