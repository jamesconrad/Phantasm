//More or less taken from https://bitbucket.org/Unity-Technologies/cinematic-image-effects
//Mainly for the temporal anti-aliasing.



using UnityEngine;

//[ImageEffectAllowedInSceneView]
//[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class TAA : MonoBehaviour
{
    private Vector2[] jitterVectors;

    public Material TemporalAAMaterial;
    private RenderTexture PreviousFrame;

    private Matrix4x4 previousProjectionMat;
    private Matrix4x4 viewProjectionMat;
    private Matrix4x4 projectionMat;

    private Camera camera_;

    public enum OffsetType
    {
        Radial,
        Halton,
        UnityRand
    }


    [System.Serializable]
    public struct OffsetSettings
    {
        [Tooltip("The form of offset for use")]
        public OffsetType offsetType;

        [Tooltip("Size of the offset in Texels. Smaller is crisper and less stable, larger is blurrier, but more stable.")]
        [Range(0.1f, 3.0f)]
        public float offsetSize;

        [Tooltip("Number of different offsets. Does not effect UnityRand.")]
        [Range(4, 30)]
        public int numberOfSamples;
    }

    [System.Serializable]
    public struct BlendSettings
    {

        [Range(0.0f, 1.0f)]
        public float stationaryBlend;

        [Range(0.0f, 1.0f)]
        public float movingBlend;

        [Range(30.0f, 100.0f)]
        public float motionAmplification;
    }

    [System.Serializable]
    public class Settings
    {
        [System.AttributeUsage(System.AttributeTargets.Field)]
        public class LayoutAttribute : PropertyAttribute
        {
        }

        [Layout]
        public OffsetSettings offsetSettings;

        [Layout]
        public BlendSettings blendSettings;

        private static readonly Settings defaultSet = new Settings
        {
            offsetSettings = new OffsetSettings
            {
                offsetType = OffsetType.Halton,
                offsetSize = 0.5f,
                numberOfSamples = 8
            },

            blendSettings = new BlendSettings
            {
                stationaryBlend = 0.87f,
                movingBlend = 0.3f,
                motionAmplification = 60.0f
            }
        };
        public static Settings defaultSettings
        {
            get
            {
                return defaultSet;
            }
        }
    }

    [SerializeField]
    public Settings settings = Settings.defaultSettings;

    
    // Use this for initialization
    void Start()
    {
        camera_ = GetComponent<Camera>();
        camera_.depthTextureMode = DepthTextureMode.Depth | DepthTextureMode.MotionVectors;
        jitterVectors = CreateJitterOffsetArray(settings.offsetSettings.numberOfSamples, settings.offsetSettings.offsetSize);
        
        previousProjectionMat = Matrix4x4.identity;
        viewProjectionMat = Matrix4x4.identity;
        TemporalAAMaterial.SetMatrix("_PreviousProjMat", previousProjectionMat);
        TemporalAAMaterial.SetMatrix("_VPMatrix", Matrix4x4.identity);
        TemporalAAMaterial.SetVector("pixSize", new Vector4(1.0f / Screen.width, 1.0f / Screen.height, 0.0f, 0.0f));
    }

    // This function is called when the object becomes enabled and active
    public void OnEnable()
    {
        jitterVectors = CreateJitterOffsetArray(settings.offsetSettings.numberOfSamples, settings.offsetSettings.offsetSize);
    }

    // OnPreCull is called before a camera culls the scene
    private int currentOffset = 0;
    private float currentSize = Settings.defaultSettings.offsetSettings.offsetSize;
    private int currentArraySize = Settings.defaultSettings.offsetSettings.numberOfSamples;
    public void OnPreCull()
    {
        projectionMat = camera_.projectionMatrix;
        camera_.nonJitteredProjectionMatrix = camera_.projectionMatrix;
        switch (settings.offsetSettings.offsetType)
        {
            case OffsetType.Radial:
                if (currentArraySize != settings.offsetSettings.numberOfSamples || currentSize != settings.offsetSettings.offsetSize)
                {
                    jitterVectors = CreateJitterOffsetArray(settings.offsetSettings.numberOfSamples, settings.offsetSettings.offsetSize);
                    currentOffset = 0;
                }
                camera_.projectionMatrix = GetPerspectiveProjectionMatrix(jitterVectors[currentOffset]);
                currentOffset++;
                if (currentOffset >= settings.offsetSettings.numberOfSamples - 1)
                {
                    currentOffset = 0;
                }
                break;

            case OffsetType.Halton:
                Vector2 jitterOffset = GenerateRandomOffset();
                camera_.projectionMatrix = GetPerspectiveProjectionMatrix(jitterOffset);
                break;
                
            case OffsetType.UnityRand:
                camera_.projectionMatrix = GetPerspectiveProjectionMatrix(Random.insideUnitCircle * settings.offsetSettings.offsetSize);
                break;

            default: //Just in case. If this get hit though, something seriously fucked happened.
                Vector2 jitterOff = GenerateRandomOffset();
                camera_.projectionMatrix = GetPerspectiveProjectionMatrix(jitterOff);
                break;
        }
    }

    // OnRenderImage is called after all rendering is complete to render image
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (PreviousFrame == null || (PreviousFrame.width != source.width || PreviousFrame.height != source.height))
        {
            if (PreviousFrame)
                RenderTexture.ReleaseTemporary(PreviousFrame);

            PreviousFrame = RenderTexture.GetTemporary(source.width, source.height, 0, source.format, RenderTextureReadWrite.Default);
            PreviousFrame.filterMode = FilterMode.Bilinear;
            PreviousFrame.hideFlags = HideFlags.HideAndDontSave;
            Graphics.Blit(source, PreviousFrame);

            TemporalAAMaterial.SetVector("pixSize", new Vector4(1.0f / Screen.width, 1.0f / Screen.height, 0.0f, 0.0f));
        }

        viewProjectionMat = GL.GetGPUProjectionMatrix(projectionMat, true) * camera_.worldToCameraMatrix;

        //Set material properties and render.
        TemporalAAMaterial.SetTexture("_MainTex", source);
        TemporalAAMaterial.SetTexture("_PreviousTex", PreviousFrame);
        TemporalAAMaterial.SetMatrix("_PreviousProjMat", previousProjectionMat * Matrix4x4.Inverse(viewProjectionMat));
        TemporalAAMaterial.SetVector("blendWeights", new Vector4(settings.blendSettings.stationaryBlend, settings.blendSettings.movingBlend, 100.0f * settings.blendSettings.motionAmplification, 0.0f));


        RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, source.format, RenderTextureReadWrite.Default);
        temporary.filterMode = FilterMode.Bilinear;

        var effectDestination = destination;
        var doesNeedExtraBlit = false;

        if (destination == null)
        {
            effectDestination = RenderTexture.GetTemporary(source.width, source.height, 0, source.format, RenderTextureReadWrite.Default);
            effectDestination.filterMode = FilterMode.Bilinear;

            doesNeedExtraBlit = true;
        }

        var renderTargets = new RenderBuffer[2];
        renderTargets[0] = effectDestination.colorBuffer;
        renderTargets[1] = temporary.colorBuffer;

        Graphics.SetRenderTarget(renderTargets, effectDestination.depthBuffer);
        RenderFullScreenQuad();

        RenderTexture.ReleaseTemporary(PreviousFrame);
        PreviousFrame = temporary;

        if (doesNeedExtraBlit)
        {
            Graphics.Blit(effectDestination, destination);
            RenderTexture.ReleaseTemporary(effectDestination);
        }

        RenderTexture.active = destination;

    }

    // OnPostRender is called after a camera finished rendering the scene
    public void OnPostRender()
    {
        previousProjectionMat = viewProjectionMat;
        camera_.ResetProjectionMatrix();
    }



    // Adapted heavily from PlayDead's TAA code
    // https://github.com/playdeadgames/temporal/blob/master/Assets/Scripts/Extensions.cs
    private Matrix4x4 GetPerspectiveProjectionMatrix(Vector2 offset)
    {
        float vertical = Mathf.Tan(0.5f * Mathf.Deg2Rad * camera_.fieldOfView);
        float horizontal = vertical * camera_.aspect;

        offset.x *= horizontal / (0.5f * camera_.pixelWidth);
        offset.y *= vertical / (0.5f * camera_.pixelHeight);

        float left = (offset.x - horizontal) * camera_.nearClipPlane;
        float right = (offset.x + horizontal) * camera_.nearClipPlane;
        float top = (offset.y + vertical) * camera_.nearClipPlane;
        float bottom = (offset.y - vertical) * camera_.nearClipPlane;

        Matrix4x4 matrix = new Matrix4x4();

        matrix[0, 0] = (2.0f * camera_.nearClipPlane) / (right - left);
        matrix[0, 1] = 0.0f;
        matrix[0, 2] = (right + left) / (right - left);
        matrix[0, 3] = 0.0f;

        matrix[1, 0] = 0.0f;
        matrix[1, 1] = (2.0f * camera_.nearClipPlane) / (top - bottom);
        matrix[1, 2] = (top + bottom) / (top - bottom);
        matrix[1, 3] = 0.0f;

        matrix[2, 0] = 0.0f;
        matrix[2, 1] = 0.0f;
        matrix[2, 2] = -(camera_.farClipPlane + camera_.nearClipPlane) / (camera_.farClipPlane - camera_.nearClipPlane);
        matrix[2, 3] = -(2.0f * camera_.farClipPlane * camera_.nearClipPlane) / (camera_.farClipPlane - camera_.nearClipPlane);

        matrix[3, 0] = 0.0f;
        matrix[3, 1] = 0.0f;
        matrix[3, 2] = -1.0f;
        matrix[3, 3] = 0.0f;

        return matrix;
    }

    //I really fucking hate this. I'll try and find a better way to do this. Immediate mode has to die.
    private void RenderFullScreenQuad()
    {
        GL.PushMatrix();
        GL.LoadOrtho();
        TemporalAAMaterial.SetPass(0);

        //Render the full screen quad manually.
        GL.Begin(GL.QUADS);
        GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(0.0f, 0.0f, 0.1f);
        GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, 0.0f, 0.1f);
        GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 0.1f);
        GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(0.0f, 1.0f, 0.1f);
        GL.End();

        GL.PopMatrix();
    }

    public Vector2[] CreateJitterOffsetArray(int numberOfOffsets, float offsetScale)
    {
        Vector2[] offsets = new Vector2[numberOfOffsets];
        
        float theta = Mathf.Deg2Rad * (360.0f / numberOfOffsets) + Mathf.Deg2Rad * ((360.0f / numberOfOffsets) / 2);

        for (int i = 0; i < numberOfOffsets; i++)
        {
            float currentTheta = theta * i;
            Vector2 currentOffset;
            currentOffset.x = offsetScale * Mathf.Sin(currentTheta);
            currentOffset.y = offsetScale * Mathf.Cos(currentTheta);
            offsets[i] = currentOffset;
        }

        return offsets;
    }

    //I also don't particularly enjoy these either. The radial version is, I bet, a bit faster.
    private int m_SampleIndex = 0;
    private float GetHaltonValue(int index, int radix)
    {
        float result = 0.0f;
        float fraction = 1.0f / (float)radix;

        while (index > 0)
        {
            result += (float)(index % radix) * fraction;

            index /= radix;
            fraction /= (float)radix;
        }

        return result;
    }
    private Vector2 GenerateRandomOffset()
    {
        Vector2 offset = new Vector2(
                GetHaltonValue(m_SampleIndex & 1023, 2),
                GetHaltonValue(m_SampleIndex & 1023, 3));

        if (++m_SampleIndex >= settings.offsetSettings.numberOfSamples)
            m_SampleIndex = 0;
        offset *= settings.offsetSettings.offsetSize;
        return offset;
    }

}
