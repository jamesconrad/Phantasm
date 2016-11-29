using UnityEngine;
using System.Collections;

namespace Plasma
{

    /* 
        Here's the scoop, these determine whether an object shows up to certain camera modes
      
        Visible will draw it with the regular material
        Translucent will draw it with the Distortion shader
        Invisible will not draw it, by putting it in a camera layer based what it is

                            Player  Camera  Thermal Sonar   
        EnemyVisible		X		X		X		X		
        Enemy						X		X		X		
        EnemyThermal						X				
        EnemySonar									X		

    */

    namespace VisibleTo
    {
        public enum Agent
        {
            Visible,
            Translucent,
            Invisible
        }

        public enum Camera
        {
            Visible,
            Translucent,
            Invisible
        }

        public enum Thermal
        {
            Visible,
            Invisible
        }

        public enum Sonar
        {
            Visible,
            Invisible
        }
    }


    [System.Serializable]
    public struct Visibility
    {
        [Tooltip("Sets if the Entity is visible by the Agent")]
        public VisibleTo.Agent agentVisibility;

        [Tooltip("Sets if the Entity is visible by the Cameras in Default & Night Vision modes")]        
        public VisibleTo.Camera cameraVisibility;

		[Tooltip("Sets if the Entity is visible by Thermal imaging")]        
        public VisibleTo.Thermal thermalVisibility;

		[Tooltip("Sets if the Entity is visible by Sonar")]        
        public VisibleTo.Sonar sonarVisibility;

        [Tooltip("Sets how hot the Entity shows up by Thermal imaging")]
        [Range(0.0f, 1.0f)]
        public float temperature;
    }
}
