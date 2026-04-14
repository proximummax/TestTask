using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Storage
{
    public class StorageLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IBoxesPersistence, JsonBoxesPersistence>(Lifetime.Singleton);
            builder.RegisterEntryPoint<StorageBoxesService>();
        }
    }
}
