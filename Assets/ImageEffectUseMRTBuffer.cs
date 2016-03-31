using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class ImageEffectUseMRTBuffer : MonoBehaviour, IDisposable
{
  private const CameraEvent EVENT_TIMING = CameraEvent.BeforeImageEffectsOpaque;

  public Material imgEffMat;

  public RenderTexture camTarget;
  private RenderTexture buf1;
  private RenderTexture buf2;

  private CommandBuffer customImgEff;

  private Camera cam;

  private void Awake()
  {
    cam = GetComponent<Camera>();

    buf1 = RenderTexture.GetTemporary(cam.pixelWidth, cam.pixelHeight);
    buf2 = RenderTexture.GetTemporary(cam.pixelWidth, cam.pixelHeight);

    customImgEff = new CommandBuffer();
    RenderTargetIdentifier[] ids = new RenderTargetIdentifier[2] { buf1, buf2 };
    RenderTargetIdentifier tar = new RenderTargetIdentifier(camTarget);
    imgEffMat.SetTexture("_Buf1", buf1);
    imgEffMat.SetTexture("_Buf2", buf2);
    customImgEff.name = "CunstomImageEffect";
    customImgEff.Blit(tar, tar, imgEffMat);

    cam.AddCommandBuffer(EVENT_TIMING, customImgEff);
    cam.SetTargetBuffers(new RenderBuffer[2] { buf1.colorBuffer, buf2.colorBuffer }, buf1.depthBuffer);
  }

  public void OnDisable()
  {
    Dispose();
  }

  public void OnDestroy()
  {
    Dispose();
  }

  public void OnPreRender()
  {
    GL.Clear(true, true, Color.black);
    Graphics.DrawTexture(cam.pixelRect, camTarget);
  }

  public void Dispose()
  {
    if(buf1 != null)
      buf1.Release();
    if(buf2 != null)
      buf2.Release();
    cam.RemoveCommandBuffer(EVENT_TIMING, customImgEff);
  }
}