//Made by Galactspace Studios

using Core;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine
{
    public static class Gizmosf
    {
        public static void DrawCircle(Vector3 position, Vector3 normal, float radius, Color fill, Color stroke)
        {
#if UNITY_EDITOR
            Handles.color = fill;
            Handles.DrawSolidDisc(position, normal, radius);
            Handles.color = stroke;
            Handles.DrawWireDisc(position, normal, radius);
#endif
        }

        public static void DrawRectangle(Vector3 position, Vector2 size, Color fill, Color stroke)
        {
#if UNITY_EDITOR
            Rect posRect = new Rect(position.ToV2() - (size / 2), size);
            Handles.DrawSolidRectangleWithOutline(posRect, fill, stroke);
#endif
        }

        public static void DrawSphere(Vector3 position, float radius, Color color)
        {
#if UNITY_EDITOR
            Gizmos.color = color;
            Gizmos.DrawSphere(position, radius);
#endif
        }

        public static void DrawCube(Vector3 position, Vector3 size, Color color)
        {
#if UNITY_EDITOR
            Gizmos.color = color;
            Gizmos.DrawCube(position, size);
#endif            
        }

        public static void DrawCubeWithBorder(Vector3 position, Vector3 size, Color color, Color border)
        {
#if UNITY_EDITOR
            Gizmos.color = color;
            Gizmos.DrawCube(position, size);
            Gizmos.color = border;
            Gizmos.DrawWireCube(position, size);
#endif            
        }

        public static void DrawCubeWithBorder(Vector3 position, Vector3 size, Color color, Color border, Matrix4x4 matrix)
        {
#if UNITY_EDITOR
            Matrix4x4 m = Gizmos.matrix;
            Gizmos.matrix = matrix;

            Gizmos.color = color;
            Gizmos.DrawCube(position, size);
            Gizmos.color = border;
            Gizmos.DrawWireCube(position, size);

            Gizmos.matrix = m;
#endif            
        }

        public static void DrawLine(Vector3 start, Vector3 end, Color color, float thickness)
        {
#if UNITY_EDITOR
            Handles.color = color;
            Handles.DrawLine(start, end, thickness);
#endif            
        }
    }
}
