using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Storage
{
    public class StorageLifetimeScope : LifetimeScope
    {
        [SerializeField] private StorageMonoBehaviour _storage;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IBoxesPersistence, JsonBoxesPersistence>(Lifetime.Singleton);
            builder.Register<StorageBoxesService>(Lifetime.Singleton);
            builder.RegisterComponent(_storage);
        }
    }
}
