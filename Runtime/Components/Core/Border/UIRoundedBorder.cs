using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Uinity
{
    [ExecuteAlways]
    public class UIRoundedBorder : MaskableGraphic
    {
        [SerializeField] private Radius _radius;
        [SerializeField] private float _thickness = 1f;
        public int cornerSegments = 64;
        public float antiAliasWidth = 1.75f;
        public float innerOverlap = 0.5f;

        public Radius radius
        {
            get => _radius;
            set
            {
                _radius = value;
                SetVerticesDirty();
            }
        }

        public float thickness
        {
            get => _thickness;
            set
            {
                _thickness = value;
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

            if (_thickness <= 0f)
                return;

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

            float maxRadius = Mathf.Max(rTL, Mathf.Max(rTR, Mathf.Max(rBR, rBL)));
            int adaptiveSegments = Mathf.Clamp(Mathf.CeilToInt(maxRadius * 3.0f), 64, 256);
            int segmentsPerCorner = Mathf.Max(cornerSegments, adaptiveSegments);

            List<Vector2> ring0Verts = new List<Vector2>();
            List<Vector2> ring1Verts = new List<Vector2>();
            List<Vector2> ring2Verts = new List<Vector2>();
            List<Vector2> ring3Verts = new List<Vector2>();

            Vector2 cTR = new Vector2(rect.xMax - rTR, rect.yMax - rTR);
            AddCornerRings(ring0Verts, ring1Verts, ring2Verts, ring3Verts, cTR, rTR, _thickness, innerOverlap, antiAliasWidth, 90f, 0f, segmentsPerCorner, rect, new Vector2(1f, 1f));

            Vector2 cBR = new Vector2(rect.xMax - rBR, rect.yMin + rBR);
            AddCornerRings(ring0Verts, ring1Verts, ring2Verts, ring3Verts, cBR, rBR, _thickness, innerOverlap, antiAliasWidth, 0f, -90f, segmentsPerCorner, rect, new Vector2(1f, -1f));

            Vector2 cBL = new Vector2(rect.xMin + rBL, rect.yMin + rBL);
            AddCornerRings(ring0Verts, ring1Verts, ring2Verts, ring3Verts, cBL, rBL, _thickness, innerOverlap, antiAliasWidth, 270f, 180f, segmentsPerCorner, rect, new Vector2(-1f, -1f));

            Vector2 cTL = new Vector2(rect.xMin + rTL, rect.yMax - rTL);
            AddCornerRings(ring0Verts, ring1Verts, ring2Verts, ring3Verts, cTL, rTL, _thickness, innerOverlap, antiAliasWidth, 180f, 90f, segmentsPerCorner, rect, new Vector2(-1f, 1f));

            int totalPairs = ring0Verts.Count;
            if (totalPairs < 3)
                return;

            Color32 solidColor = this.color;
            Color32 transparentColor = new Color32(solidColor.r, solidColor.g, solidColor.b, 0);

            for (int i = 0; i < totalPairs; i++)
            {
                UIVertex v = UIVertex.simpleVert;
                v.position = ring0Verts[i];
                v.color = transparentColor;
                v.uv0 = GetUV(ring0Verts[i], rect);
                vh.AddVert(v);
            }

            for (int i = 0; i < totalPairs; i++)
            {
                UIVertex v = UIVertex.simpleVert;
                v.position = ring1Verts[i];
                v.color = solidColor;
                v.uv0 = GetUV(ring1Verts[i], rect);
                vh.AddVert(v);
            }

            for (int i = 0; i < totalPairs; i++)
            {
                UIVertex v = UIVertex.simpleVert;
                v.position = ring2Verts[i];
                v.color = solidColor;
                v.uv0 = GetUV(ring2Verts[i], rect);
                vh.AddVert(v);
            }

            for (int i = 0; i < totalPairs; i++)
            {
                UIVertex v = UIVertex.simpleVert;
                v.position = ring3Verts[i];
                v.color = transparentColor;
                v.uv0 = GetUV(ring3Verts[i], rect);
                vh.AddVert(v);
            }

            int n = totalPairs;

            for (int i = 0; i < n; i++)
            {
                int next = (i + 1) % n;
                int r0Curr = i;
                int r0Next = next;
                int r1Curr = i + n;
                int r1Next = next + n;

                vh.AddTriangle(r0Curr, r1Curr, r1Next);
                vh.AddTriangle(r0Curr, r1Next, r0Next);
            }

            for (int i = 0; i < n; i++)
            {
                int next = (i + 1) % n;
                int r1Curr = i + n;
                int r1Next = next + n;
                int r2Curr = i + 2 * n;
                int r2Next = next + 2 * n;

                vh.AddTriangle(r1Curr, r2Curr, r2Next);
                vh.AddTriangle(r1Curr, r2Next, r1Next);
            }

            for (int i = 0; i < n; i++)
            {
                int next = (i + 1) % n;
                int r2Curr = i + 2 * n;
                int r2Next = next + 2 * n;
                int r3Curr = i + 3 * n;
                int r3Next = next + 3 * n;

                vh.AddTriangle(r2Curr, r3Curr, r3Next);
                vh.AddTriangle(r2Curr, r3Next, r2Next);
            }
        }

        private void AddCornerRings(
            List<Vector2> ring0,
            List<Vector2> ring1,
            List<Vector2> ring2,
            List<Vector2> ring3,
            Vector2 cornerCenter,
            float radius,
            float thickness,
            float overlap,
            float aaWidth,
            float startAngleDeg,
            float endAngleDeg,
            int segments,
            Rect rect,
            Vector2 cornerSign)
        {
            if (radius <= 0f)
            {
                Vector2 dirStart = GetAngleDirection(startAngleDeg);
                Vector2 dirEnd = GetAngleDirection(endAngleDeg);

                Vector2 innerAaStart = cornerCenter - dirStart * (overlap + aaWidth);
                Vector2 innerAaCorner = cornerCenter - new Vector2(cornerSign.x * (overlap + aaWidth), cornerSign.y * (overlap + aaWidth));
                Vector2 innerAaEnd = cornerCenter - dirEnd * (overlap + aaWidth);

                Vector2 innerSolidStart = cornerCenter - dirStart * overlap;
                Vector2 innerSolidCorner = cornerCenter - new Vector2(cornerSign.x * overlap, cornerSign.y * overlap);
                Vector2 innerSolidEnd = cornerCenter - dirEnd * overlap;

                Vector2 outerSolidStart = cornerCenter + dirStart * thickness;
                Vector2 outerSolidCorner = cornerCenter + new Vector2(cornerSign.x * thickness, cornerSign.y * thickness);
                Vector2 outerSolidEnd = cornerCenter + dirEnd * thickness;

                Vector2 outerAaStart = cornerCenter + dirStart * (thickness + aaWidth);
                Vector2 outerAaCorner = cornerCenter + new Vector2(cornerSign.x * (thickness + aaWidth), cornerSign.y * (thickness + aaWidth));
                Vector2 outerAaEnd = cornerCenter + dirEnd * (thickness + aaWidth);

                ring0.Add(innerAaStart);
                ring1.Add(innerSolidStart);
                ring2.Add(outerSolidStart);
                ring3.Add(outerAaStart);

                ring0.Add(innerAaCorner);
                ring1.Add(innerSolidCorner);
                ring2.Add(outerSolidCorner);
                ring3.Add(outerAaCorner);

                ring0.Add(innerAaEnd);
                ring1.Add(innerSolidEnd);
                ring2.Add(outerSolidEnd);
                ring3.Add(outerAaEnd);
                return;
            }

            float r0 = Mathf.Max(0f, radius - overlap - aaWidth);
            float r1 = Mathf.Max(0f, radius - overlap);
            float r2 = radius + thickness;
            float r3 = radius + thickness + aaWidth;

            for (int i = 0; i <= segments; i++)
            {
                float t = (float)i / segments;
                float angleRad = Mathf.Deg2Rad * Mathf.Lerp(startAngleDeg, endAngleDeg, t);
                Vector2 dir = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

                ring0.Add(cornerCenter + dir * r0);
                ring1.Add(cornerCenter + dir * r1);
                ring2.Add(cornerCenter + dir * r2);
                ring3.Add(cornerCenter + dir * r3);
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
