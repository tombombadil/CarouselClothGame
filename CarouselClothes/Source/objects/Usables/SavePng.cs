using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ruckcat;
using System.IO;
using UnityEngine.Experimental.Rendering;

public class SavePng : HyperSceneObj
{

    public MeshRenderer Toobj;

    public override void Update()
    {
        base.Update();


        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            SaveTextureToPng(Toobj);
        }
    }

    private void SaveTextureToPng(MeshRenderer ObjectRend)
    {
        Texture2D tex = toTexture2D((RenderTexture)ObjectRend.material.GetTexture("_BaseMap"));

        //then Save To Disk as PNG
        byte[] bytes = tex.EncodeToPNG();
        var dirPath = Application.dataPath + "/SaveImages/";
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        int RandNum = UnityEngine.Random.Range(0, 99999);
        File.WriteAllBytes(dirPath + RandNum.ToString() + ".png", bytes);
        Debug.Log(dirPath + RandNum.ToString() + ".png"+ bytes);
    }

    private Texture2D toTexture2D(RenderTexture rt)
    {
        var texture = new Texture2D(rt.width, rt.height, rt.graphicsFormat, 0, TextureCreationFlags.None);
        RenderTexture.active = rt;
        texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        texture.Apply();

        RenderTexture.active = null;

        return texture;
    }
}
