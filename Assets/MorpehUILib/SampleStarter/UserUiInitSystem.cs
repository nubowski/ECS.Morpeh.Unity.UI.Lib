using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(UserUiInitSystem))]
public sealed class UserUiInitSystem : UpdateSystem
{
    private Filter _filter;

    public override void OnAwake()
    {
        _filter = World.Filter
            .With<MUIInitComponent>()
            .Build();
    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (var entity in _filter)
        {
            entity.RemoveComponent<MUIInitComponent>();

            InitPanel("TestUI").AddComponent<TestUITag>();
        }
    }

    private Entity InitPanel(string panelName)
    {
        var panel = World.CreateEntity();
        panel.AddComponent<NameComponent>();
        ref var panelNameComponent = ref panel.GetComponent<NameComponent>();
        panelNameComponent.value = panelName;
        
        panel.AddComponent<MUIInitPanelComponent>();
        
        return panel;
    }
}