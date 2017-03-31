using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Net;
using UnityEngine;

public class PhaNetworkingAPI : MonoBehaviour {
	

	public static System.IntPtr mainSocket;
	public static int mainPort = 8889;
	public static int targetPort = 8888;
	public static IPAddress hostAddress;
	public static StringBuilder targetIP = new StringBuilder("127.0.0.1");

	/// <summary>
	///Returns a socket pointer. Should be assigned to the mainSocket variable.>
	/// </summary>
	[DllImport("PhaNetworking", EntryPoint="Initialize", CallingConvention = CallingConvention.Cdecl)]
	public static extern System.IntPtr InitializeNetworking(int port = 8889);

	[DllImport("PhaNetworking", EntryPoint="Send", CallingConvention = CallingConvention.Cdecl)]
	public static extern int SendTo(System.IntPtr givenSocket, StringBuilder buffer, int bufferLength, StringBuilder givenAddress, int givenPort = 8889);
	
	[DllImport("PhaNetworking", EntryPoint="Receive", CallingConvention = CallingConvention.Cdecl)]
	public static extern int ReceiveFrom(System.IntPtr givenSocket, StringBuilder receiveBuffer, int bufferLength);
	
	[DllImport("PhaNetworking", EntryPoint="GetRemoteAddress", CallingConvention = CallingConvention.Cdecl)]
	public static extern void GetRemoteAddress(System.IntPtr givenSocket, StringBuilder buffer, int bufferLength);
	
	[DllImport("PhaNetworking", EntryPoint="CloseDown", CallingConvention = CallingConvention.Cdecl)]
	public static extern void ShutDownNetwork(System.IntPtr givenSocket);

}
