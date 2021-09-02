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

    private const int threadsPerGroup = 24;

    private int xThreadGroups = Mathf.CeilToInt(Screen.width / threadsPerGroup);
    private int yThreadGroups = Mathf.CeilToInt(Screen.height / threadsPerGroup);

    private int lastWidth = Screen.width;
    private int lastHeight = Screen.height;


    private void Awake() => self = this;

    // Start is called before the first frame update
    void Start()
    {

        MatLensFlare.SetTexture("_StarburstTex", starburstTex);

        //BlurPass = postProcMat.FindPass("Blur");

        createTextures();
        setTextures();

        setBloomUniforms();
        setLensFlareUniforms();

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
            //xThreadGroups = Mathf.CeilToInt(Screen.width / threadsPerGroup);
            //yThreadGroups = Mathf.CeilToInt(Screen.height / threadsPerGroup);

            createTextures();
            setTextures();
        }

        //Graphics.Blit(source, sourceTex);

        //Perform Bloom, Result: 'sourceTex'
        if (useBloom)
        {
            setBloomUniforms();
            //bloom.Dispatch(0, xThreadGroups, yThreadGroups, 1);
            Graphics.Blit(source, brightTex, MatBloom, 0);
            blur(brightTex, bloomBlurrAmount);
            Graphics.Blit(source, writeBackBuff);
            Graphics.Blit(writeBackBuff, source, MatBloom, 1);
            //bloom.Dispatch(1, xThreadGroups, yThreadGroups, 1);
        }

        //Perform Chromatic Aberration, Result: 'caResult'
        if (useCA)
        {
            setCAUniforms();
            //Graphics.Blit(sourceTex, caResult, postProcMat, CAPass);
        } 
        else Graphics.Blit(sourceTex, caResult);

        //Perform Lens Flare, Result: 'caResult'
        if (useLensFlare)
        {
            setLensFlareUniforms();
            lensFlare.Dispatch(0, xThreadGroups, yThreadGroups, 1);
            blur(lfResult, lfBlurrCount);
            lensFlare.Dispatch(1, xThreadGroups, yThreadGroups, 1);
        }

        //Perform CRT Effect, Result: 'destination'
        if (useCRTEffect)
        {
            setCRTUniforms();
            //Graphics.Blit(caResult, destination, postProcMat, CRTPass);
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
        //tex.enableRandomWrite = SystemInfo.supportsComputeShaders;
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
        Graphics.Blit(lensDirtTex, lensTex, MatLensFlare, 1);
    }

    private void setTextures()
    {
        if (useBloom) setBloomTextures();
        if (useLensFlare) setLensFlareTextures();
    }

    private void setBloomUniforms()
    {
        //bloom.SetFloat("threshold", bloomThreshold);
    }

    private void setBloomTextures()
    {
        /*bloom.SetTexture(0, "Source", sourceTex);
        bloom.SetTexture(0, "BrightSpots", brightTex);

        bloom.SetTexture(1, "Source", sourceTex);
        bloom.SetTexture(1, "BrightSpots", brightTex);*/
        MatBloom.SetTexture("BrightTex", brightTex);
    }

    private void setLensFlareUniforms()
    {
        lensFlare.SetInt("ghostCount", lfGhostCount);
        lensFlare.SetFloat("ghostSpacing", lfGhostSpacing);
        lensFlare.SetFloat("threshold", lfThreshold);
        lensFlare.SetFloat("caStrength", lfCAStrength);
    }

    private void setLensFlareTextures()
    {
        lensFlare.SetTexture(0, "Source", sourceTex);
        lensFlare.SetTexture(0, "Result", lfResult);
        lensFlare.SetTexture(1, "lensDirt", lensTex);
        lensFlare.SetTexture(1, "Source", caResult);
        lensFlare.SetTexture(1, "Result", lfResult);
        //MatLensFlare.SetTexture();
    }

    private void setCAUniforms()
    {
        //postProcMat.SetFloat("_CAAmount", caAmount);
    }

    private void setCRTUniforms()
    {
        //postProcMat.SetFloat("_vignetteAmount", vignetteAmount);
        //postProcMat.SetFloat("_vignetteWidth", vignetteWidth);
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
