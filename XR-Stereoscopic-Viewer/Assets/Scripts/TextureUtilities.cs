using UnityEngine;

public static class TextureUtilities
{
    public static Texture2D ResizeTexture(Texture2D originalTexture, float targetHeight)
    {
        float originalWidth = originalTexture.width;
        float originalHeight = originalTexture.height;

        float targetWidth = (originalWidth / originalHeight) * targetHeight;

        // 创建一个新的纹理
        Texture2D resizedTexture = new Texture2D(Mathf.RoundToInt(targetWidth), Mathf.RoundToInt(targetHeight));

        // 使用Bilinear过滤对原始纹理进行下采样
        for (int i = 0; i < resizedTexture.height; i++)
        {
            for (int j = 0; j < resizedTexture.width; j++)
            {
                Color newColor = originalTexture.GetPixelBilinear((float)j / (float)resizedTexture.width, (float)i / (float)resizedTexture.height);
                resizedTexture.SetPixel(j, i, newColor);
            }
        }

        resizedTexture.Apply();  // 应用改动到纹理

        return resizedTexture;
    }
}
