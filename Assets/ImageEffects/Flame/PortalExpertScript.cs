using UnityEngine;
using System.Collections;

public class PortalExpertScript : MonoBehaviour
{
    private Vector4 uRGB;
    private Vector4 uUVMult;
    private Vector4 uLoopAdd;
    private Vector4 uLoopMult;
    private Vector4 uLoopFinalAdd;
    private Vector4 uFinalColAdd;

    private Vector4 uNewRGB;
    private Vector4 uNewUVMult;
    private Vector4 uNewLoopAdd;
    private Vector4 uNewLoopMult;
    private Vector4 uNewLoopFinalAdd;
    private Vector4 uNewFinalColAdd;

    public Material effectMaterial;
    //public Shader effectShader;
    //private

    float lerpInterp = 1.0f;

    public void Start()
    {


        uRGB = new Vector4(0.333f, 0.500f, -0.150f, 0.0f);

        uUVMult =       new Vector4(5.0f,  5.0f, 0.0f, 0.0f);
        uLoopAdd =      new Vector4(0.3f,  0.3f, 0.0f, 1.0f);
        uLoopMult =     new Vector4(1.0f, 10.0f, 0.0f, 1.0f);
        uLoopFinalAdd = new Vector4(4.0f, -4.0f, 0.0f, 1.0f);
        
        uFinalColAdd = new Vector4(0.100f, 0.000f, -0.050f, 0.0f);


        uNewRGB = Random.ColorHSV();

        uNewUVMult =       new Vector4(5.0f,  5.0f, 0.0f, 0.0f);
        uNewLoopAdd =      new Vector4(0.3f,  0.3f, 0.0f, 1.0f);
        uNewLoopMult =     new Vector4(1.0f, 10.0f, 0.0f, 1.0f);
        uNewLoopFinalAdd = new Vector4(4.0f, -4.0f, 0.0f, 1.0f);

        uNewFinalColAdd = Random.ColorHSV();
        
    }

    public void Update()
    {
        float smoothInterp = Mathf.SmoothStep(0.0f, 1.0f, lerpInterp);

        effectMaterial.SetVector("uRGB",            Color.Lerp(     uRGB,           uNewRGB,            smoothInterp));
        effectMaterial.SetVector("uUVMult",         Vector2.Lerp(   uUVMult,        uNewUVMult,         smoothInterp));
        effectMaterial.SetVector("uLoopAdd",        Vector4.Lerp(   uLoopAdd,       uNewLoopAdd,        smoothInterp));
        effectMaterial.SetVector("uLoopMult",       Vector4.Lerp(   uLoopMult,      uNewLoopMult,       smoothInterp));
        effectMaterial.SetVector("uLoopFinalAdd",   Vector4.Lerp(   uLoopFinalAdd,  uNewLoopFinalAdd,   smoothInterp));
        effectMaterial.SetVector("uFinalColAdd",    Color.Lerp(     uFinalColAdd,   uNewFinalColAdd,    smoothInterp));

        
        lerpInterp += Time.deltaTime * 0.5f;

        if (lerpInterp > 1.0f)
        {
            lerpInterp = 0.0f;

            uRGB                = uNewRGB;            
            uUVMult             = uNewUVMult;
            uLoopAdd            = uNewLoopAdd;
            uLoopMult           = uNewLoopMult;
            uLoopFinalAdd       = uNewLoopFinalAdd;
            uFinalColAdd        = uNewFinalColAdd;

            //Random.ColorHSV(0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f)
            //                              Hue         Sat          Value       Alpha    
            uNewRGB = Random.ColorHSV(0.0f, 1.0f, 0.25f, 1.0f, 0.5f, 1.00f, 1.0f, 1.0f);
            uNewFinalColAdd = Random.ColorHSV(0.0f, 1.0f, 0.25f, 1.0f, 0.0f, 0.25f, 1.0f, 1.0f);

            //uNewUVMult =       new Vector4(Random.Range(5.0f, 2.0f * 5.0f), Random.Range( 5.0f, 2.0f *  5.0f), 0.0f, 0.0f);
            uNewLoopAdd =      new Vector4(Random.Range(0.3f, 2.0f * 0.3f), Random.Range( 0.3f, 2.0f *  0.3f), 0.0f, 1.0f);
            uNewLoopMult =     new Vector4(Random.Range(1.0f, 2.0f * 1.0f), Random.Range(10.0f, 2.0f * 10.0f), 0.0f, 1.0f);
            uNewLoopFinalAdd = new Vector4(Random.Range(4.0f, 2.0f * 4.0f), Random.Range(-4.0f, 2.0f * -4.0f), 0.0f, 1.0f);

            //uNewFinalColAdd = Random.ColorHSV(0.0f, 1.0f, 0.25f, 1.0f, 0.0f, 0.25f, 1.0f, 1.0f);
        }
    }    
}
