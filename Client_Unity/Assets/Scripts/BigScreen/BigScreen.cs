using UnityEngine;
using UnityEngine.UI;

public class BigScreen : MonoBehaviour
{
    [SerializeField]
    Renderer mScreen;
    [SerializeField]
    Text mFPS;

    private Color32[] colors;
    private Texture2D texture;

    public const int width = 480;
    public const int height = 320;
    public static bool flag = false;

    private void Start()
    {
        texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Bilinear;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();

        mScreen.material.mainTexture = texture;

        Client client = new Client();
        client.Init();

        Time.fixedDeltaTime = 0.025f;
    }

    private float lastframeTime = 0;
    private void FixedUpdate()
    {
        if (flag)
        {
            //Debug.LogError("Time1: " + Time.timeSinceLevelLoad);
            float time = Time.timeSinceLevelLoad;
            int fps = (int)(1 / (time - lastframeTime));
            mFPS.text = string.Format("{0} FPS", fps);
            lastframeTime = time;

            //UpdateTex();
            texture.SetPixels32(Client.colors);
            texture.Apply();
            flag = false;
            //Debug.LogError("Time2: " + Time.timeSinceLevelLoad);
        }
    }

    //private void UpdateTex()
    //{
    //    if (Client.curFrameBuffer != null)
    //    {
    //        byte[] rgb = Client.curFrameBuffer;
    //        int length = rgb.Length / 3;

    //        if (colors == null)
    //        {
    //            colors = new Color32[length];
    //        }

    //        for (int i = 0; i < rgb.Length; i = i + 3)
    //        {
    //            colors[i / 3] = new Color32(rgb[i + 2], rgb[i + 1], rgb[i], 255);
    //        }

    //        texture.SetPixels32(colors);
    //        texture.filterMode = FilterMode.Bilinear;
    //        texture.wrapMode = TextureWrapMode.Clamp;
    //        texture.Apply();

    //        flag = false;
    //    }
    //}
}