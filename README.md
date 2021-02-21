# Zenject.Entities
Dependency injection ([Zenject](https://github.com/modesttree/zenject)) into your ECS  ([Unity Entities](https://docs.unity3d.com/Packages/com.unity.entities@0.17/manual/index.html)) Unity projects.
Verified for Unity 2020.1

  ## Installation

- [Intall Zenject](https://github.com/modesttree/Zenject#installation)
- [Install Entity Component System](https://docs.unity3d.com/Packages/com.unity.entities@0.17/manual/install_setup.html)
- [Install this package](https://docs.unity3d.com/Manual/upm-ui-giturl.html)
## Getting Started

```csharp
Container.InitEcs(); // Called in every context where you will bind systems
Container.BindSystem<SomeSystem>();
```

```csharp
Container.Bind<ComponentSystemBase>().To<SomeSystem>().AsSingle(); //equels Container.BindSystem<SomeSystem>()
Container.Bind<ComponentSystemBase>().FromInstance(new SceneSystem2()).AsSingle();
```
## World
```csharp
World.DefaultGameObjectInjectionWorld //default world
```
Another world can be used
```csharp
Container.Bind<World>().To<World>().AsSingle().WithArguments("AnotherWorld", WorldFlags.Game)  
    .OnInstantiated((context, world) => { /*init world*/ }); // world for this and all children contexts
```
```csharp
Container.InitEcs().WithArguments(anotherWorldInstance); // world for this contexts
```
## InitializationTime
```csharp
Container.InitEcs().WithArguments(InitializationTime.Start); // (default) systems will be created on context.Start()
```
```csharp
Container.InitEcs().WithArguments(InitializationTime.Awake); // (default) systems will be created on context.Awake()
