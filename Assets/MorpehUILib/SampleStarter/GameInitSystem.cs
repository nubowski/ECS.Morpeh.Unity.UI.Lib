using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Initializers/" + nameof(GameInitSystem))]
public sealed class GameInitSystem : Initializer
{
    public override void OnAwake()
    {
        var uiEntity = World.CreateEntity();
        uiEntity.AddComponent<MUIInitComponent>();
    }

    public override void Dispose()
    {

    }
}