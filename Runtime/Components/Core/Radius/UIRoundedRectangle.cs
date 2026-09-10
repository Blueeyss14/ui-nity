using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Uinity
{
    [ExecuteAlways]
    public class UIRoundedRectangle : MaskableGraphic
    {
        [SerializeField] private Radius _radius;
        public int cornerSegments = 64;
        public float antiAliasWidth = 1.75f;

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
            Color32 transparentColor = new Color32(vertColor.r, vertColor.g, vertColor.b, 0);

            float maxRadius = Mathf.Max(rTL, Mathf.Max(rTR, Mathf.Max(rBR, rBL)));
            int adaptiveSegments = Mathf.Clamp(Mathf.CeilToInt(maxRadius * 2.5f), 64, 256);
            int segmentsPerCorner = Mathf.Max(cornerSegments, adaptiveSegments);

            List<Vector2> innerVerts = new List<Vector2>();
            List<Vector2> outerVerts = new List<Vector2>();

            Vector2 cTR = new Vector2(rect.xMax - rTR, rect.yMax - rTR);
            AddCorner(innerVerts, outerVerts, cTR, rTR, antiAliasWidth, 90f, 0f, segmentsPerCorner, new Vector2(1f, 1f));

            Vector2 cBR = new Vector2(rect.xMax - rBR, rect.yMin + rBR);
            AddCorner(innerVerts, outerVerts, cBR, rBR, antiAliasWidth, 0f, -90f, segmentsPerCorner, new Vector2(1f, -1f));

            Vector2 cBL = new Vector2(rect.xMin + rBL, rect.yMin + rBL);
            AddCorner(innerVerts, outerVerts, cBL, rBL, antiAliasWidth, 270f, 180f, segmentsPerCorner, new Vector2(-1f, -1f));

            Vector2 cTL = new Vector2(rect.xMin + rTL, rect.yMax - rTL);
            AddCorner(innerVerts, outerVerts, cTL, rTL, antiAliasWidth, 180f, 90f, segmentsPerCorner, new Vector2(-1f, 1f));

            int count = innerVerts.Count;
            if (count < 3) return;

            UIVertex centerVert = UIVertex.simpleVert;
            centerVert.position = center;
            centerVert.color = vertColor;
            centerVert.uv0 = GetUV(center, rect);
            vh.AddVert(centerVert);

            for (int i = 0; i < count; i++)
            {
                UIVertex vInner = UIVertex.simpleVert;
                vInner.position = innerVerts[i];
                vInner.color = vertColor;
                vInner.uv0 = GetUV(innerVerts[i], rect);
                vh.AddVert(vInner);
            }

            for (int i = 0; i < count; i++)
            {
                UIVertex vOuter = UIVertex.simpleVert;
                vOuter.position = outerVerts[i];
                vOuter.color = transparentColor;
                vOuter.uv0 = GetUV(outerVerts[i], rect);
                vh.AddVert(vOuter);
            }

            for (int i = 0; i < count; i++)
            {
                int curr = i + 1;
                int next = (i == count - 1) ? 1 : i + 2;
                vh.AddTriangle(0, next, curr);
            }

            for (int i = 0; i < count; i++)
            {
                int next = (i + 1) % count;
                int idxInnerCurr = i + 1;
                int idxInnerNext = next + 1;
                int idxOuterCurr = i + 1 + count;
                int idxOuterNext = next + 1 + count;

                vh.AddTriangle(idxInnerCurr, idxOuterCurr, idxOuterNext);
                vh.AddTriangle(idxInnerCurr, idxOuterNext, idxInnerNext);
            }
        }

        private void AddCorner(
            List<Vector2> innerVerts,
            List<Vector2> outerVerts,
            Vector2 cornerCenter,
            float radius,
            float aaWidth,
            float startAngleDeg,
            float endAngleDeg,
            int segments,
            Vector2 cornerSign)
        {
            if (radius <= 0f)
            {
                Vector2 innerPos = cornerCenter;
                Vector2 outerStart = cornerCenter + GetAngleDirection(startAngleDeg) * aaWidth;
                Vector2 outerCorner = cornerCenter + new Vector2(cornerSign.x * aaWidth, cornerSign.y * aaWidth);
                Vector2 outerEnd = cornerCenter + GetAngleDirection(endAngleDeg) * aaWidth;

                innerVerts.Add(innerPos);
                outerVerts.Add(outerStart);

                innerVerts.Add(innerPos);
                outerVerts.Add(outerCorner);

                innerVerts.Add(innerPos);
                outerVerts.Add(outerEnd);
                return;
            }

            float innerR = radius;
            float outerR = radius + aaWidth;

            for (int i = 0; i <= segments; i++)
            {
                float t = (float)i / segments;
                float angleRad = Mathf.Deg2Rad * Mathf.Lerp(startAngleDeg, endAngleDeg, t);
                Vector2 dir = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

                innerVerts.Add(cornerCenter + dir * innerR);
                outerVerts.Add(cornerCenter + dir * outerR);
            }
        }

        private Vector2 GetAngleDirection(float angleDeg)
        {
            float rad = Mathf.Deg2Rad * angleDeg;
            return new Vector2(Mathf.Round(Mathf.Cos(rad)), Mathf.Round(Mathf.Sin(rad)));
        }

        private Vector2 GetUV(Vector2 pos, Rect rect)
        {
            float u = (pos.x - rect.xMin) / rect.width;
            float v = (pos.y - rect.yMin) / rect.height;
            return new Vector2(u, v);
        }
    }
}
