using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using GMEngine;
public class LightTracker : MonoBehaviour
{
    //public LightSensitiveSO m_LightSensitiveSO;
    RenderTexture m_LightTexture;
    Camera m_Tracker;
    public Transform m_receiver;
    public float m_width;
    public float m_height;
    void Start()
    {
        m_Tracker = GetComponent<Camera>();
        m_LightTexture = m_Tracker.targetTexture;
    }

    void FixedUpdate()
    {
        CaluculateColor();
    }
    void CaluculateColor()
    {
        Texture2D readableTexture = new Texture2D(Screen.width, Screen.height) ;
        Debug.Log("current screen width is" + Screen.width + " current screen width is " + Screen.height);
        Debug.Log("i have" + readableTexture.mipmapCount + "mipmap");
        Vector3 receiverTranformSS = m_Tracker.WorldToScreenPoint(m_receiver.position); //The z position is in world units from the camera
        Debug.Log(receiverTranformSS);
        Vector4 color = Color.white;

        //Use the returned array immediately. If you store the array and use it later,
        //it might not point to the correct memory location if the texture has been modified or updated.
        //NativeArray<Color32> color32s = readableTexture.GetPixelData<Color32>(4);

        color = readableTexture.GetPixelBilinear(receiverTranformSS.x, receiverTranformSS.y);
        float m_magnitude = color.magnitude;
        Debug.Log(color);
        //color32s.Dispose();

        UnityEngine.Object.Destroy(readableTexture);
    }
}
