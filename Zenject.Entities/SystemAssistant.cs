using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Zenject.Entities
{
    public class SystemAssistant : IInitializable, IDisposable
    {
        private readonly World _world;
        private readonly List<ComponentSystemBase> _systems;
        private readonly SimulationSystemGroup _simulationSystemGroup;
        private bool _isInitialized;

        public SystemAssistant(
            List<ComponentSystemBase> systems,
            World world = null,
            InitializationTime initializationTime = InitializationTime.Start)
        {
            _world = world ?? World.DefaultGameObjectInjectionWorld;
            _systems = systems;
            _simulationSystemGroup =  _world.GetOrCreateSystem<SimulationSystemGroup>();
            if (initializationTime == InitializationTime.Awake)
            {
                Initialize();
            }
        }
        
        public void Initialize()
        {
            if(_isInitialized) return;

            foreach (ComponentSystemBase system in _systems)
            {
                _world.AddSystem(system);
                foreach (ComponentSystemGroup systemGroup in GetSystemGroup(system))
                {
                    systemGroup.AddSystemToUpdateList(system);
                }
            }
            _simulationSystemGroup.SortSystems();

            _isInitialized = true;
        }

        public void Dispose()
        {
            foreach (ComponentSystemBase system in _systems)
            {
                foreach (ComponentSystemGroup systemGroup in GetSystemGroup(system))
                {
                    systemGroup.RemoveSystemFromUpdateList(system);
                }
                _world.DestroySystem(system);
            }
        }

        private IEnumerable<ComponentSystemGroup> GetSystemGroup(ComponentSystemBase system)
        {
            var updateInGroupAttributes = TypeManager.GetSystemAttributes(
                system.GetType(),
                typeof(UpdateInGroupAttribute));

            if (updateInGroupAttributes.Length == 0)
            {
                yield return _simulationSystemGroup;
            }
            else
            {
                foreach (var attr in updateInGroupAttributes)
                {
                    if (attr is UpdateInGroupAttribute uga)
                    {
                        if (_world.GetExistingSystem(uga.GroupType) is ComponentSystemGroup systemGroup)
                        {
                            yield return systemGroup;
                        }
                    }
                }
            }
        }
    }
}