using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/MUI/" + nameof(MUIPanelElementsMappingSystem) + " 5")]
public sealed class MUIPanelElementsMappingSystem : UpdateSystem
{
    private Filter _filter;

    public override void OnAwake()
    {
        _filter = World.Filter
            .With<MUIElementForMappingComponent>()
            .Build();
    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (var entity in _filter)
        {
            var elementType = entity.GetComponent<MUIElementReferenceComponent>().element.GetType();
            
            if (_elementToComponentMap.TryGetValue(elementType, out var action))
            {
                action.Invoke(entity);
            }
            else
            {
                Debug.LogWarning($"Map does not have an action for {elementType} element type. No components were added");
            }

            entity.RemoveComponent<MUIElementForMappingComponent>();
        }
    }
    
    private readonly Dictionary<Type, Action<Entity>> _elementToComponentMap = new()
    {
        {
            typeof(Button), (entity) =>
            {
                entity.AddComponent<MUIButtonComponent>();
                entity.AddComponent<TextComponent>();
                entity.AddComponent<MUIRefreshComponent>();
            }
        },
        {
            typeof(Label), (entity) =>
            {
                entity.AddComponent<MUILabelComponent>();
                entity.AddComponent<TextComponent>();
            }
        },
        
        // TODO: ... more mappings as needed (~60)
        // TODO: dispose/destroy entities, like containers/roots/so on [ELSE]???
    };
}