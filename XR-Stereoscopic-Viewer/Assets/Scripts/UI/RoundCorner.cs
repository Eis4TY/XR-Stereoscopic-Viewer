using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundCorner : MaskableGraphic
{
    /// <summary>
    /// 圆角系数，归一化从0到1
    /// </summary>
    [Range(0, 1.0f)]
    public float radius = 1f;
    //提供访问器，供外部设置圆角系数
    public float Radius
    {
        get { return radius; }
        set
        {
            radius = value;
            SetVerticesDirty();
        }
    }

    //三角形顶点下标
    [SerializeField]
    private int triangleIdx = 0;
    /// <summary>
    /// 每个圆的三角形个数
    /// </summary>
    [Range(3, 200)]
    public int triangleCount = 100;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        //计算真实半径范围
        float realRadius;
        if (rectTransform.rect.height < rectTransform.rect.width)
            realRadius = 0.5f * rectTransform.rect.height * radius;
        else
            realRadius = 0.5f * rectTransform.rect.width * radius;
        //确定四个边角圆形圆心的坐标
        //左下角圆心
        Vector2 leftBottomCenter = new Vector2(-0.5f * rectTransform.rect.width + realRadius, -0.5f * rectTransform.rect.height + realRadius);
        //左上角圆心
        Vector2 leftTopCenter = new Vector2(-0.5f * rectTransform.rect.width + realRadius, 0.5f * rectTransform.rect.height - realRadius);
        //右上角圆心
        Vector2 rightTopCenter = new Vector2(0.5f * rectTransform.rect.width - realRadius, 0.5f * rectTransform.rect.height - realRadius);
        //右下角圆心
        Vector2 rightBottomCenter = new Vector2(0.5f * rectTransform.rect.width - realRadius, -0.5f * rectTransform.rect.height + realRadius);
        //确定左右矩形顶点坐标
        var left_corner0 = new Vector2(-0.5f * rectTransform.rect.width, -0.5f * rectTransform.rect.height + realRadius);
        var left_corner1 = new Vector2(-0.5f * rectTransform.rect.width, 0.5f * rectTransform.rect.height - realRadius);
        var right_corner2 = new Vector2(0.5f * rectTransform.rect.width, 0.5f * rectTransform.rect.height - realRadius);
        var right_corner3 = new Vector2(0.5f * rectTransform.rect.width, -0.5f * rectTransform.rect.height + realRadius);
        //确定上下矩形顶点坐标
        var down_corner0 = new Vector2(-0.5f * rectTransform.rect.width + realRadius, -0.5f * rectTransform.rect.height);
        var up_corner1 = new Vector2(-0.5f * rectTransform.rect.width + realRadius, 0.5f * rectTransform.rect.height);
        var up_corner2 = new Vector2(0.5f * rectTransform.rect.width - realRadius, 0.5f * rectTransform.rect.height);
        var down_corner3 = new Vector2(0.5f * rectTransform.rect.width - realRadius, -0.5f * rectTransform.rect.height);


        //首先绘制四个边角圆形
        List<Vector2> circleCenters = new List<Vector2>();
        circleCenters.Add(leftBottomCenter);
        circleCenters.Add(leftTopCenter);
        circleCenters.Add(rightTopCenter);
        circleCenters.Add(rightBottomCenter);

        //三角形的弧度
        float angle = 360f / triangleCount * Mathf.Deg2Rad;
        triangleIdx = 0;
        for (int i = 0; i < circleCenters.Count; i++)
        {
            DrawCircle(vh, circleCenters[i], realRadius, triangleCount, angle, color);

        }
        //最后绘制两个矩形
        vh.AddUIVertexQuad(GetRectangleQuad(color, left_corner0, left_corner1, right_corner2, right_corner3));
        vh.AddUIVertexQuad(GetRectangleQuad(color, down_corner0, up_corner1, up_corner2, down_corner3));
    }

    /// <summary>
    /// 画圆
    /// </summary>
    /// <param name="vh"></param>
    /// <param name="center">圆心坐标</param>
    /// <param name="r">半径</param>
    /// <param name="angle">弧度</param>
    /// <param name="c">颜色</param>
    public void DrawCircle(VertexHelper vh, Vector2 center, float r, int triangle, float angle, Color c)
    {
        //添加圆心点为第一个点
        vh.AddVert(GetUIVertex(center, c));
        //获取圆上的点
        for (int i = 0; i < triangle; i++)
        {
            float _angle = angle * i;
            //此时以圆心为0,0点,且从圆形顶部开始计算
            Vector2 borderXY = new Vector2(r * Mathf.Sin(_angle), r * Mathf.Cos(_angle));
            //映射到矩形上的点应为
            Vector2 borderPos = center + borderXY;
            vh.AddVert(GetUIVertex(borderPos, c));
        }
        //绘制圆中三角形
        for (int i = 0; i < triangle; i++)
        {
            if (i == triangle - 1)
                vh.AddTriangle(triangleIdx, i + 1 + triangleIdx, 1 + triangleIdx);
            else
                vh.AddTriangle(triangleIdx, i + 1 + triangleIdx, i + 2 + triangleIdx);
        }
        triangleIdx += triangleCount + 1;
    }

    /// <summary>
    /// 获取矩形面片
    /// </summary>
    public static UIVertex[] GetRectangleQuad(Color _color, params Vector2[] vector2s)
    {
        UIVertex[] vertexs = new UIVertex[vector2s.Length];
        for (int i = 0; i < vertexs.Length; i++)
        {
            vertexs[i] = GetUIVertex(vector2s[i], _color);
        }
        return vertexs;
    }

    /// <summary>
    /// 获取UI顶点
    /// </summary>
    /// <param name="point"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static UIVertex GetUIVertex(Vector2 point, Color _color)
    {
        UIVertex vertex = new UIVertex
        {
            position = point,
            color = _color,
            uv0 = Vector2.zero
        };
        return vertex;
    }
}
