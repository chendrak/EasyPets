using System;
using System.Collections;
using UnityEngine;

namespace EasyPets.Helpers;

public static class CoroutineHelper
{
    private static readonly GameObject managerGo;
    private static readonly CoroutineHelperBehaviour _behaviour;
    
    static CoroutineHelper()
    {
        managerGo = new GameObject { hideFlags = HideFlags.HideAndDontSave };
        _behaviour = managerGo.AddComponent<CoroutineHelperBehaviour>();
    }

    public static void StartCoroutine(IEnumerator coroutineMethod)
    {
        _behaviour.StartCoroutine(coroutineMethod);
    }

    public static void StopCoroutine(IEnumerator coroutineMethod)
    {
        _behaviour.StartCoroutine(coroutineMethod);
    }
    
    private static Coroutine StartCoroutine(this MonoBehaviour self, IEnumerator coroutine) => self.StartCoroutine(coroutine);
    private static void StopCoroutine(this MonoBehaviour self, IEnumerator coroutine) => self.StopCoroutine(coroutine);

    private class CoroutineHelperBehaviour : MonoBehaviour { }
}