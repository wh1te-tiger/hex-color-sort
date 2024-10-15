using System;
using System.IO;
using UnityEngine;

namespace Scripts
{
    public class SaveLoad
    {
        private const string Extension = ".json";
        private string _rootPath;

        string RootPath
        {
            get
            {
                if (_rootPath != null) return _rootPath;
                
                _rootPath = Application.isEditor ? "Assets/Data/Saves/" : Application.persistentDataPath;
#if UNITY_EDITOR
                if (!Directory.Exists(_rootPath)) Directory.CreateDirectory(_rootPath);
#endif

                return _rootPath;
            }
        }

        private string FullPath(string fileName)
        {
            return Path.Combine(RootPath, fileName) + Extension;
        }

        public LoadInfo<T> Load<T>(string name = null)
        {
            var fileName = name ?? typeof(T).Name;

            try
            {
                var json = File.ReadAllText(FullPath(fileName));
                var result = JsonUtility.FromJson<T>(json);
                return result == null ? LoadInfo<T>.Failed(new NullReferenceException("loaded file is null")) : new LoadInfo<T>(result);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return LoadInfo<T>.Failed(e);
            }
        }

        public void Save(object obj, string name = null)
        {
            if (obj == null) throw new NullReferenceException("save-object is null");

            var fileName = name ?? obj.GetType().Name;

            try
            {
                var json = obj as string ?? JsonUtility.ToJson(obj);
                File.WriteAllText(FullPath(fileName), json);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public bool IsExist<T>(string name = null)
        {
            var fileName = name ?? typeof(T).Name;
            return File.Exists(FullPath(fileName));
        }

        public void Delete<T>(string name = null)
        {
            var fileName = name ?? typeof(T).Name;
            if (File.Exists(FullPath(fileName)))
            {
                File.Delete(FullPath(fileName));
            }
        }
    }
    
    public class LoadInfo<T>
    {
        public LoadResult Result;
        public T Obj;
        public Exception Exception;

        public LoadInfo(T obj)
        {
            Obj = obj;
            Result = LoadResult.Success;
        }

        public LoadInfo()
        {
        }

        public static LoadInfo<T> Failed(Exception exception)
        {
            return new LoadInfo<T> { Result = LoadResult.Failed, Exception = exception };
        }
    }

    public enum LoadResult
    {
        Success,
        Failed
    }
}