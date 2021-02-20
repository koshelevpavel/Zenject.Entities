using System;
using Unity.Entities;

namespace Zenject.Entities
{
    public static class DiContainerExtensions
    {
        public static ConcreteIdArgConditionCopyNonLazyBinder InitEcs(this DiContainer container)
        {
            return container.Bind(new []{typeof(IInitializable), typeof(IDisposable)})
                .To<SystemAssistant>().AsSingle();
        }
        
        public static InstantiateCallbackConditionCopyNonLazyBinder InitEcs(
            this DiContainer container,
            InitializationTime initializationTime)
        {
            return container.InitEcs().WithArguments(initializationTime);
        }
        
        public static InstantiateCallbackConditionCopyNonLazyBinder InitEcs(
            this DiContainer container,
            World world,
            InitializationTime initializationTime = InitializationTime.Start)
        {
            return container.InitEcs().WithArguments(initializationTime, world);
        }

        public static ConcreteIdArgConditionCopyNonLazyBinder BindSystem<TConcrete>(this DiContainer container)
            where TConcrete : ComponentSystemBase
        {
            return container.Bind<ComponentSystemBase>().To<TConcrete>().AsSingle();
        }
    }
}