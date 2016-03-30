using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class NewBehaviourScript : MonoBehaviour
{
  public Material imgEffMat;
  public RenderTexture camTarget;
  public RenderTexture buf1;
  public RenderTexture buf2;

  private Camera cam;

  private void Awake()
  {
    cam = GetComponent<Camera>();

    CommandBuffer customImgEff = new CommandBuffer();
    RenderTargetIdentifier[] ids = new RenderTargetIdentifier[2] { buf1, buf2 };
    RenderTargetIdentifier tar = new RenderTargetIdentifier(camTarget);
    imgEffMat.SetTexture("_Buf1", buf1);
    imgEffMat.SetTexture("_Buf2", buf2);
    customImgEff.name = "CunstomImageEffect";
    customImgEff.Blit(tar, tar, imgEffMat);

    cam.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, customImgEff);
    cam.SetTargetBuffers(new RenderBuffer[2] { buf1.colorBuffer, buf2.colorBuffer }, buf1.depthBuffer);
  }

  public void OnPreRender()
  {
    GL.Clear(true, true, Color.black);
    Graphics.DrawTexture(cam.pixelRect, camTarget);
  }
}