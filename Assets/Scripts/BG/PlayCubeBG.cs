using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCubeBG : MonoBehaviour
{
    public SpriteRenderer sr;
    public Color minColor;
    public Color maxColor;

    public float MaxScale = 1f;
    public float MinScale = 0.2f;

    public float MoveSpeed = 1f;
    public float ScaleSpeed = 1f;

    private Vector3 targetPos;

    private void Awake()
    {
        targetScale = Random.Range(MinScale, MaxScale);
        transform.localScale = new Vector3(targetScale, targetScale, 1);
        transform.position = new Vector3(Random.Range(-10f, 10f), Random.Range(-7f, 7f), 0);
        targetPos = RandomTargetPos();
    }

    private Vector3 RandomTargetPos()
    {
        Vector3 pos = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-2.5f, 2.5f), 0) + transform.position;
        pos.x = Mathf.Clamp(pos.x, -11, 11);
        pos.y = Mathf.Clamp(pos.y, -7, 7);
        return pos;
    }


    private void Move(float deltaTime)
    {
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            targetPos = RandomTargetPos();
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPos, MoveSpeed * deltaTime);
    }

    public void MyUpdate(float deltaTime)
    {
        Move(deltaTime);
        ScaleUpdate(deltaTime);
    }

    private float targetScale;
    private void ScaleUpdate(float deltaTime)
    {
        if (Mathf.Abs(transform.localScale.x - targetScale) < 0.01f)
            targetScale = Random.Range(MinScale, MaxScale);
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(targetScale, targetScale, 1), deltaTime * ScaleSpeed);
        sr.color = LerpInLab(minColor, maxColor, (transform.localScale.x - MinScale) / (MaxScale - MinScale));
    }

    /// <summary>
    /// 在 L*a*b* 空间中进行颜色插值
    /// </summary>
    Color LerpInLab(Color start, Color end, float t)
    {
        // 转换到 Lab 空间
        Vector3 labStart = RGBToLab(start);
        Vector3 labEnd = RGBToLab(end);

        // 插值
        Vector3 labResult = Vector3.Lerp(labStart, labEnd, t);

        // 转换回 RGB 空间
        return LabToRGB(labResult);
    }

    /// <summary>
    /// 将 RGB 转换为 L*a*b*
    /// </summary>
    Vector3 RGBToLab(Color color)
    {
        // 转换为 XYZ 空间
        Vector3 xyz = RGBToXYZ(color);

        // 转换为 Lab 空间
        float x = xyz.x / 95.047f;
        float y = xyz.y / 100.0f;
        float z = xyz.z / 108.883f;

        x = x > 0.008856f ? Mathf.Pow(x, 1f / 3f) : (7.787f * x) + (16f / 116f);
        y = y > 0.008856f ? Mathf.Pow(y, 1f / 3f) : (7.787f * y) + (16f / 116f);
        z = z > 0.008856f ? Mathf.Pow(z, 1f / 3f) : (7.787f * z) + (16f / 116f);

        float l = (116f * y) - 16f;
        float a = 500f * (x - y);
        float b = 200f * (y - z);

        return new Vector3(l, a, b);
    }

    /// <summary>
    /// 将 L*a*b* 转换为 RGB
    /// </summary>
    Color LabToRGB(Vector3 lab)
    {
        // 转换为 XYZ 空间
        float y = (lab.x + 16f) / 116f;
        float x = lab.y / 500f + y;
        float z = y - lab.z / 200f;

        x = 95.047f * (x * x * x > 0.008856f ? x * x * x : (x - 16f / 116f) / 7.787f);
        y = 100.000f * (y * y * y > 0.008856f ? y * y * y : (y - 16f / 116f) / 7.787f);
        z = 108.883f * (z * z * z > 0.008856f ? z * z * z : (z - 16f / 116f) / 7.787f);

        // 转换为 RGB 空间
        return XYZToRGB(new Vector3(x, y, z));
    }

    /// <summary>
    /// 将 RGB 转换为 XYZ 空间
    /// </summary>
    Vector3 RGBToXYZ(Color color)
    {
        float r = color.r > 0.04045f ? Mathf.Pow((color.r + 0.055f) / 1.055f, 2.4f) : color.r / 12.92f;
        float g = color.g > 0.04045f ? Mathf.Pow((color.g + 0.055f) / 1.055f, 2.4f) : color.g / 12.92f;
        float b = color.b > 0.04045f ? Mathf.Pow((color.b + 0.055f) / 1.055f, 2.4f) : color.b / 12.92f;

        r *= 100f;
        g *= 100f;
        b *= 100f;

        float x = r * 0.4124f + g * 0.3576f + b * 0.1805f;
        float y = r * 0.2126f + g * 0.7152f + b * 0.0722f;
        float z = r * 0.0193f + g * 0.1192f + b * 0.9505f;

        return new Vector3(x, y, z);
    }

    /// <summary>
    /// 将 XYZ 转换为 RGB 空间
    /// </summary>
    Color XYZToRGB(Vector3 xyz)
    {
        float x = xyz.x / 100f;
        float y = xyz.y / 100f;
        float z = xyz.z / 100f;

        float r = x * 3.2406f + y * -1.5372f + z * -0.4986f;
        float g = x * -0.9689f + y * 1.8758f + z * 0.0415f;
        float b = x * 0.0557f + y * -0.2040f + z * 1.0570f;

        r = r > 0.0031308f ? 1.055f * Mathf.Pow(r, 1f / 2.4f) - 0.055f : 12.92f * r;
        g = g > 0.0031308f ? 1.055f * Mathf.Pow(g, 1f / 2.4f) - 0.055f : 12.92f * g;
        b = b > 0.0031308f ? 1.055f * Mathf.Pow(b, 1f / 2.4f) - 0.055f : 12.92f * b;

        return new Color(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b));
    }
}
