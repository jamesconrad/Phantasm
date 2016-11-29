using UnityEngine;
using System.Collections;


namespace Plasma
{ 
	public class ScreenShot
	{
	    public static void Check()
	    {
	        int Magnify;
	
	        if (Input.GetKey(KeyCode.RightControl))
	        {
	            Magnify = 4;
				Debug.Log("Taking High Resolution Screenshot (x4)");
	        }
	        else if (Input.GetKey(KeyCode.RightAlt))
	        {
	            Magnify = 2;
				Debug.Log("Taking High Resolution Screenshot (x2)");
	        }
			else
	        {
	            Magnify = 1;				
				Debug.Log("Taking Screenshot");
	        }
	
	        if (Input.GetKeyDown(KeyCode.F12))
	        {
	            Application.CaptureScreenshot("./Screenshot.png", Magnify);
	            Debug.Log("Screenshot captured");
	        }
	    }

		public static void TakeScreenShot(int i = 1)
		{
			Application.CaptureScreenshot("./Screenshot.png", i);
			Debug.Log("Screenshot captured");
		}
	}
}
