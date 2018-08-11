using System;
using System.Reflection;
using UnityEngine;

namespace ClientFramework.Tool.Singleton
{
    public static class MonoSingletonCreator
    {
        public static bool IsUnitTestMode { get; set; }

        public static T CreateMonoSingleton<T>() where T : MonoBehaviour, ISingleton
        {
            T instance = null;

            if (!IsUnitTestMode && !Application.isPlaying) return instance;
            instance = UnityEngine.Object.FindObjectOfType<T>(); //搜索当前场景有没有实例

            if (instance != null) return instance;
            MemberInfo info = typeof(T);
            //获取自定义的属性 
            var attributes = info.GetCustomAttributes(true);
            foreach (var atribute in attributes)
            {
                //转化成MonoSingletonPath类
                var defineAttri = atribute as MonoSingletonPath;
                if (defineAttri == null)
                {
                    continue;
                }
                //defineAttri.PathInHierarchy获取路径,根据路径创建组件
                instance = CreateComponentOnGameObject<T>(defineAttri.PathInHierarchy, true);
                break;
            }
            //如果没有根据路径创建
            if (instance == null)
            {
                var obj = new GameObject(typeof(T).Name); //用类型的名字创建物体
                if (!IsUnitTestMode)
                    UnityEngine.Object.DontDestroyOnLoad(obj);
                instance = obj.AddComponent<T>(); //在该名字上添加组件
            }

            instance.OnSingletonInit();

            return instance;
        }
        //根据路径寻找/创建，添加脚本组件
        private static T CreateComponentOnGameObject<T>(string path, bool dontDestroy) where T : MonoBehaviour
        {
            var obj = FindGameObject(path, true, dontDestroy);
            if (obj == null)
            {
                obj = new GameObject("Singleton of " + typeof(T).Name);
                if (dontDestroy && !IsUnitTestMode)
                {
                    UnityEngine.Object.DontDestroyOnLoad(obj);
                }
            }

            return obj.AddComponent<T>();
        }
        //第二个参数是没有找到就创建此路径
        private static GameObject FindGameObject(string path, bool build, bool dontDestroy)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            var subPath = path.Split('/');
            if (subPath == null || subPath.Length == 0)
            {
                return null;
            }

            return FindGameObject(null, subPath, 0, build, dontDestroy);
        }

        private static GameObject FindGameObject(GameObject root, string[] subPath, int index, bool build, bool dontDestroy)
        {
            GameObject client = null;

            if (root == null)
            {
                client = GameObject.Find(subPath[index]);
            }
            else
            {
                var child = root.transform.Find(subPath[index]);
                if (child != null)
                {
                    client = child.gameObject;
                }
            }

            if (client == null) //没有找到此路径则创建路径
            {
                if (build)
                {
                    client = new GameObject(subPath[index]);
                    if (root != null)
                    {
                        client.transform.SetParent(root.transform);
                    }

                    if (dontDestroy && index == 0 && !IsUnitTestMode)
                    {
                        GameObject.DontDestroyOnLoad(client);
                    }
                }
            }

            if (client == null)
            {
                return null;
            }

            return ++index == subPath.Length ? client : FindGameObject(client, subPath, index, build, dontDestroy);
        }
    }
}
