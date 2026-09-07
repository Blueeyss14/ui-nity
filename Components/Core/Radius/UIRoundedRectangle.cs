using UnityEngine;
using UnityEngine.UI;

namespace Uinity
{
    [ExecuteAlways]
    public class UIRoundedRectangle : MaskableGraphic
    {
        [SerializeField] private Radius _radius;
        public int cornerSegments = 8;

        public Radius radius
        {
            get => _radius;
            set
            {
                _radius = value;
                SetVerticesDirty();
            }
        }

        public override Texture mainTexture => s_WhiteTexture;

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            Rect rect = rectTransform.rect;
            float width = rect.width;
            float height = rect.height;

            if (width <= 0f || height <= 0f)
                return;

            float rTL = _radius.TopLeft;
            float rTR = _radius.TopRight;
            float rBR = _radius.BottomRight;
            float rBL = _radius.BottomLeft;

            if (_radius.IsFull)
            {
                float minSide = Mathf.Min(width, height) * 0.5f;
                rTL = rTR = rBR = rBL = minSide;
            }
            else
            {
                float maxAllowedRadius = Mathf.Min(width, height) * 0.5f;
                rTL = Mathf.Clamp(rTL, 0f, maxAllowedRadius);
                rTR = Mathf.Clamp(rTR, 0f, maxAllowedRadius);
                rBR = Mathf.Clamp(rBR, 0f, maxAllowedRadius);
                rBL = Mathf.Clamp(rBL, 0f, maxAllowedRadius);

                float scaleW1 = (rTL + rTR > width) ? width / (rTL + rTR) : 1f;
                float scaleW2 = (rBL + rBR > width) ? width / (rBL + rBR) : 1f;
                float scaleH1 = (rTL + rBL > height) ? height / (rTL + rBL) : 1f;
                float scaleH2 = (rTR + rBR > height) ? height / (rTR + rBR) : 1f;

                float scale = Mathf.Min(scaleW1, Mathf.Min(scaleW2, Mathf.Min(scaleH1, scaleH2)));
                if (scale < 1f)
                {
                    rTL *= scale;
                    rTR *= scale;
                    rBR *= scale;
                    rBL *= scale;
                }
            }

            Vector2 center = rect.center;
            Color32 vertColor = this.color;

            UIVertex centerVert = UIVertex.simpleVert;
            centerVert.position = center;
            centerVert.color = vertColor;
            centerVert.uv0 = GetUV(center, rect);
            vh.AddVert(centerVert);

            int segmentsPerCorner = Mathf.Max(1, cornerSegments);

            Vector2 cTR = new Vector2(rect.xMax - rTR, rect.yMax - rTR);
            AddArc(vh, cTR, rTR, 90f, 0f, segmentsPerCorner, rect, vertColor);

            Vector2 cBR = new Vector2(rect.xMax - rBR, rect.yMin + rBR);
            AddArc(vh, cBR, rBR, 0f, -90f, segmentsPerCorner, rect, vertColor);

            Vector2 cBL = new Vector2(rect.xMin + rBL, rect.yMin + rBL);
            AddArc(vh, cBL, rBL, 270f, 180f, segmentsPerCorner, rect, vertColor);

            Vector2 cTL = new Vector2(rect.xMin + rTL, rect.yMax - rTL);
            AddArc(vh, cTL, rTL, 180f, 90f, segmentsPerCorner, rect, vertColor);

            int totalVerts = vh.currentVertCount;
            for (int i = 1; i < totalVerts; i++)
            {
                int next = (i == totalVerts - 1) ? 1 : i + 1;
                vh.AddTriangle(0, next, i);
            }
        }

        private void AddArc(VertexHelper vh, Vector2 cornerCenter, float radius, float startAngleDeg, float endAngleDeg, int segments, Rect rect, Color32 vertColor)
        {
            if (radius <= 0f)
            {
                Vector2 pos = cornerCenter;
                UIVertex vert = UIVertex.simpleVert;
                vert.position = pos;
                vert.color = vertColor;
                vert.uv0 = GetUV(pos, rect);
                vh.AddVert(vert);
                return;
            }

            for (int i = 0; i <= segments; i++)
            {
                float t = (float)i / segments;
                float angleRad = Mathf.Deg2Rad * Mathf.Lerp(startAngleDeg, endAngleDeg, t);
                Vector2 pos = cornerCenter + new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * radius;

                UIVertex vert = UIVertex.simpleVert;
                vert.position = pos;
                vert.color = vertColor;
                vert.uv0 = GetUV(pos, rect);
                vh.AddVert(vert);
            }
        }

        private Vector2 GetUV(Vector2 pos, Rect rect)
        {
            float u = (pos.x - rect.xMin) / rect.width;
            float v = (pos.y - rect.yMin) / rect.height;
            return new Vector2(u, v);
        }
    }
}
