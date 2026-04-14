using VContainer;
using VContainer.Unity;

namespace Game.Scripts.DropZone
{
    public class DropZoneLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<TowerZoneService>(Lifetime.Singleton);
            builder.Register<DropZoneInteractionService>(Lifetime.Singleton);
        }
    }
}
