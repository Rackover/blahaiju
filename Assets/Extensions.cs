namespace LouveSystems.LagOps
{

    using System;
    using UnityEngine;

    public static class Extensions
    {
        public static string UnifyDirectorySeparators(this string path)
        {
            return path.Replace('\\' == System.IO.Path.DirectorySeparatorChar ? '/' : '\\', System.IO.Path.DirectorySeparatorChar);
        }

        public static void SetLayerAndChildren(this GameObject g, int layer)
        {
            g.layer = layer;

            foreach (Transform child in g.transform)
            {
                if (null == child)
                {
                    continue;
                }

                child.gameObject.SetLayerAndChildren(layer);
            }
        }

        public static float SquaredDistance(this Vector3Int vec3, Vector3Int otherVec3)
        {
            return Vector3.SqrMagnitude(vec3 - otherVec3);
        }

        public static float SquaredDistance(this Vector3 vec3, Vector3 otherVec3)
        {
            return Vector3.SqrMagnitude(vec3 - otherVec3);
        }

        public static Vector2 Rotate(this Vector2 vec2, float deltaRadians)
        {
            return Rotate(vec2.x, vec2.y, deltaRadians);
        }

        public static Vector2 Rotate(float x, float y, float deltaRadians)
        {
            return new Vector2(
                x * Mathf.Cos(deltaRadians) - y * Mathf.Sin(deltaRadians),
                x * Mathf.Sin(deltaRadians) + y * Mathf.Cos(deltaRadians)
            );
        }

        public static Vector2Int Rotate(this Vector2Int direction, float deltaRadians)
        {
            var rot = Rotate(direction.x, direction.y, deltaRadians);
            return new Vector2Int(Mathf.RoundToInt(rot.x), Mathf.RoundToInt(rot.y));
        }

        public static Vector3Int Rotate(this Vector3Int direction, float deltaRadians)
        {
            var rot = Rotate(new Vector2Int(direction.x, direction.z), deltaRadians);
            return new Vector3Int(rot.x, direction.y, rot.y);
        }

        public static Vector3 Rotate(this Vector3 direction, float deltaRadians)
        {
            var rot = Rotate(direction.x, direction.z, deltaRadians);
            return new Vector3(rot.x, direction.y, rot.y);
        }

        public static Vector3Int Round(this Vector3 vec)
        {
            return new Vector3Int(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y), Mathf.RoundToInt(vec.z));
        }

        public static Texture2D DeCompress(this Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                        source.width,
                        source.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }

        public static string Format(this string s, params object[] ars)
        {
            return string.Format(s, ars);
        }

        public static string Truncate(this string value, int maxChars)
        {
            const string ellipses = "...";
            return value.Length <= maxChars ? value : value.Substring(0, maxChars - ellipses.Length) + ellipses;
        }

        public static Vector3 WithoutY(this Vector3 vec)
        {
            return Vector3.Scale(Vector3.one - Vector3.up, vec);
        }
        public static Vector3 WithoutX(this Vector3 vec)
        {
            return Vector3.Scale(Vector3.one - Vector3.right, vec);
        }
        public static Vector3 WithoutZ(this Vector3 vec)
        {
            return Vector3.Scale(Vector3.one - Vector3.forward, vec);
        }
        public static Vector3Int WithoutY(this Vector3Int vec)
        {
            return Vector3Int.Scale(Vector3Int.one - Vector3Int.up, vec);
        }

        public static string ToFinishTimeString(this TimeSpan finishTime)
        {
            return finishTime.ToString(@"mm\:ss\:fff");
        }

        public static Color ToHexColor(this string colorCode)
        {
            if (colorCode.Length == 7 && colorCode[0] == '#')
            {
                colorCode = colorCode.Substring(1, 6);
            }

            if (colorCode.Length == 6)
            {
                var R = Convert.ToByte(colorCode.Substring(0, 2), 16);
                var G = Convert.ToByte(colorCode.Substring(2, 2), 16);
                var B = Convert.ToByte(colorCode.Substring(4, 2), 16);
                return new Color32(R, G, B, 255);
            }
            else
            {
                throw new FormatException();
            }
        }

        public static void DrawBoxCastBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float distance, Color color)
        {
            direction.Normalize();
            Box bottomBox = new Box(origin, halfExtents, orientation);
            Box topBox = new Box(origin + (direction * distance), halfExtents, orientation);

            Debug.DrawLine(bottomBox.backBottomLeft, topBox.backBottomLeft, color);
            Debug.DrawLine(bottomBox.backBottomRight, topBox.backBottomRight, color);
            Debug.DrawLine(bottomBox.backTopLeft, topBox.backTopLeft, color);
            Debug.DrawLine(bottomBox.backTopRight, topBox.backTopRight, color);
            Debug.DrawLine(bottomBox.frontTopLeft, topBox.frontTopLeft, color);
            Debug.DrawLine(bottomBox.frontTopRight, topBox.frontTopRight, color);
            Debug.DrawLine(bottomBox.frontBottomLeft, topBox.frontBottomLeft, color);
            Debug.DrawLine(bottomBox.frontBottomRight, topBox.frontBottomRight, color);

            DrawBox(bottomBox, color);
            DrawBox(topBox, color);
        }

        public static void DrawBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Color color)
        {
            DrawBox(new Box(origin, halfExtents, orientation), color);
        }
        public static void DrawBox(Box box, Color color)
        {
            Debug.DrawLine(box.frontTopLeft, box.frontTopRight, color);
            Debug.DrawLine(box.frontTopRight, box.frontBottomRight, color);
            Debug.DrawLine(box.frontBottomRight, box.frontBottomLeft, color);
            Debug.DrawLine(box.frontBottomLeft, box.frontTopLeft, color);

            Debug.DrawLine(box.backTopLeft, box.backTopRight, color);
            Debug.DrawLine(box.backTopRight, box.backBottomRight, color);
            Debug.DrawLine(box.backBottomRight, box.backBottomLeft, color);
            Debug.DrawLine(box.backBottomLeft, box.backTopLeft, color);

            Debug.DrawLine(box.frontTopLeft, box.backTopLeft, color);
            Debug.DrawLine(box.frontTopRight, box.backTopRight, color);
            Debug.DrawLine(box.frontBottomRight, box.backBottomRight, color);
            Debug.DrawLine(box.frontBottomLeft, box.backBottomLeft, color);
        }

        public struct Box
        {
            public Vector3 localFrontTopLeft { get; private set; }
            public Vector3 localFrontTopRight { get; private set; }
            public Vector3 localFrontBottomLeft { get; private set; }
            public Vector3 localFrontBottomRight { get; private set; }
            public Vector3 localBackTopLeft { get { return -localFrontBottomRight; } }
            public Vector3 localBackTopRight { get { return -localFrontBottomLeft; } }
            public Vector3 localBackBottomLeft { get { return -localFrontTopRight; } }
            public Vector3 localBackBottomRight { get { return -localFrontTopLeft; } }

            public Vector3 frontTopLeft { get { return localFrontTopLeft + origin; } }
            public Vector3 frontTopRight { get { return localFrontTopRight + origin; } }
            public Vector3 frontBottomLeft { get { return localFrontBottomLeft + origin; } }
            public Vector3 frontBottomRight { get { return localFrontBottomRight + origin; } }
            public Vector3 backTopLeft { get { return localBackTopLeft + origin; } }
            public Vector3 backTopRight { get { return localBackTopRight + origin; } }
            public Vector3 backBottomLeft { get { return localBackBottomLeft + origin; } }
            public Vector3 backBottomRight { get { return localBackBottomRight + origin; } }

            public Vector3 origin { get; private set; }

            public Box(Vector3 origin, Vector3 halfExtents, Quaternion orientation) : this(origin, halfExtents)
            {
                Rotate(orientation);
            }
            public Box(Vector3 origin, Vector3 halfExtents)
            {
                this.localFrontTopLeft = new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
                this.localFrontTopRight = new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
                this.localFrontBottomLeft = new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
                this.localFrontBottomRight = new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);

                this.origin = origin;
            }


            public void Rotate(Quaternion orientation)
            {
                localFrontTopLeft = RotatePointAroundPivot(localFrontTopLeft, Vector3.zero, orientation);
                localFrontTopRight = RotatePointAroundPivot(localFrontTopRight, Vector3.zero, orientation);
                localFrontBottomLeft = RotatePointAroundPivot(localFrontBottomLeft, Vector3.zero, orientation);
                localFrontBottomRight = RotatePointAroundPivot(localFrontBottomRight, Vector3.zero, orientation);
            }
        }

        //This should work for all cast types
        static Vector3 CastCenterOnCollision(Vector3 origin, Vector3 direction, float hitInfoDistance)
        {
            return origin + (direction.normalized * hitInfoDistance);
        }

        static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
        {
            Vector3 direction = point - pivot;
            return pivot + rotation * direction;
        }

        public static float ComputeDistanceWith(this string str1, string str2)
        {
            int n = str1.Length;
            int m = str2.Length;
            int[,] distance = new int[n + 1, m + 1]; // matrix
            int cost = 0;
            if (n == 0) return m;
            if (m == 0) return n;
            //init1
            for (int i = 0; i <= n; distance[i, 0] = i++) ;
            for (int j = 0; j <= m; distance[0, j] = j++) ;
            //find min distance
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    cost = (str2.Substring(j - 1, 1) ==
                        str1.Substring(i - 1, 1) ? 0 : 1);
                    distance[i, j] = Mathf.Min(distance[i - 1, j] + 1,
                    distance[i, j - 1] + 1,
                    distance[i - 1, j - 1] + cost);
                }
            }
            return distance[n, m];
        }

    }
}