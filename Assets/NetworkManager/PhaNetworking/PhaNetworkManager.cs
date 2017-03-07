using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PhaNetworkManager : PhaNetworkingMessager {

	private static readonly PhaNetworkManager singleton = new PhaNetworkManager();
	public static PhaNetworkManager Singleton { get { return singleton; } }
	public static IPAddress GetLocalHost()
	{
		//Get local IP address. Hope it doesn't change. It could. I should change this to whenever it specically tries to create a game.
		IPAddress[] ipv4Addresses = Dns.GetHostAddresses(Dns.GetHostName());
		for (int i = 0; i < ipv4Addresses.Length; i++)
		{
			if (ipv4Addresses.GetValue(i).ToString() != "127.0.0.1" && (ipv4Addresses.GetValue(i) as IPAddress).AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
			{
				PhaNetworkingAPI.hostAddress = ipv4Addresses.GetValue(i) as IPAddress;
				break;
			}
		}	
		return PhaNetworkingAPI.hostAddress;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
