using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundCorner : MaskableGraphic
{
    /// <summary>
    /// Բ��ϵ������һ����0��1
    /// </summary>
    [Range(0, 1.0f)]
    public float radius = 1f;
    //�ṩ�����������ⲿ����Բ��ϵ��
    public float Radius
    {
        get { return radius; }
        set
        {
            radius = value;
            SetVerticesDirty();
        }
    }

    //�����ζ����±�
    [SerializeField]
    private int triangleIdx = 0;
    /// <summary>
    /// ÿ��Բ�������θ���
    /// </summary>
    [Range(3, 200)]
    public int triangleCount = 100;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        //������ʵ�뾶��Χ
        float realRadius;
        if (rectTransform.rect.height < rectTransform.rect.width)
            realRadius = 0.5f * rectTransform.rect.height * radius;
        else
            realRadius = 0.5f * rectTransform.rect.width * radius;
        //ȷ���ĸ��߽�Բ��Բ�ĵ�����
        //���½�Բ��
        Vector2 leftBottomCenter = new Vector2(-0.5f * rectTransform.rect.width + realRadius, -0.5f * rectTransform.rect.height + realRadius);
        //���Ͻ�Բ��
        Vector2 leftTopCenter = new Vector2(-0.5f * rectTransform.rect.width + realRadius, 0.5f * rectTransform.rect.height - realRadius);
        //���Ͻ�Բ��
        Vector2 rightTopCenter = new Vector2(0.5f * rectTransform.rect.width - realRadius, 0.5f * rectTransform.rect.height - realRadius);
        //���½�Բ��
        Vector2 rightBottomCenter = new Vector2(0.5f * rectTransform.rect.width - realRadius, -0.5f * rectTransform.rect.height + realRadius);
        //ȷ�����Ҿ��ζ�������
        var left_corner0 = new Vector2(-0.5f * rectTransform.rect.width, -0.5f * rectTransform.rect.height + realRadius);
        var left_corner1 = new Vector2(-0.5f * rectTransform.rect.width, 0.5f * rectTransform.rect.height - realRadius);
        var right_corner2 = new Vector2(0.5f * rectTransform.rect.width, 0.5f * rectTransform.rect.height - realRadius);
        var right_corner3 = new Vector2(0.5f * rectTransform.rect.width, -0.5f * rectTransform.rect.height + realRadius);
        //ȷ�����¾��ζ�������
        var down_corner0 = new Vector2(-0.5f * rectTransform.rect.width + realRadius, -0.5f * rectTransform.rect.height);
        var up_corner1 = new Vector2(-0.5f * rectTransform.rect.width + realRadius, 0.5f * rectTransform.rect.height);
        var up_corner2 = new Vector2(0.5f * rectTransform.rect.width - realRadius, 0.5f * rectTransform.rect.height);
        var down_corner3 = new Vector2(0.5f * rectTransform.rect.width - realRadius, -0.5f * rectTransform.rect.height);


        //���Ȼ����ĸ��߽�Բ��
        List<Vector2> circleCenters = new List<Vector2>();
        circleCenters.Add(leftBottomCenter);
        circleCenters.Add(leftTopCenter);
        circleCenters.Add(rightTopCenter);
        circleCenters.Add(rightBottomCenter);

        //�����εĻ���
        float angle = 360f / triangleCount * Mathf.Deg2Rad;
        triangleIdx = 0;
        for (int i = 0; i < circleCenters.Count; i++)
        {
            DrawCircle(vh, circleCenters[i], realRadius, triangleCount, angle, color);

        }
        //��������������
        vh.AddUIVertexQuad(GetRectangleQuad(color, left_corner0, left_corner1, right_corner2, right_corner3));
        vh.AddUIVertexQuad(GetRectangleQuad(color, down_corner0, up_corner1, up_corner2, down_corner3));
    }

    /// <summary>
    /// ��Բ
    /// </summary>
    /// <param name="vh"></param>
    /// <param name="center">Բ������</param>
    /// <param name="r">�뾶</param>
    /// <param name="angle">����</param>
    /// <param name="c">��ɫ</param>
    public void DrawCircle(VertexHelper vh, Vector2 center, float r, int triangle, float angle, Color c)
    {
        //���Բ�ĵ�Ϊ��һ����
        vh.AddVert(GetUIVertex(center, c));
        //��ȡԲ�ϵĵ�
        for (int i = 0; i < triangle; i++)
        {
            float _angle = angle * i;
            //��ʱ��Բ��Ϊ0,0��,�Ҵ�Բ�ζ�����ʼ����
            Vector2 borderXY = new Vector2(r * Mathf.Sin(_angle), r * Mathf.Cos(_angle));
            //ӳ�䵽�����ϵĵ�ӦΪ
            Vector2 borderPos = center + borderXY;
            vh.AddVert(GetUIVertex(borderPos, c));
        }
        //����Բ��������
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
    /// ��ȡ������Ƭ
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
    /// ��ȡUI����
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
