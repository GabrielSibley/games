using UnityEngine;
using System.Collections;

public class BlitDecal : MonoBehaviour {
	
	public Texture2D tex;
	
	public void BlitAdditiveUV(Texture2D src, float u, float v)
	{
		BlitAdditive(src, (int)(u * tex.width), (int)(v * tex.height));
	}
	
	public void BlitAdditive(Texture2D src, int x, int y)
	{
		int width = src.width;
		int srcOffsetX = 0, srcOffsetY = 0;
		if(x < 0)
		{
			width += x;
			srcOffsetX = -x;
			x = 0;			
		}
		if(x + width >= tex.width)
		{
			width = tex.width - x - 1;
		}
		
		int height = src.height;
		if(y < 0)
		{
			height += y;
			srcOffsetY = -y;
			y = 0;
		}
		if(y + height >= tex.height)
		{
			height = tex.height - y - 1;
		}
		
		if(width <= 0 || height <= 0)
		{
			return;
		}
		Color[] srcColors = src.GetPixels(srcOffsetX, srcOffsetY, width, height);
		Color[] targetColors = tex.GetPixels(x, y, width, height);
		for(int i = 0; i < srcColors.Length; i++)
		{
			targetColors[i] = targetColors[i] + srcColors[i];
		}
		tex.SetPixels(x, y, width, height, targetColors);
		tex.Apply();
	}
}
