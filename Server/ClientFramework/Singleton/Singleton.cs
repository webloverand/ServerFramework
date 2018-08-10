namespace ClientFramework.Tool.Singleton
{
    public abstract class Singleton<T> : ISingleton where T : Singleton<T>
    {
        protected static T mInstance;

        static object mLock = new object();

        protected Singleton()
        {
        }

        public static T Instance
        {
            get
            {
                lock (mLock)
                {
                    if (mInstance == null)
                    {
                        mInstance = SingletonCreator.CreateSingleton<T>();
                    }
                }

                return mInstance;
            }
        }
        //释放
        public virtual void Dispose()
        {
            mInstance = null;
        }

        public virtual void OnSingletonInit()
        {
        }
    }
}
