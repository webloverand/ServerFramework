using UnityEngine;

namespace ClientFramework.Tool.Singleton
{
    public abstract class MonoSingleton<T> : MonoBehaviour, ISingleton where T : MonoSingleton<T>
    {
        protected static T mInstance = null;

        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = MonoSingletonCreator.CreateMonoSingleton<T>();
                }

                return mInstance;
            }
        }
        //初始化调用
        public virtual void OnSingletonInit()
        {

        }

        public virtual void Dispose()
        {
            if (MonoSingletonCreator.IsUnitTestMode)
            {
                var curTrans = transform;
                do
                {
                    var parent = curTrans.parent;
                    DestroyImmediate(curTrans.gameObject);
                    curTrans = parent;
                } while (curTrans != null);

                mInstance = null;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            mInstance = null;
        }
    }
}
