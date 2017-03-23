using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using UnityEngine;

public class PhaNetworkingMessager : MonoBehaviour {

	public const int recvBufferSize = 256;
	public StringBuilder receiveBuffer = new StringBuilder("Nothing", recvBufferSize);

	public enum MessageType
	{
		Connection = 0,
		CharacterLock,
		LoadLevel,
		PlayerUpdate,
		EnemyUpdate,
		HealthUpdate,
		ConsoleMessage,
		DoorUpdate
	}

	//Tell the other player that you are online
	public int SendConnectionMessage(StringBuilder givenAddress)
	{
		StringBuilder sendBuffer = new StringBuilder(((int)MessageType.Connection).ToString(), recvBufferSize);
		return PhaNetworkingAPI.SendTo(PhaNetworkingAPI.mainSocket, sendBuffer, sendBuffer.Length, givenAddress);
	}
	//Receive information that the other player is online.
	public int ReceiveConnectionMessage()
	{
		PhaNetworkingAPI.ReceiveFrom(PhaNetworkingAPI.mainSocket, receiveBuffer, recvBufferSize);
		if (receiveBuffer.ToString().StartsWith(((int)MessageType.Connection).ToString()))
		{
			return 1;
		}
		return 0;
	}	


	//Send a message informing the other player what character you have chosen.
	public int SendCharacterLockMessage(int choice, StringBuilder givenAddress)
	{
		StringBuilder sendBuffer = new StringBuilder(((int)MessageType.CharacterLock).ToString() + " " + choice.ToString(), recvBufferSize);
		Debug.Log("Send buffer contents: " + sendBuffer.ToString());
		return PhaNetworkingAPI.SendTo(PhaNetworkingAPI.mainSocket, sendBuffer, recvBufferSize, givenAddress);
	}
	//Receive a message that tells you what character the other player has chosen.
	public int ReceiveCharacterLockMessage()
	{
		StringBuilder characterLockMessage = new StringBuilder(recvBufferSize);
		Debug.Log("bytes received: " + PhaNetworkingAPI.ReceiveFrom(PhaNetworkingAPI.mainSocket, characterLockMessage, recvBufferSize));
		Debug.Log("receive from receive character lock " +  characterLockMessage.ToString());
		if (characterLockMessage.ToString().StartsWith(((int)MessageType.CharacterLock).ToString()))
		{
			string[] message = characterLockMessage.ToString().Split(' ');
			Debug.Log(message);
			if (message.Length > 0)
			{
				int result;
				result = int.Parse(message[1]);
				return result;
			}
		}
		return -1;
	}

	//Send a message to inform the other player to begin loading the level.
	public int SendLoadLevelMessage(StringBuilder givenAddress)
	{
		StringBuilder sendBuffer = new StringBuilder(((int)MessageType.LoadLevel).ToString(), recvBufferSize);
		return PhaNetworkingAPI.SendTo(PhaNetworkingAPI.mainSocket, sendBuffer, recvBufferSize, givenAddress);
	}
	public int ReceiveLoadLevelMessage()
	{
		StringBuilder LevelReceiveBuffer = new StringBuilder(recvBufferSize);
		PhaNetworkingAPI.ReceiveFrom(PhaNetworkingAPI.mainSocket, LevelReceiveBuffer, recvBufferSize);
		if (LevelReceiveBuffer.ToString().StartsWith(((int)MessageType.LoadLevel).ToString()))
		{
			return 1;
		}
		return 0;
	}

	public int SendPlayerUpdate(Vector3 position, Quaternion orientation, StringBuilder givenAddress)
	{
		StringBuilder sendBuffer = new StringBuilder(
		((int)MessageType.PlayerUpdate).ToString() + " " + 
		position.x + " " + position.y + " " + position.z + " " +
		orientation.w + " " + orientation.x + " " + orientation.y + " " + orientation.z,
		 recvBufferSize);
		return PhaNetworkingAPI.SendTo(PhaNetworkingAPI.mainSocket, sendBuffer, sendBuffer.Length, givenAddress);
	}

	public void ReceivePlayerUpdate(Transform playerTransform)
	{
		StringBuilder PlayerReceiveBuffer = new StringBuilder(recvBufferSize);
		PhaNetworkingAPI.ReceiveFrom(PhaNetworkingAPI.mainSocket, PlayerReceiveBuffer, recvBufferSize);
		if (PlayerReceiveBuffer.ToString().StartsWith(((int)MessageType.PlayerUpdate).ToString()))
		{
			string[] message = PlayerReceiveBuffer.ToString().Split(' ');
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


	public int SendEnemyUpdate(Vector3 position, Quaternion orientation, int index, StringBuilder givenAddress)
	{
		StringBuilder sendBuffer = new StringBuilder(
		((int)MessageType.EnemyUpdate).ToString() + " " + 
		index.ToString() + " " +
		position.x + " " + position.y + " " + position.z + " " +
		orientation.w + " " + orientation.x + " " + orientation.y + " " + orientation.z,
		recvBufferSize);
		return PhaNetworkingAPI.SendTo(PhaNetworkingAPI.mainSocket, sendBuffer, sendBuffer.Length, givenAddress);
	}

	public void ReceiveEnemyUpdate(GameObject playerTransform, ref StringBuilder EnemyReceiveBuffer)
	{
		if (EnemyReceiveBuffer.ToString().StartsWith(((int)MessageType.EnemyUpdate).ToString()))
		{
			string[] message = EnemyReceiveBuffer.ToString().Split(' ');
			Vector3 position;
			Quaternion orientation;

			int id = int.Parse(message[1]);

			position.x = float.Parse(message[2]);
			position.y = float.Parse(message[3]);
			position.z = float.Parse(message[4]);

			orientation.w = float.Parse(message[5]);
			orientation.x = float.Parse(message[6]);
			orientation.y = float.Parse(message[7]);
			orientation.z = float.Parse(message[8]);

			playerTransform.transform.position = position;
			playerTransform.transform.rotation = orientation;
		}
	}

	public void ParseObjectUpdate(StringBuilder buffer, Transform objectTransform)
	{
		string[] message = buffer.ToString().Split(' ');
		Vector3 position;
		Quaternion orientation;

		position.x = float.Parse(message[1]);
		position.y = float.Parse(message[2]);
		position.z = float.Parse(message[3]);

		orientation.w = float.Parse(message[4]);
		orientation.x = float.Parse(message[5]);
		orientation.y = float.Parse(message[6]);
		orientation.z = float.Parse(message[7]);

		objectTransform.position = position;
		objectTransform.rotation = orientation;

		return;
	}

	public int SendHealthUpdate(int damageTaken, StringBuilder givenAddress)
	{
		StringBuilder sendBuffer = new StringBuilder(((int)MessageType.HealthUpdate).ToString() + " " + damageTaken.ToString());
		return PhaNetworkingAPI.SendTo(PhaNetworkingAPI.mainSocket, sendBuffer, sendBuffer.Length, givenAddress);
	}

	///returns the value of damage taken.
	public int ReceiveHealthUpdate()
	{
		PhaNetworkingAPI.ReceiveFrom(PhaNetworkingAPI.mainSocket, receiveBuffer, recvBufferSize);

		if (receiveBuffer.ToString().StartsWith(((int)MessageType.HealthUpdate).ToString()))
		{
			return int.Parse(receiveBuffer.ToString().Split()[1]);
		}
		return 0;
	}
	public int ParseHealthUpdate(StringBuilder buffer)
	{
		string[] message = buffer.ToString().Split(' ');

		return int.Parse(message[1]);
	}

	public int SendDoorUpdate(int id, Quaternion rotation)
	{
		StringBuilder sendBuffer = new StringBuilder(((int)MessageType.DoorUpdate).ToString() + " " + 
		rotation.x.ToString() + " " + rotation.y.ToString() + " " + rotation.z.ToString() + " " + rotation.w.ToString());
		return PhaNetworkingAPI.SendTo(PhaNetworkingAPI.mainSocket, sendBuffer, sendBuffer.Length, PhaNetworkingAPI.targetIP);
	}

	public int ReceiveInGameMessage() 
	{
		receiveBuffer = new StringBuilder(recvBufferSize);
		PhaNetworkingAPI.ReceiveFrom(PhaNetworkingAPI.mainSocket, receiveBuffer, recvBufferSize);
		if (receiveBuffer.Length > 0)
		{
			return int.Parse(receiveBuffer.ToString().Split(' ')[0]);
		}
		return -1;
	}
}
