using AOT;
using InsightAR.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class InsightARBackground 
{
    public const string TAG = "InsightARBackground";
    private Camera mARCamera;
    //hsh: 主相机渲染实时阴影内容同时进行addComandBuffer操作，会将手机摄像头纹理异常，表现背景变黑。
    //暂未发现可替换API，目前通过将相机拆分，用一个不渲染任何内容的相机接收摄像头纹理
    //让主相机Depth高于背景相机depth层级，让摄像头纹理始终在3d内容后
    private Camera aRBackgroundCamera;//背景相机作为子节点，放在mARCamera节点下
    public const string ARBackgroundCameraFeature = "ARBackgroundCamera";
    private GameObject backgroundCameraObject = null;
    private Camera ARBackgroundCamera => aRBackgroundCamera;
    private bool mARCameraConfiged = false;
    private Material m_BackgroundMaterial = null;

#if UNITY_IOS
        private Texture2D _videoTextureY = null;
        private Texture2D _videoTextureCbCr = null;
#elif UNITY_ANDROID
    private Texture2D _videoTextureRGBA = null;
#endif
    private int textureTarget = 3553;
    private float[] uvcoords = new float[8] { 0f, 1f, 0f, 0f, 1f, 1f, 1f, 0f };
    private CommandBuffer m_VideoCommandBuffer;

    //mask 暂时不开启
    //private static Material mMaskMaterial = null;
    //private static Camera mMaskCamera = null;   //mask相机，用于人像或者天空分割（场景只存在一个）
    //private static CommandBuffer mMaskCommandBuffer = null;
    //private static Texture2D mMaskTexture = null;
    //private static IntPtr mMaskTexInptr = IntPtr.Zero;

    /// <summary>
    /// 设置AR相机
    /// </summary>
    /// <param name="camera"></param>
    public void SetARCamera(Camera camera) {
        mARCamera = camera;
        backgroundCameraObject = GameObject.Find(ARBackgroundCameraFeature);
        if (backgroundCameraObject)
            GameObject.DestroyImmediate(backgroundCameraObject);
        backgroundCameraObject = new GameObject(ARBackgroundCameraFeature);
        backgroundCameraObject.transform.parent = mARCamera.transform;
        backgroundCameraObject.transform.localPosition = UnityEngine.Vector3.zero;
        backgroundCameraObject.transform.localRotation = UnityEngine.Quaternion.identity;
        aRBackgroundCamera = backgroundCameraObject.AddComponent<Camera>();
        aRBackgroundCamera.depth = mARCamera.depth - 1;
        aRBackgroundCamera.cullingMask = 0;
        aRBackgroundCamera.clearFlags = CameraClearFlags.Depth;
        aRBackgroundCamera.gameObject.hideFlags = HideFlags.NotEditable;
#if !UNITY_EDITOR
        mARCamera.clearFlags = CameraClearFlags.Depth;
#endif
    }

    /// <summary>
    /// 释放AR相机渲染内容
    /// </summary>
    public void ResetConfigARCamera()
    {
        if (mARCameraConfiged)
        {
            //RemoveCommandBuffers(ref mARCamera, ref m_VideoCommandBuffer);
            RemoveCommandBuffers(ref aRBackgroundCamera, ref m_VideoCommandBuffer);
            //RemoveCommandBuffers(ref mMaskCamera, ref mMaskCommandBuffer);
            mARCameraConfiged = false;
        }

        if (m_VideoCommandBuffer != null)
        {
            m_VideoCommandBuffer.Dispose();
            m_VideoCommandBuffer = null;
        }
        //if (mMaskCommandBuffer != null)
        //{
        //    mMaskCommandBuffer.Dispose();
        //    mMaskCommandBuffer = null;
        //}
#if UNITY_IOS
        DetroyTexture2D(ref _videoTextureY);
        DetroyTexture2D(ref _videoTextureCbCr);
#elif UNITY_ANDROID
        DetroyTexture2D(ref _videoTextureRGBA);
#endif
        //DetroyTexture2D(ref mMaskTexture);
    }
    /// <summary>
    /// 更新相机纹理，在LateUpdate中执行
    /// </summary>
    /// <param name="trackResult">算法结果</param>
    /// <param name="arState">算法状态</param>
    public void UpdateARBackground(InsightARResult trackResult, InsightARState arState) {

#if UNITY_IOS
            // Android V1.7.0
            // 2022.11.04版本
            // v1.7.0.14的相机异常问题已经修复. 
            // AndroidiOS应该一致，不过保险起见，还是保留iOS不动地好
            if (arState < InsightARState.Init_OK || arState >= InsightARState.Track_Stop)
            {
                return;
            }
#endif
#if UNITY_ANDROID || UNITY_IOS
#if UNITY_ANDROID && !UNITY_EDITOR
        GL.IssuePluginEvent(UpdateARBackground_AndrHandlePtr, RENDER_EVENT_UPDATE_BG);
#endif
        InsightARTextureHandles handles = InsightARNative.iarlsGetVideoTextureHandles();
        if (handles.textureY == null || handles.textureCbCr == null)
        {
            return;
        }
        if (handles.textureY == System.IntPtr.Zero)
        {
            return;
        }
#if UNITY_IOS
            if (handles.textureCbCr == System.IntPtr.Zero)
            {
                return;
            }
#endif
        if (!mARCameraConfiged)
        {
#if UNITY_IOS
                configARCamera();
#elif UNITY_ANDROID
            textureTarget = handles.textureTarget;
            configARCamera(textureTarget);
#endif
        }
        Resolution currentResolution = Screen.currentResolution;
        // Debug.Log("-ar- current resolution " + currentResolution +" " + handles.textureY.ToString() +" " + textureTarget);
#if UNITY_IOS
            // Texture Y
            _videoTextureY = Texture2D.CreateExternalTexture(currentResolution.width, currentResolution.height,
                TextureFormat.R8, false, false, (System.IntPtr)handles.textureY);
            _videoTextureY.filterMode = FilterMode.Bilinear;
            _videoTextureY.wrapMode = TextureWrapMode.Repeat;
            _videoTextureY.UpdateExternalTexture(handles.textureY);

            // Texture CbCr
            _videoTextureCbCr = Texture2D.CreateExternalTexture(currentResolution.width, currentResolution.height,
                TextureFormat.RG16, false, false, (System.IntPtr)handles.textureCbCr);
            _videoTextureCbCr.filterMode = FilterMode.Bilinear;
            _videoTextureCbCr.wrapMode = TextureWrapMode.Repeat;
            _videoTextureCbCr.UpdateExternalTexture(handles.textureCbCr);

            m_BackgroundMaterial.SetTexture("_textureY", _videoTextureY);
            m_BackgroundMaterial.SetTexture("_textureCbCr", _videoTextureCbCr);

#elif UNITY_ANDROID
        if (_videoTextureRGBA == null
            || _videoTextureRGBA.GetNativeTexturePtr().ToInt32() != handles.textureY.ToInt32())
        {
            _videoTextureRGBA = Texture2D.CreateExternalTexture(currentResolution.width, currentResolution.height,
                TextureFormat.RGBA32, false, false, (System.IntPtr)handles.textureY);
            _videoTextureRGBA.filterMode = FilterMode.Bilinear;
            _videoTextureRGBA.wrapMode = TextureWrapMode.Clamp;
        }
        _videoTextureRGBA.UpdateExternalTexture(handles.textureY);
        m_BackgroundMaterial.SetTexture("_MainTex", _videoTextureRGBA);

#endif
        int isPortrait = Screen.orientation == ScreenOrientation.Portrait ? 1 : Screen.orientation == ScreenOrientation.PortraitUpsideDown ? 1 : 0;
        float rotation = Screen.orientation == ScreenOrientation.Portrait ? -90 : Screen.orientation == ScreenOrientation.PortraitUpsideDown ? 90 : 0;
        float imageAspect = (float)trackResult.param.width / (float)trackResult.param.height;
        float screenAspect = (float)currentResolution.width / (float)currentResolution.height;
        //以短边为单位1对齐，计算图像场边/屏幕长边的比值
        //比值小于1，即图像相对长边比屏幕相对长边短，需要裁切短边
        //比值大于1，即图像相对长边比屏幕相对长边长，需要裁切长边
        float ratio = screenAspect > 1 ? imageAspect / screenAspect : imageAspect * screenAspect;

        float s_ShaderScaleX = 1.0f;
        float s_ShaderScaleY = 1.0f;
#if UNITY_ANDROID
        if (Screen.orientation == ScreenOrientation.LandscapeLeft)
        {
            if (ratio > 1.0f)
            {
                s_ShaderScaleX = 1.0f / ratio;
            }
            else if (ratio < 1.0f)
            {
                s_ShaderScaleY = ratio;
            }
        }
        else if (Screen.orientation == ScreenOrientation.Portrait)
        {
            if (ratio > 1.0f)
            {
                s_ShaderScaleX = 1.0f / ratio;
            }
            else if (ratio < 1.0f)
            {
                s_ShaderScaleY = ratio;
            }
        }

        if (textureTarget == 3553) //GL_TEXTURE_2D
        {

            if (Screen.orientation == ScreenOrientation.LandscapeLeft)
            {
                s_ShaderScaleX = -s_ShaderScaleX;
            }
            else if (Screen.orientation == ScreenOrientation.Portrait)
            {
                float t = s_ShaderScaleX;
                s_ShaderScaleX = s_ShaderScaleY;
                s_ShaderScaleY = t;
            }
            m_BackgroundMaterial.SetFloat("_texCoordScaleX", s_ShaderScaleX);
            m_BackgroundMaterial.SetFloat("_texCoordScaleY", s_ShaderScaleY);
            m_BackgroundMaterial.SetInt("_isPortrait", isPortrait);
        }
        else if (textureTarget == 36197) //GL_TEXTURE_EXTERNAL_OES
        {
            if (Screen.orientation == ScreenOrientation.Portrait)
            {
                float t = s_ShaderScaleX;
                s_ShaderScaleX = s_ShaderScaleY;
                s_ShaderScaleY = t;
            }
            const string topLeftRight = "_UvTopLeftRight";
            const string botLeftRight = "_UvBottomLeftRight";
            float deltaX = (1.0f - s_ShaderScaleX) * 0.5f;
            float deltaY = (1.0f - s_ShaderScaleY) * 0.5f;
            if (Screen.orientation == ScreenOrientation.Portrait)
            {
                uvcoords[0] = 1.0f - deltaY;
                uvcoords[1] = 1.0f - deltaX;
                uvcoords[2] = 1.0f - deltaY;
                uvcoords[3] = 0.0f + deltaX;
                uvcoords[4] = 0.0f + deltaY;
                uvcoords[5] = 1.0f - deltaX;
                uvcoords[6] = 0.0f + deltaY;
                uvcoords[7] = 0.0f + deltaX;
            }
            else if (Screen.orientation == ScreenOrientation.LandscapeLeft)
            {
                uvcoords[0] = 0.0f + deltaX;
                uvcoords[1] = 1.0f - deltaY;
                uvcoords[2] = 1.0f - deltaX;
                uvcoords[3] = 1.0f - deltaY;
                uvcoords[4] = 0.0f + deltaX;
                uvcoords[5] = 0.0f + deltaY;
                uvcoords[6] = 1.0f - deltaX;
                uvcoords[7] = 0.0f + deltaY;
            }
            m_BackgroundMaterial.SetVector(topLeftRight, new UnityEngine.Vector4(uvcoords[0], uvcoords[1], uvcoords[2], uvcoords[3]));
            m_BackgroundMaterial.SetVector(botLeftRight, new UnityEngine.Vector4(uvcoords[4], uvcoords[5], uvcoords[6], uvcoords[7]));
        }
#endif
#if UNITY_IOS
            if (isPortrait == 1)
            {
                s_ShaderScaleX = ratio < 1 ? ratio : ratio > 1 ? (1.0f / ratio) : 1.0f;
            }
            else if (isPortrait == 0)
            {
                s_ShaderScaleY = ratio < 1 ? ratio : ratio > 1 ? (1.0f / ratio) : 1.0f;
            }

            if (textureTarget == 3553) //GL_TEXTURE_2D
            {
#if UNITY_IOS
                UnityEngine.Matrix4x4 m = UnityEngine.Matrix4x4.TRS(UnityEngine.Vector3.zero, UnityEngine.Quaternion.Euler(0.0f, 0.0f, rotation), UnityEngine.Vector3.one);
                m_BackgroundMaterial.SetMatrix("_TextureRotation", m);
#elif UNITY_ANDROID
                s_ShaderScaleX = isPortrait == 1 ? s_ShaderScaleX : -s_ShaderScaleX;
#endif
                m_BackgroundMaterial.SetFloat("_texCoordScaleX", s_ShaderScaleX);
                m_BackgroundMaterial.SetFloat("_texCoordScaleY", s_ShaderScaleY);
                m_BackgroundMaterial.SetInt("_isPortrait", isPortrait);
            }
            else if (textureTarget == 36197) //GL_TEXTURE_EXTERNAL_OES
            {
                const string topLeftRight = "_UvTopLeftRight";
                const string botLeftRight = "_UvBottomLeftRight";
                float deltaX = (1.0f - s_ShaderScaleX) * 0.5f;
                float deltaY = (1.0f - s_ShaderScaleY) * 0.5f;
                if (isPortrait == 1)
                {
                    uvcoords[0] = 1.0f - deltaY;
                    uvcoords[1] = 1.0f - deltaX;
                    uvcoords[2] = 1.0f - deltaY;
                    uvcoords[3] = 0.0f + deltaX;
                    uvcoords[4] = 0.0f + deltaY;
                    uvcoords[5] = 1.0f - deltaX;
                    uvcoords[6] = 0.0f + deltaY;
                    uvcoords[7] = 0.0f + deltaX;
                }
                else
                {
                    uvcoords[0] = 0.0f + deltaX;
                    uvcoords[1] = 1.0f - deltaY;
                    uvcoords[2] = 1.0f - deltaX;
                    uvcoords[3] = 1.0f - deltaY;
                    uvcoords[4] = 0.0f + deltaX;
                    uvcoords[5] = 0.0f + deltaY;
                    uvcoords[6] = 1.0f - deltaX;
                    uvcoords[7] = 0.0f + deltaY;
                }
                m_BackgroundMaterial.SetVector(topLeftRight, new UnityEngine.Vector4(uvcoords[0], uvcoords[1], uvcoords[2], uvcoords[3]));
                m_BackgroundMaterial.SetVector(botLeftRight, new UnityEngine.Vector4(uvcoords[4], uvcoords[5], uvcoords[6], uvcoords[7]));
                
            }
#endif
#endif

    }

#if UNITY_ANDROID
    // Android支持多线程渲染
    private delegate void RenderEventDelegate(int eventId);
    private static RenderEventDelegate RenderThreadHandle = new RenderEventDelegate(RunOnRenderThread);
    private IntPtr UpdateARBackground_AndrHandlePtr = Marshal.GetFunctionPointerForDelegate(RenderThreadHandle);
    private static int RENDER_EVENT_UPDATE_BG = 2;

    [MonoPInvokeCallback(typeof(RenderEventDelegate))]
    private static void RunOnRenderThread(int eventId)
    {
        if (eventId == RENDER_EVENT_UPDATE_BG)
        {
            InsightARNative.iarlsUpdateOnRenderThread();
        }
    }
#endif
    /// <summary>
    /// 更新相机Pose，在LateUpdate中执行
    /// </summary>
    /// <param name="trackResult">算法结果</param>
    /// <param name="arState">算法状态</param>
    public void UpdateARCameraPose(InsightARResult trackResult, InsightARState arState) {
        //iOS重新计算fov
#if UNITY_IOS
            float fov = CameraUtility.CalculateFov(trackResult.param.width, trackResult.param.height
                , Screen.width, Screen.height, trackResult.param.fov);
#elif UNITY_ANDROID
        float fov = trackResult.param.fov[1];
#else
            float fov = 60;
#endif
        if (mARCamera == null) return;

        if (arState > InsightARState.Init_OK && mARCamera.fieldOfView != fov)
        {
            //InsightDebug.Log(TAG, "camera acitve fov is: " + fov);
            mARCamera.fieldOfView = fov;
            if(ARBackgroundCamera) ARBackgroundCamera.fieldOfView = fov;
        }
        mARCamera.transform.position = new UnityEngine.Vector3(
            trackResult.camera.center_u3d[0],
            trackResult.camera.center_u3d[1],
            trackResult.camera.center_u3d[2]);

        if (trackResult.camera.quaternion_u3d[0] == float.Epsilon ||
           trackResult.camera.quaternion_u3d[1] == float.Epsilon ||
           trackResult.camera.quaternion_u3d[2] == float.Epsilon ||
           trackResult.camera.quaternion_u3d[3] == float.Epsilon)
        {
            //如果其中某一个值检测到极小值，则忽略该次设置相机旋转
        }
        else
        {
            mARCamera.transform.rotation = new UnityEngine.Quaternion(
                trackResult.camera.quaternion_u3d[0],
                trackResult.camera.quaternion_u3d[1],
                trackResult.camera.quaternion_u3d[2],
                trackResult.camera.quaternion_u3d[3]
            );
        }
    }
    public void updataInsightARMaskResult(InsightARMaskResult maskResult)
    {
//#if UNITY_IOS
//            maskResult = InsightARNative.iarlsGetLastMaskResult();
//            InsightDebug.Log(TAG, string.Format("mask result: {0}/{1}/{2}/{3}", maskResult.width, maskResult.height, maskResult.maskPtr, maskResult.maskType));
//#endif
//        if (mMaskMaterial == null)
//        {
//            InsightDebug.Log(TAG, "mask material is null");
//#if UNITY_EDITOR
//            mMaskMaterial = new Material(Shader.Find("Unlit/MaskUnlitShader"));
//            mMaskCommandBuffer = new CommandBuffer();
//            mMaskCommandBuffer.Blit(null, BuiltinRenderTextureType.CurrentActive, mMaskMaterial);
//            if (mMaskCamera)
//            {
//                mMaskCamera.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, mMaskCommandBuffer);
//                InsightDebug.Log(TAG, "mask cemera add commandbuffer");
//            }
//#endif
//            return;
//        }
//        if (maskResult.maskPtr == System.IntPtr.Zero)
//            return;

//        if (mMaskTexInptr != maskResult.maskPtr || mMaskTexture == null)
//        {
//            mMaskTexInptr = maskResult.maskPtr;
//#if UNITY_IOS || UNITY_ANDROID
//            mMaskTexture = Texture2D.CreateExternalTexture(maskResult.width, maskResult.height,
//#if UNITY_IOS
//                                TextureFormat.BGRA32,
//#elif UNITY_ANDROID
//                                TextureFormat.RGBA32,
//#endif
//                                false, false, maskResult.maskPtr);
//#else
//                mMaskTexture = null; //todo
//#endif
//            Debug.Log("create: " + maskResult.maskPtr);
//        }
//        else
//        {
//            mMaskTexture.UpdateExternalTexture(maskResult.maskPtr);
//        }
//        mMaskTexture.filterMode = FilterMode.Bilinear;
//        mMaskTexture.wrapMode = TextureWrapMode.Clamp;
//        mMaskMaterial.SetTexture("_MainTex", mMaskTexture);
    }

#region private apis
    private void configARCamera(int textureTarget = 3553)
    {
        InsightDebug.Log(TAG, "-ar- config ar camera target == " + textureTarget);
        m_VideoCommandBuffer = new CommandBuffer();
#if UNITY_IOS
        m_BackgroundMaterial = new Material(Shader.Find("Unlit/ARCameraShader"));
#else
        if (textureTarget == 36197) // GL_TEXTURE_EXTERNAL_OES
        {
            m_BackgroundMaterial = new Material(Shader.Find("VideoPlaneOES"));
        }
        else // GL_TEXTURE_2D（3553）
        {
            m_BackgroundMaterial = new Material(Shader.Find("VideoPlaneNoLight"));
        }
#endif
        mARCameraConfiged = true;

        m_VideoCommandBuffer.Blit(null, BuiltinRenderTextureType.CameraTarget, m_BackgroundMaterial);
        //mARCamera.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, m_VideoCommandBuffer);
        //hsh:Addcommandbuffer问题：将摄像头视频流叠加到 渲染实时阴影的相机时，会出现视频流无效-黑屏问题。通过更换接口进行规避
        ARBackgroundCamera.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, m_VideoCommandBuffer);
        ARBackgroundCamera.AddCommandBuffer(CameraEvent.BeforeGBuffer, m_VideoCommandBuffer);

        // mask相机
        //mMaskCamera = InsightCamerasManager.initMaskCamera();
        //mMaskMaterial = new Material(Shader.Find("Unlit/MaskUnlitShader"));
        //mMaskCommandBuffer = new CommandBuffer();
        //mMaskCommandBuffer.Blit(null, BuiltinRenderTextureType.CurrentActive, mMaskMaterial);
        //if (mMaskCamera)
        //{
        //    mMaskCamera.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, mMaskCommandBuffer);
        //    InsightDebug.Log(TAG, "mask cemera add commandbuffer");
        //}
    }
    private void RemoveCommandBuffers(ref Camera camera, ref CommandBuffer commandBuffer)
    {
        if (camera != null)
        {
            CommandBuffer[] cbs = camera.GetCommandBuffers(CameraEvent.BeforeForwardOpaque);
            if (cbs != null && cbs.Length > 0)
            {
                camera.RemoveCommandBuffer(CameraEvent.BeforeForwardOpaque, commandBuffer);
            }
        }
    }
    private void DetroyTexture2D(ref Texture2D tex)
    {
        if (tex != null)
        {
            UnityEngine.Object.Destroy(tex);
            tex = null;
        }
    }
#endregion
}
