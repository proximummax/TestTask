using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Box
{
    public class BoxesLifetimeScope : LifetimeScope
    {
        [SerializeField] private BoxesScrollerView _boxesScrollerView;
        [SerializeField] private Transform _boxesContainer;
        [SerializeField] private BoxView _boxViewPrefab;
        [SerializeField] private Transform _boxParentAfterDrag;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_boxParentAfterDrag);
            builder.RegisterComponent(_boxesScrollerView);
            builder.Register<BoxesScrollerService>(Lifetime.Singleton);
            builder.Register<BoxesSpawnerService>(Lifetime.Singleton);
            builder.Register<BoxController>(Lifetime.Singleton);
            builder.Register<System.Func<BoxView>>(
                container => () =>
                {
                    return container.Instantiate(_boxViewPrefab, _boxesContainer);
                }, Lifetime.Singleton);
            builder.Register<BoxesPresenter>(Lifetime.Singleton);
        }
    }
}
