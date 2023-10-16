using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/MUI/" + nameof(MUIInitPanelTemplateSystem) + " 3")]
public sealed class MUIInitPanelTemplateSystem : UpdateSystem
{
    private Filter _filter;

    public override void OnAwake()
    {
        _filter = World.Filter
            .With<MUIPanelReadyToBuildComponent>()
            .Build();
    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (var entity in _filter)
        {
            var panelName = entity.GetComponent<NameComponent>().value;
            var key = panelName + "Template";
            
            Addressables.LoadAssetAsync<VisualTreeAsset>(key).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    var template = handle.Result;
                    var panelClone = template.CloneTree();

                    ref var globalUIComponent = ref entity.GetComponent<MUIPanelComponent>();
                    
                    var rootVisualElement = globalUIComponent.uiDocument.rootVisualElement;

                    // VisualElement to the UIDocument's root
                    rootVisualElement.Add(panelClone);

                    entity.AddComponent<MUIPanelReadyToViewComponent>();
                }
                else
                {
                    Debug.LogError($"Failed to load {key} from Addressables.");
                }
            };

            entity.RemoveComponent<MUIPanelReadyToBuildComponent>();
        }
    }
}