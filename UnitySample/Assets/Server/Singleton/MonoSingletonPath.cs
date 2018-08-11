using System;

namespace ClientFramework.Tool.Singleton
{
    //继承自MonoSingletonPath,是自定义属性
    public class QMonoSingletonPath : MonoSingletonPath
    {
        public QMonoSingletonPath(string pathInHierarchy) : base(pathInHierarchy) //给父类的mPathInHierarchy复制
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class)] //限制使用在Class前面
    public class MonoSingletonPath : Attribute
    {
        private string mPathInHierarchy;

        public MonoSingletonPath(string pathInHierarchy)
        {
            mPathInHierarchy = pathInHierarchy;
        }

        public string PathInHierarchy
        {
            get { return mPathInHierarchy; }
        }
    }
}
