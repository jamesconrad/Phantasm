using UnityEngine;
using System.Collections;


namespace Plasma
{ 
	public class ScreenShot
	{
	    public static void Check()
	    {		        	
	        if (Input.GetKeyDown(KeyCode.F12))
	        {
	            int Magnify;

                if (Input.GetKey(KeyCode.RightControl))
	            {
	                Magnify = 4;
	            }
	            else if (Input.GetKey(KeyCode.RightAlt))
	            {
	                Magnify = 2;
	            }
			    else
	            {
	                Magnify = 1;				
	            }

	            Debug.Log("Taking Screenshot (x" + Magnify + ")");
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
