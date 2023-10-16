using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(TestButtonSystem))]
public sealed class TestButtonSystem : UpdateSystem
{
    private Filter _filter;

    public override void OnAwake()
    {
        _filter = World.Filter
            .With<MUIButtonComponent>()
            .With<MUIRefreshComponent>()
            .Build();
    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (var entity in _filter)
        {
            var referenceButton = entity.GetComponent<MUIElementReferenceComponent>().element;

            if (referenceButton.name != "uiButton2") continue; // name check
            if (referenceButton is Button button)
            {
                button.clicked += () =>
                {
                    var labelEntities = World.Filter.With<MUILabelComponent>().Build();

                    foreach (var labelEntity in labelEntities)
                    {
                        var referenceLabel = labelEntity.GetComponent<MUIElementReferenceComponent>().element;
                        
                        if (referenceLabel.name != "label1") continue;
                        
                        ref var labelText = ref labelEntity.GetComponent<TextComponent>();
                        labelText.value = "Fuck Yea!";
                        labelEntity.AddComponent<MUIRefreshComponent>();
                    }
                };

                entity.RemoveComponent<MUIRefreshComponent>();
            }
        }
    }
}