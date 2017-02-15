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
		LoadLevel,
		PlayerUpdate,
		EnemyUpdate,
		HealthUpdate
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

	protected int SendPlayerUpdate(Vector3 position, Quaternion orientation, StringBuilder givenAddress)
	{
		StringBuilder sendBuffer = new StringBuilder(
		MessageType.PlayerUpdate.ToString() + " " + 
		position.x + " " + position.y + " " + position.z + " " +
		orientation.w + " " + orientation.x + " " + orientation.y + " " + orientation.z,
		 recvBufferSize);
		return PhaNetworkingAPI.SendTo(PhaNetworkingAPI.mainSocket, sendBuffer, recvBufferSize, givenAddress);
	}

	protected void ReceivePlayerUpdate(Transform playerTransform)
	{
		PhaNetworkingAPI.ReceiveFrom(PhaNetworkingAPI.mainSocket, receiveBuffer, recvBufferSize);
		if (receiveBuffer.ToString().StartsWith(MessageType.PlayerUpdate.ToString()))
		{
			string[] message = receiveBuffer.ToString().Split(' ');
			Vector3 position;
			Quaternion orientation;

			position.x = float.Parse(message[1]);
			position.y = float.Parse(message[2]);
			position.z = float.Parse(message[3]);

			orientation.w = float.Parse(message[4]);
			orientation.x = float.Parse(message[5]);
			orientation.y = float.Parse(message[6]);
			orientation.z = float.Parse(message[7]);

			playerTransform.position = position;
			playerTransform.rotation = orientation;
		}
	}


//Have multiple sockets instead of this. This could cause a backlog of issues, but it's fine for now.
	protected int SendEnemyUpdate(Vector3 position, Quaternion orientation, StringBuilder givenAddress)
	{
		StringBuilder sendBuffer = new StringBuilder(
		MessageType.EnemyUpdate.ToString() + " " + 
		position.x + " " + position.y + " " + position.z + " " +
		orientation.w + " " + orientation.x + " " + orientation.y + " " + orientation.z,
		 recvBufferSize);
		return PhaNetworkingAPI.SendTo(PhaNetworkingAPI.mainSocket, sendBuffer, recvBufferSize, givenAddress);
	}

	protected void ReceiveEnemyUpdate(Transform playerTransform)
	{
		PhaNetworkingAPI.ReceiveFrom(PhaNetworkingAPI.mainSocket, receiveBuffer, recvBufferSize);
		if (receiveBuffer.ToString().StartsWith(MessageType.EnemyUpdate.ToString()))
		{
			string[] message = receiveBuffer.ToString().Split(' ');
			Vector3 position;
			Quaternion orientation;

			position.x = float.Parse(message[1]);
			position.y = float.Parse(message[2]);
			position.z = float.Parse(message[3]);

			orientation.w = float.Parse(message[4]);
			orientation.x = float.Parse(message[5]);
			orientation.y = float.Parse(message[6]);
			orientation.z = float.Parse(message[7]);

			playerTransform.position = position;
			playerTransform.rotation = orientation;
		}
	}

	protected int SendHealthUpdate(int damageTaken, StringBuilder givenAddress)
	{
		StringBuilder sendBuffer = new StringBuilder(MessageType.HealthUpdate.ToString() + " " + damageTaken.ToString());
		return PhaNetworkingAPI.SendTo(PhaNetworkingAPI.mainSocket, sendBuffer, sendBuffer.Capacity, givenAddress);
	}

	protected int ReceiveHealthUpdate()
	{
		PhaNetworkingAPI.ReceiveFrom(PhaNetworkingAPI.mainSocket, receiveBuffer, recvBufferSize);

		if (receiveBuffer.ToString().StartsWith(MessageType.HealthUpdate.ToString()))
		{
			return int.Parse(receiveBuffer.ToString().Split()[1]);
		}
		return 0;
	}

	protected int ReceiveInGameMessage()
	{
		PhaNetworkingAPI.ReceiveFrom(PhaNetworkingAPI.mainSocket, receiveBuffer, recvBufferSize);
		if (receiveBuffer.Length > 0)
		{
			return int.Parse(receiveBuffer.ToString().Split(' ')[0]);
		}
		return -1;
	}
}
