using UnityEditor;
using UnityEngine;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace SuperSceneBaker
{
    public static class Utils
    {
        public static string GetMemberName<T>(Expression<Func<T>> memberExpression)
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            return expressionBody.Member.Name;
        }

        public static string[] GetStringArray(string key)
        {
            if (EditorPrefs.HasKey(key))
            {
                var str = EditorPrefs.GetString(key);
                if (!string.IsNullOrEmpty(str))
                    return str.Split("\n"[0]);
            }
            return new string[0];
        }

        public static bool SetStringArray(string key, char separator, params string[] stringArray)
        {
            if (stringArray.Length == 0) return false;
            try
            {
                EditorPrefs.SetString(key, String.Join(separator.ToString(), stringArray));
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool SetStringArray(string key, params string[] stringArray)
        {
            if (!SetStringArray(key, "\n"[0], stringArray))
                return false;
            return true;
        }

        public static T[] GetAssetArray<T>(string key) where T : UnityEngine.Object
        {
            var pathArray = GetStringArray(key);
            if (pathArray == null || pathArray.Length <= 0)
                return null;

            var assetArray = new T[pathArray.Length];
            for (int i = 0; i < pathArray.Length; ++i)
                assetArray[i] = (T)AssetDatabase.LoadAssetAtPath(pathArray[i], typeof(T));

            return assetArray;
        }

        public static bool SetAssetArray(string key, UnityEngine.Object[] assetArray)
        {
            if (assetArray == null || assetArray.Length <= 0)
            {
                EditorPrefs.SetString(key, null);
                return true;
            }

            var pathArray = new string[assetArray.Length];
            for (int i = 0; i < assetArray.Length; ++i)
                pathArray[i] = AssetDatabase.GetAssetPath(assetArray[i]);

            SetStringArray(key, pathArray);
            return true;
        }

        public static bool CleanDuplicates<T>(ref T[] self)
        {
            if (self == null)
                return false;

            var cleaned = new List<T>();
            foreach (var elem in self)
            {
                if (!cleaned.Contains(elem))
                    cleaned.Add(elem);
            }

            if (self.Length != cleaned.Count)
            {
                self = cleaned.ToArray();
                return true;
            }
            return false;
        }

        public static void Add<T>(ref T[] array, T newValue)
        {
            int newLength = array.Length + 1;
            var result = new T[newLength];

            for (int i = 0; i < array.Length; i++)
                result[i] = array[i];

            result[newLength - 1] = newValue;

            array = result;
        }

    }
}
