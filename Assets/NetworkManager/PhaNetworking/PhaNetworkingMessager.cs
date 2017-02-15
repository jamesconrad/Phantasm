using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using UnityEngine;

public class PhaNetworkingMessager : MonoBehaviour {

	protected const int recvBufferSize = 256;
	protected StringBuilder receiveBuffer = new StringBuilder("Nothing", recvBufferSize);

	protected enum MessageType
	{
		Connection = 0,
		CharacterLock,
		LoadLevel
	}

	//Tell the other player that you are online
	protected int SendConnectionMessage(StringBuilder givenAddress)
	{
		StringBuilder sendBuffer = new StringBuilder(MessageType.Connection.ToString(), recvBufferSize);
		return PhaNetworkingAPI.SendTo(PhaNetworkingAPI.mainSocket, sendBuffer, recvBufferSize, givenAddress);
	}
	//Receive information that the other player is online.
	protected int ReceiveConnectionMessage()
	{
		PhaNetworkingAPI.ReceiveFrom(PhaNetworkingAPI.mainSocket, receiveBuffer, recvBufferSize);
		if (receiveBuffer.ToString().StartsWith(MessageType.Connection.ToString()))
		{
			return 1;
		}
		return 0;
	}	


	//Send a message informing the other player what character you have chosen.
	protected int SendCharacterLockMessage(int choice, StringBuilder givenAddress)
	{
		StringBuilder sendBuffer = new StringBuilder(MessageType.CharacterLock.ToString() + " " + choice.ToString(), recvBufferSize);
		return PhaNetworkingAPI.SendTo(PhaNetworkingAPI.mainSocket, sendBuffer, recvBufferSize, givenAddress);
	}
	//Receive a message that tells you what character the other player has chosen.
	protected int ReceiveCharacterLockMessage()
	{
		PhaNetworkingAPI.ReceiveFrom(PhaNetworkingAPI.mainSocket, receiveBuffer, recvBufferSize);
		if (receiveBuffer.ToString().StartsWith(MessageType.CharacterLock.ToString()))
		{
			string[] message = receiveBuffer.ToString().Split(' ');
			if (message.Length > 0)
			{
				int result;
				int.TryParse(message[1], out result);
				return result;
			}
		}
		return 0;
	}

	//Send a message to inform the other player to begin loading the level.
	protected int SendLoadLevelMessage(StringBuilder givenAddress)
	{
		StringBuilder sendBuffer = new StringBuilder(MessageType.LoadLevel.ToString(), recvBufferSize);
		return PhaNetworkingAPI.SendTo(PhaNetworkingAPI.mainSocket, sendBuffer, recvBufferSize, givenAddress);
	}
	protected int ReceiveLoadLevelMessage()
	{
		PhaNetworkingAPI.ReceiveFrom(PhaNetworkingAPI.mainSocket, receiveBuffer, recvBufferSize);
		if (receiveBuffer.ToString().StartsWith(MessageType.LoadLevel.ToString()))
		{
			return 1;
		}
		return 0;
	}
}
