using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleStart.Migrations
{
    public class MyContextConfiguration : DbConfiguration
    {
        public MyContextConfiguration()
        {
            MyDbModelStore cachedDbModelStore = new MyDbModelStore(MyContext.EfCacheDirPath);
            IDbDependencyResolver dependencyResolver = new SingletonDependencyResolver<DbModelStore>(cachedDbModelStore);
            AddDependencyResolver(dependencyResolver);
        }

        private class MyDbModelStore : DefaultDbModelStore
        {
            public MyDbModelStore(string location)
                : base(location)
            { }

            public override DbCompiledModel TryLoad(Type contextType)
            {
                string path = GetFilePath(contextType);
                if (File.Exists(path))
                {
                    DateTime lastWriteTime = File.GetLastWriteTimeUtc(path);
                    DateTime lastWriteTimeDomainAssembly = File.GetLastWriteTimeUtc(typeof(MyDomain.SomeTypeInOurDomain).Assembly.Location);
                    if (lastWriteTimeDomainAssembly > lastWriteTime)
                    {
                        File.Delete(path);
                        Tracers.EntityFramework.TraceInformation("Cached db model obsolete. Re-creating cached db model edmx.");
                    }
                }
                else
                {
                    Tracers.EntityFramework.TraceInformation("No cached db model found. Creating cached db model edmx.");
                }

                return base.TryLoad(contextType);
            }
        }
    }
}