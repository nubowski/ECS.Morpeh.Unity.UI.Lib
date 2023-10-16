using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/MUI/" + nameof(MUIInitPanelElementsSystem) + " 4")]
public sealed class MUIInitPanelElementsSystem : UpdateSystem
{
    private Filter _filter;

    public override void OnAwake()
    {
        _filter = World.Filter
            .With<MUIPanelReadyToViewComponent>()
            .Build();
    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (var entity in _filter)
        {
            entity.RemoveComponent<MUIPanelReadyToViewComponent>();
            
            ref var globalUIComponent = ref entity.GetComponent<MUIPanelComponent>();
            var uiDocument = globalUIComponent.uiDocument;
        
            var rootVisualElement = uiDocument.rootVisualElement;
            
            IterateAndCreateEntitiesForElements(rootVisualElement);

            
        }
    }

    private void IterateAndCreateEntitiesForElements(VisualElement parentElement)
    {
        foreach (var childElement in parentElement.Children())
        {
            
            var elementEntity = World.CreateEntity();
        
            // Add and set a name
            elementEntity.AddComponent<NameComponent>();
            ref var elementName = ref elementEntity.GetComponent<NameComponent>();
            elementName.value = childElement.name;
        
            // Add and set a ref
            elementEntity.AddComponent<MUIElementReferenceComponent>();
            ref var elementReference = ref elementEntity.GetComponent<MUIElementReferenceComponent>();
            elementReference.element = childElement;
            
            elementEntity.AddComponent<MUIElementForMappingComponent>();
            
        
            // Recursive call (for child's child's child's children <3)
            IterateAndCreateEntitiesForElements(childElement);
        }
    }
}