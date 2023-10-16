using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/MUI/" + nameof(MUIInitPanelSystem) + " 1")]
public sealed class MUIInitPanelSystem : UpdateSystem
{
    private Filter _filter;

    public override void OnAwake()
    {
        _filter = World.Filter
            .With<MUIInitPanelComponent>()
            .Build();
    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (var entity in _filter)
        {
            
            entity.AddComponent<MUIPanelComponent>();
        
            var nameComponent = entity.GetComponent<NameComponent>();
            var panelName = nameComponent.value;
        
            ref var panel = ref entity.GetComponent<MUIPanelComponent>();
        
            var uiDocumentGO = new GameObject(panelName);
            panel.uiDocument = uiDocumentGO.AddComponent<UIDocument>();

            entity.AddComponent<MUIInitPanelAddressableComponent>();
            entity.RemoveComponent<MUIInitPanelComponent>();
            
        }
    }
}