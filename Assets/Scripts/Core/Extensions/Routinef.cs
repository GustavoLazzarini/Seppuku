//Copyright Galactspace Studio

using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine
{
    public static class Routinef
    {
        public static Coroutine Invoke(Action method, float delay, MonoBehaviour holder, Action onComplete = null)
        {
            return holder.StartCoroutine(InvokeCoroutine());

            IEnumerator InvokeCoroutine()
            {
                yield return new WaitForSeconds(delay);
                method?.Invoke();
                onComplete?.Invoke();
            }
        }
        public static Coroutine Invoke<T>(Action<T> method, float delay, MonoBehaviour holder, T arg, Action onComplete = null)
        {
            return holder.StartCoroutine(InvokeCoroutine());

            IEnumerator InvokeCoroutine()
            {
                yield return new WaitForSeconds(delay);
                method?.Invoke(arg);
                onComplete?.Invoke();
            }
        }
        public static Coroutine Invoke<T1, T2>(Action<T1, T2> method, float delay, MonoBehaviour holder, T1 arg0, T2 arg1, Action onComplete = null)
        {
            return holder.StartCoroutine(InvokeCoroutine());

            IEnumerator InvokeCoroutine()
            {
                yield return new WaitForSeconds(delay);
                method?.Invoke(arg0, arg1);
                onComplete?.Invoke();
            }
        }
        public static Coroutine Invoke<T1, T2, T3>(Action<T1, T2, T3> method, float delay, MonoBehaviour holder, T1 arg0, T2 arg1, T3 arg2, Action onComplete = null)
        {
            return holder.StartCoroutine(InvokeCoroutine());

            IEnumerator InvokeCoroutine()
            {
                yield return new WaitForSeconds(delay);
                method?.Invoke(arg0, arg1, arg2);
                onComplete?.Invoke();
            }
        }
        public static Coroutine Invoke<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method, float delay, MonoBehaviour holder, T1 arg0, T2 arg1, T3 arg2, T4 arg3, Action onComplete = null)
        {
            return holder.StartCoroutine(InvokeCoroutine());

            IEnumerator InvokeCoroutine()
            {
                yield return new WaitForSeconds(delay);
                method?.Invoke(arg0, arg1, arg2, arg3);
                onComplete?.Invoke();
            }
        }

        public static Coroutine Loop(Action method, float delay, int times, MonoBehaviour holder, Action onComplete = null)
        {
            return holder.StartCoroutine(LoopCoroutine());

            IEnumerator LoopCoroutine()
            {
                for (int i = 0; i < times; i++)
                {
                    method?.Invoke();
                    yield return new WaitForSeconds(delay);
                }

                onComplete?.Invoke();
                yield return null;
            }
        }
        public static Coroutine LoopUntil(Action method, Func<bool> condition, float delay, MonoBehaviour holder, Action onComplete = null)
        {
            return holder.StartCoroutine(LoopCoroutine());

            IEnumerator LoopCoroutine()
            {
                while (!condition())
                {
                    method?.Invoke();
                    yield return new WaitForSeconds(delay);
                }

                onComplete?.Invoke();
                yield return null;
            }
        }

        public static Coroutine LoopWhile(Action method, Func<bool> condition, float delay, MonoBehaviour holder, Action onComplete = null)
        {
            return holder.StartCoroutine(LoopCoroutine());

            IEnumerator LoopCoroutine()
            {
                while (condition())
                {
                    method?.Invoke();
                    yield return new WaitForSeconds(delay);
                }

                onComplete?.Invoke();
                yield return null;
            }
        }

        public static Coroutine LoopWhile(Action<float> method, Func<bool> condition, float delay, MonoBehaviour holder, Action onComplete = null)
        {
            float stime = Time.time;
            float time = 0;

            return holder.StartCoroutine(LoopCoroutine());

            IEnumerator LoopCoroutine()
            {
                while (condition())
                {
                    time = Time.time - stime;

                    method?.Invoke(time);
                    yield return new WaitForSeconds(delay);
                }

                onComplete?.Invoke();
                yield return null;
            }
        }

        public static Coroutine Cooldown(Action<bool> method, float delay, MonoBehaviour holder, Action onComplete = null)
        {
            return holder.StartCoroutine(CooldownCoroutine());

            IEnumerator CooldownCoroutine()
            {
                method?.Invoke(false);
                yield return new WaitForSeconds(delay);
                method?.Invoke(true);
                onComplete?.Invoke();
            }
        }

        public static Coroutine InvokeRealtime(Action method, float delay, MonoBehaviour holder, Action onComplete = null)
        {
            return holder.StartCoroutine(InvokeCoroutine());

            IEnumerator InvokeCoroutine()
            {
                yield return new WaitForSecondsRealtime(delay);
                method?.Invoke();
                onComplete?.Invoke();
            }
        }
        public static Coroutine InvokeRealtime<T>(Action<T> method, float delay, MonoBehaviour holder, T arg, Action onComplete = null)
        {
            return holder.StartCoroutine(InvokeCoroutine());

            IEnumerator InvokeCoroutine()
            {
                yield return new WaitForSecondsRealtime(delay);
                method?.Invoke(arg);
                onComplete?.Invoke();
            }
        }
        public static Coroutine InvokeRealtime<T1, T2>(Action<T1, T2> method, float delay, MonoBehaviour holder, T1 arg0, T2 arg1, Action onComplete = null)
        {
            return holder.StartCoroutine(InvokeCoroutine());

            IEnumerator InvokeCoroutine()
            {
                yield return new WaitForSecondsRealtime(delay);
                method?.Invoke(arg0, arg1);
                onComplete?.Invoke();
            }
        }
        public static Coroutine InvokeRealtime<T1, T2, T3>(Action<T1, T2, T3> method, float delay, MonoBehaviour holder, T1 arg0, T2 arg1, T3 arg2, Action onComplete = null)
        {
            return holder.StartCoroutine(InvokeCoroutine());

            IEnumerator InvokeCoroutine()
            {
                yield return new WaitForSecondsRealtime(delay);
                method?.Invoke(arg0, arg1, arg2);
                onComplete?.Invoke();
            }
        }
        public static Coroutine InvokeRealtime<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method, float delay, MonoBehaviour holder, T1 arg0, T2 arg1, T3 arg2, T4 arg3, Action onComplete = null)
        {
            return holder.StartCoroutine(InvokeCoroutine());

            IEnumerator InvokeCoroutine()
            {
                yield return new WaitForSecondsRealtime(delay);
                method?.Invoke(arg0, arg1, arg2, arg3);
                onComplete?.Invoke();
            }
        }

        public static Coroutine LoopRealtime(Action method, float delay, int times, MonoBehaviour holder, Action onComplete = null)
        {
            return holder.StartCoroutine(LoopCoroutine());

            IEnumerator LoopCoroutine()
            {
                for (int i = 0; i < times; i++)
                {
                    method?.Invoke();
                    yield return new WaitForSecondsRealtime(delay);
                }

                onComplete?.Invoke();
                yield return null;
            }
        }
        public static Coroutine LoopUntilRealtime(Action method, Func<bool> condition, float delay, MonoBehaviour holder, Action onComplete = null)
        {
            return holder.StartCoroutine(LoopCoroutine());

            IEnumerator LoopCoroutine()
            {
                while (!condition())
                {
                    method?.Invoke();
                    yield return new WaitForSecondsRealtime(delay);
                }

                onComplete?.Invoke();
                yield return null;
            }
        }
        public static Coroutine LoopWhileRealtime(Action method, Func<bool> condition, float delay, MonoBehaviour holder, Action onComplete = null)
        {
            return holder.StartCoroutine(LoopCoroutine());

            IEnumerator LoopCoroutine()
            {
                while (condition())
                {
                    method?.Invoke();
                    yield return new WaitForSecondsRealtime(delay);
                }

                onComplete?.Invoke();
                yield return null;
            }
        }

        public static Coroutine CooldownRealtime(Action<bool> method, float delay, MonoBehaviour holder, Action onComplete = null)
        {
            return holder.StartCoroutine(CooldownCoroutine());

            IEnumerator CooldownCoroutine()
            {
                method?.Invoke(false);
                yield return new WaitForSecondsRealtime(delay);
                method?.Invoke(true);
                onComplete?.Invoke();
            }
        }
    }
}