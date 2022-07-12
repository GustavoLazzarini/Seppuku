//Made by Galactspace Studios

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace System.Threading.Tasks
{
    public static class Taskf
    {
        private static Dictionary<int, bool> _runningInvokeRepeating = new Dictionary<int, bool>();
        private static int GetNewTaskId()
        {
            int taskId = 0;
            while (_runningInvokeRepeating.ContainsKey(taskId)) taskId++;
            return taskId;
        }

        public static Task WaitSeconds(float seconds) => Task.Delay((int)(seconds * 1000));
        
        public static async void Invoke(Action method, float delay)
        {
            await WaitSeconds(delay);
            method?.Invoke();
        }
        public static async void Invoke<T>(Action<T> method, float delay, T arg)
        {
            await WaitSeconds(delay);
            method?.Invoke(arg);
        }
        public static async void Invoke<T, T2>(Action<T, T2> method, float delay, T arg0, T2 arg1)
        {
            await WaitSeconds(delay);
            method?.Invoke(arg0, arg1);
        }
        public static async void Invoke<T, T2, T3>(Action<T, T2, T3> method, float delay, T arg0, T2 arg1, T3 arg2)
        {
            await WaitSeconds(delay);
            method?.Invoke(arg0, arg1, arg2);
        }

        public static void Invoke(Action method, float delay, MonoBehaviour holder)
        {
            holder.StartCoroutine(waitCoroutine());

            IEnumerator waitCoroutine()
            {
                yield return new WaitForSeconds(delay);
                method?.Invoke();
            }
        }

        public static async void InvokeRepeating(Action method, float delay, Action<int> onRegistered = null)
        {
            int taskId = GetNewTaskId();
            _runningInvokeRepeating.Add(taskId, true);
            onRegistered?.Invoke(taskId);

            while (_runningInvokeRepeating.TryGetValue(taskId, out bool value) && value)
            {
#if UNITY_EDITOR
                if (!UnityEditor.EditorApplication.isPlaying)
                {
                    break;
                }
#endif

                method.Invoke();
                await WaitSeconds(delay);
            }

            _runningInvokeRepeating.Remove(taskId);
        }
        public static async void InvokeRepeating<T>(Action<T> method, float delay, T arg, Action<int> onRegistered = null)
        {
            int taskId = GetNewTaskId();
            _runningInvokeRepeating.Add(taskId, true);
            onRegistered?.Invoke(taskId);

            while (_runningInvokeRepeating.TryGetValue(taskId, out bool value) && value)
            {
#if UNITY_EDITOR
                if (!UnityEditor.EditorApplication.isPlaying)
                {
                    break;
                }
#endif

                method.Invoke(arg);
                await WaitSeconds(delay);
            }

            _runningInvokeRepeating.Remove(taskId);
        }
        public static async void InvokeRepeating<T, T2>(Action<T, T2> method, float delay, T arg0, T2 arg1, Action<int> onRegistered = null)
        {
            int taskId = GetNewTaskId();
            _runningInvokeRepeating.Add(taskId, true);
            onRegistered?.Invoke(taskId);

            while (_runningInvokeRepeating.TryGetValue(taskId, out bool value) && value)
            {
#if UNITY_EDITOR
                if (!UnityEditor.EditorApplication.isPlaying)
                {
                    break;
                }
#endif

                method.Invoke(arg0, arg1);
                await WaitSeconds(delay);
            }

            _runningInvokeRepeating.Remove(taskId);
        }
        public static async void InvokeRepeating<T, T2, T3>(Action<T, T2, T3> method, float delay, T arg0, T2 arg1, T3 arg2, Action<int> onRegistered = null)
        {
            int taskId = GetNewTaskId();
            _runningInvokeRepeating.Add(taskId, true);
            onRegistered?.Invoke(taskId);

            while (_runningInvokeRepeating.TryGetValue(taskId, out bool value) && value)
            {
#if UNITY_EDITOR
                if (!UnityEditor.EditorApplication.isPlaying)
                {
                    break;
                }
#endif

                method.Invoke(arg0, arg1, arg2);
                await WaitSeconds(delay);
            }

            _runningInvokeRepeating.Remove(taskId);
        }

        public static void Cooldown(Action<bool> method, float delay, MonoBehaviour holder)
        {
            holder.StartCoroutine(waitCoroutine());

            IEnumerator waitCoroutine()
            {
                method?.Invoke(false);
                yield return new WaitForSeconds(delay);
                method?.Invoke(true);
            }
        }

        public static void LoopUntil(Action loop, Func<bool> condition, float delay, MonoBehaviour holder, Action doAfter = null)
        {
            LoopUntil(loop, condition, delay, holder, out Coroutine c, doAfter);
        }

        public static void LoopUntil(Action loop, Func<bool> condition, float delay, MonoBehaviour holder, out Coroutine coroutine, Action doAfter = null)
        {
            coroutine = holder.StartCoroutine(waitCoroutine());

            IEnumerator waitCoroutine()
            {
                while (!condition())
                {
                    loop?.Invoke();
                    yield return new WaitForSeconds(delay);
                }

                doAfter?.Invoke();
            }
        }

        public static void StopInvokeRepeating(int taskId) => _runningInvokeRepeating[taskId] = false;
    }
}
