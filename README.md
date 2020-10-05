
# Jedi
Just another IoC container framework specifically focusing on Async support.

# Development Schedule
| Date | Status | Comment
|--|--|--|
| 6/9/2020 | Early Alpha | Unit Tests Added. Packages coming soon.
| 6/2/2020 | Early Alpha | Source Only. DLL/NUGET to be added soon.

# Key Features
 - Asynchronous/Synchronous Injection
	 - Constructor
	 - Field
	 - Property
	 - Method
 - Asynchronous/Synchronous Resolution
 - Subcontainers/Container Extensions/Nested Containers
 - Generic/Non Generic binding
 - Prewarmed type registry
 - Jedi Inject attribute support
 - Fully threadsafe
 - Easy to use API with chained method support.
	 - container.Bind\<Foo>().WithInstance(bar).AsSingle()
	 - container.Bind\<Foo>().To\<Bar>().AsTransient()
 - Fully unit tested

# Motivation
The lack of an Asynchronous Depdendency Injection /IOC framework with support for Asynchronous Injection and Resolution support.
> This framework was heavily inspired by **[Extenject]**(https://github.com/svermeulen/Extenject)

# API
### Container Creation
1. Simple
```csharp
DiContainer container = new DiContainer();
```
2. With Extenstions
```csharp
DiContainer parentContainer1 = new DiContainer();
DiContainer parentContainer2 = new DiContainer();
DiContainer parentContainer3 = new DiContainer();

// Option 1
DiContainer childContainer = new DiContainer(parentContainer1, parentContainer2, parentContainer3);

// Option 2
DiContainer childContainer = new DiContainer();
childContainer.AddExtensions(new DiContainer[]{ parentContainer1, parentContainer2, parentContainer3 });
```
3. Sub Container
```csharp
DiContainer parentContainer1 = new DiContainer();

DiContainer childContainer = parentContainer.CreateSubContainer();
```

### Binding
 1. Bind()
```csharp
container.Bind<Foo>();
container.Bind(typeof(Foo));
```
 2. To()
```csharp
container.Bind<Foo>().To<Bar>();
container.Bind(typeof(Foo)).To(typeof(Bar));
```
 3. WithId()
```csharp
container.Bind<Foo>().WithId("binding_id");
```
 4. AndInterfaces()
```csharp
class Foo : IFoo
{
}
container.Bind<Foo>().WithInterfaces();
container.Resolve<IFoo>();
```
 5. WithInstance()
```csharp
Foo foo = new Foo();
container.Bind<Foo>().WithInstance(foo);
```
 6. AsTransient()
```csharp
class Foo
{
	public string Id = Guid.NewGuid().ToString();
}

container.Bind<Foo>().AsTransient();

// Resolve creates a new instance every time
container.Resolve<Foo>().Id;
// 12345
container.Resolve<Foo>().Id;
// 54321
container.Resolve<Foo>().Id;
// 09876
```
 7. AsSingle()
```csharp
class Foo
{
	public string Id = Guid.NewGuid().ToString();
}

// Foo instance is created when bound.
container.Bind<Foo>().AsSingle();

// Resolve gives the same instance all the time
container.Resolve<Foo>().Id;
// 12345
container.Resolve<Foo>().Id;
// 12345
container.Resolve<Foo>().Id;
// 12345
```
 8. AsSingleAsync()
```csharp
class Foo
{
	public string Id = Guid.NewGuid().ToString();
}

// Foo instance is created asynchronously when bound.
container.Bind<Foo>().AsSingleAsync();

// Asynchronous resolve gives the same instance all the time
container.Resolve<Foo>().Id;
// 12345
container.Resolve<Foo>().Id;
// 12345
container.Resolve<Foo>().Id;
// 12345
```
 9. AsSingleLazy()
```csharp
class Foo
{
	public string Id = Guid.NewGuid().ToString();
}

// Foo instance created only when resolved
container.Bind<Foo>().AsSingleLazy();

// Asynchronous resolve gives the same instance all the time
await container.ResolveAsync<Foo>().Id;
// 12345
await container.ResolveAsync<Foo>().Id;
// 12345
await container.ResolveAsync<Foo>().Id;
// 12345
```
 10. FromMethod()
```csharp
public Foo GetFooInstance()
{
	return new Foo();
}

// Method used to resolve foo instance
container.Bind<Foo>().FromMethod(GetFooInstance);
```
 11. FromMethodAsync()
```csharp
public async Task<Foo> GetFooInstanceAsync()
{
	return await _database.GetFoo();
}

// Method used to resolve foo instance
container.Bind<Foo>().FromMethodAsync(GetFooInstanceAsync);
```
 12. OnInstantiated()
```csharp
// Called when Foo is resolved
public void OnFooInstantiated(Foo instance)
{
	// Do something with instance
}

// Method used to resolve foo instance
container.Bind<Foo>().OnInstantiated(OnFooInstantiated);
``` 
 13. OnInstantiatedAsync()
```csharp
// Called when Foo is resolved
public Task OnFooInstantiatedAsync(Foo instance)
{
	// Do something asynchronous
}

// Method used to resolve foo instance
container.Bind<Foo>().OnInstantiatedAsync(OnFooInstantiatedAsync);
```
  
### Injection
1. Inject()
```csharp
container.Inject(injectable);
```
2. InjectAsync()
```csharp
await container.InjectAsync(injectable);
```

### Resolve
1. Resolve()
```csharp
// Option 1
container.Resolve<Foo>();

// Option 2
container.Resolve(typeof(Foo));
```
2. ResolveAsync()
```csharp
// Option 1
await container.ResolveAsync<Foo>();

// Option 2
await container.ResolveAsync(typeof(Foo));
```

# Benchmarks
| Test (1 000 000) | Infuse | Zenject
|--|--|--|
| Resolves | 982 MS, 9829598 Ticks | 2463 MS, 24639955 Ticks
| Injects - No Inheritance | 1613 MS, 16135145 Ticks | 5113 MS, 51137168 Ticks
| Injects - With Inheritance | 1992 MS, 19922531 Ticks | 5061 MS, 50619097 Ticks
| Inject Lock Contention | 0 MS | 514 MS	
| Resolve Lock Contention | 0 MS | 0 MS
