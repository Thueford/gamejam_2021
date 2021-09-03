using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessing : MonoBehaviour
{
    public static PostProcessing self;

    [Header("Shader")]
    //public ComputeShader bloom;
    public ComputeShader lensFlare;
    public Material MatBlur;
    public Material MatBloom;
    public Material MatCA;
    public Material MatLensFlare;
    public Material MatCRT;

    [Header("Lens Textures")]
    public Texture2D lensDirtTex;
    public Texture2D starburstTex;

    [Header("General Settings")]
    public bool skipAll = false;

    [Header("Bloom")]
    public bool useBloom = true;
    [Range(1, 20)] public int bloomBlurrAmount = 4;
    [Range(0.01f, 1f)] public float bloomThreshold = .75f;

    [Header("Chromatic Aberration")]
    public bool useCA = true;
    [Range(0f, 0.01f)] public float caAmount = 0.0005f;

    [Header("Lens Flare")]
    public bool useLensFlare = true;
    [Range(0, 10)] public int lfGhostCount = 6;
    [Range(0, 20)] public int lfBlurrCount = 3;
    [Range(0.01f, 1f)] public float lfThreshold = 0.77f;
    [Range(0.1f, 2f)] public float lfGhostSpacing = .69f;
    [Range(0.0f, 0.3f)] public float lfCAStrength = 0.15f;

    [Header("CRT Monitor Effect")]
    public bool useCRTEffect = true;
    [Range(0.0f, 1f)] public float vignetteAmount = 0.8f;
    [Range(0.0f, 0.4f)] public float vignetteWidth = 0.1f;

    [Header("Textures")]
    public RenderTexture sourceTex;
    public RenderTexture brightTex;
    public RenderTexture blurBuff;
    public RenderTexture caResult;
    public RenderTexture lfResult;
    public RenderTexture lensTex;
    public RenderTexture writeBackBuff;

    private Light dirLight; 

    private int lastWidth = Screen.width;
    private int lastHeight = Screen.height;


    private void Awake() => self = this;

    // Start is called before the first frame update
    void Start()
    {
        MatLensFlare.SetTexture("_StarburstTex", starburstTex);

        createTextures();
        setTextures();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (skipAll)
        {
            Graphics.Blit(source, destination);
            return;
        }
            
        //Recreate Textures if Window was resized
        if (Screen.width != lastWidth ||Screen.height != lastHeight)
        {
            lastWidth = Screen.width;
            lastHeight = Screen.height;

            createTextures();
            setTextures();
        }

        //Perform Bloom, Result: 'sourceTex'
        if (useBloom)
        {
            setBloomUniforms();

            Graphics.Blit(source, brightTex, MatBloom, 0);
            blur(brightTex, bloomBlurrAmount);
            Graphics.Blit(source, sourceTex, MatBloom, 1);

        } else Graphics.Blit(source, sourceTex);

        //Perform Chromatic Aberration, Result: 'caResult'
        if (useCA)
        {
            setCAUniforms();
            Graphics.Blit(sourceTex, caResult, MatCA);
        } 
        else Graphics.Blit(sourceTex, caResult);

        //Perform Lens Flare, Result: 'caResult'
        if (useLensFlare)
        {
            setLensFlareUniforms();

            Graphics.Blit(sourceTex, lfResult, MatLensFlare, 0);
            blur(lfResult, lfBlurrCount);
            Graphics.Blit(caResult, writeBackBuff);
            Graphics.Blit(writeBackBuff, caResult, MatLensFlare, 1);
            //lensFlare.Dispatch(1, xThreadGroups, yThreadGroups, 1);
        }

        //Perform CRT Effect, Result: 'destination'
        if (useCRTEffect)
        {
            setCRTUniforms();
            Graphics.Blit(caResult, destination, MatCRT);
        }
        else Graphics.Blit(caResult, destination);

    }

    private void blur(RenderTexture tex, int count)
    {
        for (int i = 0; i < count; i++)
        {
            MatBlur.SetInt("_horizontal", 1);
            Graphics.Blit(tex, blurBuff, MatBlur);
            MatBlur.SetInt("_horizontal", 0);
            Graphics.Blit(blurBuff, tex, MatBlur);
        }
    }

    private RenderTexture createTexture()
    {
        RenderTexture tex = new RenderTexture(Screen.width, Screen.height, 24);
        tex.Create();
        return tex;
    }

    private void createTextures()
    {
        sourceTex = createTexture();
        brightTex = createTexture();
        blurBuff = createTexture();
        caResult = createTexture();
        lfResult = createTexture();
        lensTex = createTexture();
        writeBackBuff = createTexture();
        Graphics.Blit(lensDirtTex, lensTex, MatLensFlare, 2);
    }

    private void setTextures()
    {
        setBloomTextures();
        setLensFlareTextures();
    }

    private void setBloomUniforms()
    {
        MatBloom.SetFloat("_Threshold", bloomThreshold);
    }

    private void setBloomTextures()
    {
        MatBloom.SetTexture("_BrightTex", brightTex);
    }

    private void setLensFlareUniforms()
    {
        MatLensFlare.SetInt("_GhostCount", lfGhostCount);
        MatLensFlare.SetFloat("_GhostSpacing", lfGhostSpacing);
        MatLensFlare.SetFloat("_Threshold", lfThreshold);
        MatLensFlare.SetFloat("_CaStrength", lfCAStrength);
    }

    private void setLensFlareTextures()
    {
        MatLensFlare.SetTexture("_LensTex", lensTex);
        MatLensFlare.SetTexture("_ResultTex", lfResult);
    }

    private void setCAUniforms()
    {
        MatCA.SetFloat("_CAAmount", caAmount);
    }

    private void setCRTUniforms()
    {
        MatCRT.SetFloat("_vignetteAmount", vignetteAmount);
        MatCRT.SetFloat("_vignetteWidth", vignetteWidth);
    }

    public void StartPlayerHitEffect(float duration)
    {
        StartCoroutine(EndPlayerHitEffect(duration));
    }

    private IEnumerator EndPlayerHitEffect(float duration)
    {
        const float changeInterval = 0.05f;
        float originalCAAmount = caAmount, 
            originalLightIntensity = dirLight.intensity;
        int count = (int)(duration / changeInterval);

        for (int i = 0; i < count; i++)
        {
            caAmount = Random.Range(0.001f, 0.01f);
            dirLight.intensity = Random.Range(0, 0.3f);
            yield return new WaitForSeconds(changeInterval);
        }
        caAmount = originalCAAmount;
        dirLight.intensity = originalLightIntensity;
        yield return true;
    }
}
