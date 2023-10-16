using System.Reflection.Emit;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using Label = UnityEngine.UIElements.Label;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(UILabelSystem))]
public sealed class UILabelSystem : UpdateSystem
{
    private Filter _filter;

    public override void OnAwake()
    {
        _filter = World.Filter
            .With<MUILabelComponent>()
            .With<MUIRefreshComponent>()
            .Build();
    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (var entity in _filter)
        {
            
            var referenceLabel = entity.GetComponent<MUIElementReferenceComponent>().element;

            if (referenceLabel.name != "label1") continue; // fast name check
            if (referenceLabel is Label label)
            {

                ref var labelText = ref entity.GetComponent<TextComponent>();
                label.text = labelText.value;
            }
            
            entity.RemoveComponent<MUIRefreshComponent>();
        }
    }
}