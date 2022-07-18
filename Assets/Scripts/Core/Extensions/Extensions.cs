//Copyright Galactspace Studios 2022

using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

using URandom = UnityEngine.Random;
using Core.Controllers;

namespace Core
{
    public static class Extensions
    {
        public static bool IsMagLowerEqual(this Vector2 target, float value)
        {
            return target.magnitude <= value;
        }
        public static bool IsMagUpperEqual(this Vector2 target, float value)
        {
            return target.magnitude >= value;
        }
        public static bool IsMagLower(this Vector2 target, float value)
        {
            return target.magnitude < value;
        }
        public static bool IsMagUpper(this Vector2 target, float value)
        {
            return target.magnitude > value;
        }

        public static bool IsMagLowerEqual(this Vector3 target, float value)
        {
            return target.magnitude <= value;
        }
        public static bool IsMagUpperEqual(this Vector3 target, float value)
        {
            return target.magnitude >= value;
        }
        public static bool IsMagLower(this Vector3 target, float value)
        {
            return target.magnitude < value;
        }
        public static bool IsMagUpper(this Vector3 target, float value)
        {
            return target.magnitude > value;
        }

        public static Vector2 ToV2(this Vector3 target) => (Vector2)target;
        public static Vector3 ToV3(this Vector2 target) => (Vector3)target;

        public static bool IsBetween(this float target, float min, float max)
        {
            return min < target && max > target;
        }
        public static bool IsBetweenInclusive(this float target, float min, float max)
        {
            return min <= target && max >= target;
        }

        public static bool IsNull(this object target) => target == null;
        public static bool NotNull(this object target) => target != null;

        public static T GetURandom<T>(this IList<T> target)
        {
            if (target == null) return default;
            return target[URandom.Range(0, target.Count)];
        }
        public static T GetURandom<T>(this T[] target)
        {
            if (target == null) return default;
            return target[URandom.Range(0, target.Length)];
        }
        public static T GetElement<T>(this T[] target, int index)
        {
            if (target == null) return default;
            return target[Mathf.Clamp(index, 0, target.Length)];
        }

        public static Vector2 SumAll(this Vector2[] vectors)
        {
            Vector2 returnValue = Vector2.zero;
            foreach (Vector2 vec in vectors) returnValue += vec;
            return returnValue;
        }
        public static Vector2 SumAll(this List<Vector2> vectors)
        {
            Vector2 returnValue = Vector2.zero;
            foreach (Vector2 vec in vectors) returnValue += vec;
            return returnValue;
        }

        public static Vector3 SumAll(this Vector3[] vectors)
        {
            Vector3 returnValue = Vector3.zero;
            foreach (Vector3 vec in vectors) returnValue += vec;
            return returnValue;
        }
        public static Vector3 SumAll(this List<Vector3> vectors)
        {
            Vector3 returnValue = Vector3.zero;
            foreach (Vector3 vec in vectors) returnValue += vec;
            return returnValue;
        }

        public static float SumAll(this float[] floats)
        {
            float returnValue = 0;
            foreach (float f in floats) returnValue += f;
            return returnValue;
        }
        public static float SumAll(this List<float> floats)
        {
            float returnValue = 0;
            foreach (float f in floats) returnValue += f;
            return returnValue;
        }

        public static float SumAll(this int[] ints)
        {
            int returnValue = 0;
            foreach (int i in ints) returnValue += i;
            return returnValue;
        }
        public static float SumAll(this List<int> ints)
        {
            int returnValue = 0;
            foreach (int i in ints) returnValue += i;
            return returnValue;
        }

        public static bool IsPlayer(this Collision2D collision) => collision.transform.IsPlayer();
        public static bool IsPlayer(this Collision collision) => collision.transform.IsPlayer();
        public static bool IsPlayer(this Component component) => component.CompareTag("Player");
    
        public static bool InsideOf<T>(this T target, params T[] list)
        {
            if (target.IsNull()) throw new System.ArgumentNullException("target");
            return list.Contains(target);
        }
        public static void InvokeCondition(this bool target, Action onTrue, Action onFalse)
        {
            if (target)
            {
                onTrue?.Invoke();
                return;
            }

            onFalse?.Invoke();
        }

        public static bool IsInsideCollider(this Vector3 target, Vector3 colliderPos, Vector3 colliderSize)
        {
            float x = target.x;
            float y = target.y;
            float z = target.z;

            float borderLeft = colliderPos.x - (colliderSize.x / 2);
            float borderRight = colliderPos.x + (colliderSize.x / 2);

            float borderTop = colliderPos.y + (colliderSize.y / 2);
            float borderBottom = colliderPos.y - (colliderSize.y / 2);

            float borderFront = colliderPos.z - (colliderSize.z / 2);
            float borderBack = colliderPos.z + (colliderSize.z / 2);

            if (!(x > borderLeft && x < borderRight)) return false;
            if (!(y > borderBottom && y < borderTop)) return false;
            if (!(z > borderFront && z < borderBack)) return false;

            return true;
        }

        public static void LerpPosition(this GameObject target, UnityEngine.MonoBehaviour holder, Vector3 targetPosition, float speed, float threshold = 0.01f)
        {
            Routinef.LoopWhile(() => target.transform.position = Vector3.Lerp(target.transform.position, targetPosition, Time.fixedDeltaTime * speed),
                () => (target.transform.position - targetPosition).magnitude > threshold, Time.fixedDeltaTime, holder);
        }

        public static void LerpRotation(this GameObject target, UnityEngine.MonoBehaviour holder, Vector3 targetEuler, float speed, float threshold = 0.01f)
        {
            Routinef.LoopWhile(() => target.transform.localEulerAngles = Vector3.Lerp(target.transform.localEulerAngles, targetEuler, Time.fixedDeltaTime * speed),
                () => (target.transform.localEulerAngles - targetEuler).magnitude > threshold, Time.fixedDeltaTime, holder);
        }
    
        public static Vector3 Abs(this Vector3 target) => new Vector3(Mathf.Abs(target.x), Mathf.Abs(target.y), Mathf.Abs(target.z));
    
        public static bool HasNull(params object[] objs)
        {
            return objs.Any(x => x == null);
        }

        public static void RegisterOnEnter(this CubeCollider[] colliders, Action onEnter)
        {
            foreach (CubeCollider c in colliders) c.OnEnter += onEnter;
        }
        public static void UnregisterOnEnter(this CubeCollider[] colliders, Action onEnter)
        {
            foreach (CubeCollider c in colliders) c.OnEnter -= onEnter;
        }

        public static void RegisterOnExit(this CubeCollider[] colliders, Action onEnter)
        {
            foreach (CubeCollider c in colliders) c.OnExit += onEnter;
        }
        public static void UnregisterOnExit(this CubeCollider[] colliders, Action onEnter)
        {
            foreach (CubeCollider c in colliders) c.OnExit -= onEnter;
        }
    
        public static Vector3 MidPoint(this MonoBehaviour[] target)
        {
            Vector3 mid = target[0].transform.position;
            for (int i = 1; i < target.Length; i++)
                mid = (mid + target[i].transform.position) / 2;
            return mid;
        }

        public static bool ContainsPoint(this CubeCollider[] target, Vector3 point)
        {
            for (int i = 0; i < target.Length; i++)
            {
                if (target[i].InsideCollider(point)) return true;
            }

            return false;
        }
    }
}
