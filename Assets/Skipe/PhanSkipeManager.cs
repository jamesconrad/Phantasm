using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PhanSkipeManager : MonoBehaviour {

	const int AudioBufferSize = 8000;

	int currentBufferSize = AudioBufferSize;

	bool micStarted = false;
	System.IntPtr MicPtr;
	StringBuilder audioBuffer = new StringBuilder(AudioBufferSize);
	StringBuilder sendBuffer = new StringBuilder(AudioBufferSize + 10);
	

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		DontDestroyOnLoad(this);
		startSkipe();
	}

	/// <summary>
	/// This function is called when the MonoBehaviour will be destroyed.
	/// </summary>
	void OnDestroy()
	{
		closeSkipe();
	}

	public void startSkipe()
	{
		MicPtr = PhanSkipeAPI.StartupMic(audioBuffer);
		micStarted = true;
	}

	public void closeSkipe()
	{
		PhanSkipeAPI.ShutdownMic(MicPtr);
		micStarted = false;
	}

	/// <summary>
	/// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
	/// </summary>
	void FixedUpdate()
	{
		if (micStarted)
		{
			currentBufferSize = PhanSkipeAPI.GetAudioBuffer(MicPtr, audioBuffer);

			//PhanSkipeAPI.SetAudioBuffer(MicPtr, audioBuffer, currentBufferSize);
			SendAudioBuffer();
		}
	}

	void SendAudioBuffer()
	{
		sendBuffer = new StringBuilder(((int)PhaNetworkingMessager.MessageType.AudioUpdate).ToString() + " " + audioBuffer);
		PhaNetworkingAPI.SendTo(PhaNetworkingAPI.mainSocket, sendBuffer, sendBuffer.Length, PhaNetworkingAPI.targetIP);
	}

	public void ReceiveBuffer(ref StringBuilder buffer)
	{
		buffer.Remove(0, 2);
		PhanSkipeAPI.SetAudioBuffer(MicPtr, buffer, buffer.Length);
	}
}
