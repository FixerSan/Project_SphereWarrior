using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Transform = UnityEngine.Transform;
using Object = UnityEngine.Object;

public static class Util
{
    public static T GetOrAddComponent<T>(GameObject _go) where T : UnityEngine.Component
    {
        T component = _go.GetComponent<T>();
        if (component == null)
            component = _go.AddComponent<T>();

        return component;
    }
    public static T GetOrAddComponent<T>(Transform _trans) where T : UnityEngine.Component
    {
        T component = _trans.GetComponent<T>();
        if (component == null)
            component = _trans.gameObject.AddComponent<T>();

        return component;
    }

    public static T FindChild<T>(GameObject _go, string _name = null, bool _recursive = false) where T : Object
    {
        if (_go == null) return null;
        if (!_recursive)
        {
            for (int i = 0; i < _go.transform.childCount; i++)
            {
                Transform transform = _go.transform.GetChild(i);
                if (string.IsNullOrEmpty(_name) || transform.name == _name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }

        else
        {
            foreach (T component in _go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(_name) || component.name == _name)
                {
                    return component;
                }
            }
        }
        return null;
    }

    public static GameObject FindChild(GameObject _go, string _name = null, bool _recursive = false)
    {
        Transform transform = FindChild<Transform>(_go, _name, _recursive);
        if (transform != null)
            return transform.gameObject;

        return null;
    }

    public static T ParseEnum<T>(string _value)
    {
        return (T)Enum.Parse(typeof(T), _value, true);
    }

    public static void FadeOutSpriteRenderer(SpriteRenderer _spriteRenderer, float _fadeOutTime)
    {
        Managers.Instance.StartCoroutine(FadeOutSpriteRendererRoutine(_spriteRenderer, _fadeOutTime));
    }

    private static IEnumerator FadeOutSpriteRendererRoutine(SpriteRenderer _spriteRenderer, float _fadeOutTime)
    {
        while (_spriteRenderer != null || _spriteRenderer.color.a > 0)
        {
            yield return null;
            _spriteRenderer.color -= new Color(0, 0, 0, _spriteRenderer.color.a - Time.deltaTime);
        }
    }

    public static string IntToSymbolString(int _int)
    {
        string symbolString;

        if (_int > 1000000000000000f) symbolString = $"{Mathf.Floor(_int / 1000000000000000f * 100) / 100}P";

        else if (_int > 1000000000000f) symbolString = $"{Mathf.Floor(_int / 1000000000000 * 100) / 100}T";

        else if (_int > 1000000000f) symbolString = $"{Mathf.Floor(_int / 1000000000f * 100) / 100}G";

        else if (_int > 1000000f) symbolString = $"{Mathf.Floor(_int / 1000000f * 100) / 100}M";

        else if (_int > 1000f) symbolString = $"{Mathf.Floor(_int / 1000f * 100) / 100}K";

        else symbolString = $"{Mathf.Floor(_int)}";


        return symbolString;
    }

    public static string FloatToSymbolString(float _float)
    {
        string symbolString;

        if (_float > 1000000000000000f) symbolString = $"{Mathf.Floor(_float / 1000000000000000f * 100) / 100}P";

        else if (_float > 1000000000000f) symbolString = $"{Mathf.Floor(_float / 1000000000000 * 100) / 100}T";

        else if (_float > 1000000000f) symbolString = $"{Mathf.Floor(_float / 1000000000f * 100) / 100}G";

        else if (_float > 1000000f) symbolString = $"{Mathf.Floor(_float / 1000000f * 100) / 100}M";

        else if (_float > 1000f) symbolString = $"{Mathf.Floor(_float / 1000f * 100) / 100}K";

        else symbolString = $"{Mathf.Floor(_float)}";


        return symbolString;
    }

    public static float CheckPlayerCritical()
    {
        return CheckCritical(Managers.Game.player.currentBallDamage, Managers.Game.player.ballCriticalDamage, Managers.Game.player.ballCriticalPercentage);
    }

    public static float CheckCritical(float _damage, float _criticalCoefficient, int _percentage)
    {
        int randomInt = UnityEngine.Random.Range(1, 101);
        if (randomInt < _percentage)
            _damage = _damage * _criticalCoefficient;
        return _damage;
    }
}