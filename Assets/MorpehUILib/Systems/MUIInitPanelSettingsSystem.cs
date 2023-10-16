using System.Threading.Tasks;
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
[CreateAssetMenu(menuName = "ECS/Systems/MUI/" + nameof(MUIInitPanelSettingsSystem) + " 2")]
public sealed class MUIInitPanelSettingsSystem : UpdateSystem
{
    private Filter _filter;

    public override void OnAwake()
    {
        _filter = World.Filter
            .With<MUIInitPanelAddressableComponent>()
            .Build();
    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (var entity in _filter)
        {
            var currentPanelName = entity.GetComponent<NameComponent>().value;
            var currentUIDocument = entity.GetComponent<MUIPanelComponent>().uiDocument;

            LoadPanelSettings(entity, currentPanelName, currentUIDocument, fallback: true);

            entity.RemoveComponent<MUIInitPanelAddressableComponent>();
        }
    }

    private async void LoadPanelSettings(Entity entity, string panelName, UIDocument currentUIDocument, bool fallback)
    {
        if (await KeyExists(panelName))
        {
            Addressables.LoadAssetAsync<PanelSettings>(panelName).Completed += handle =>
            {
                Debug.Log($"LoadAssetAsync completed with status: {handle.Status}");
                
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    ApplyPanelSetting(entity, handle.Result, currentUIDocument);
                }
                else if (fallback)
                {
                    Debug.LogWarning($"Failed to load {panelName} from Addressables. Attempting to load default PanelSettings.");
                    LoadPanelSettings(entity, "PanelSettings", currentUIDocument, fallback: false);
                }
                else
                {
                    Debug.LogError($"Failed to load default PanelSettings from Addressables.");
                }
            };
        }
        else if (fallback)
        {
            Debug.LogWarning($"Key {panelName} does not exist. Attempting to load default PanelSettings.");
            LoadPanelSettings(entity, "PanelSettings", currentUIDocument, fallback: false);
        }
        else
        {
            Debug.LogError($"Failed to load default PanelSettings from Addressables and the key {panelName} does not exist.");
        }
    }

    private void ApplyPanelSetting(Entity entity, PanelSettings setting, UIDocument currentUIDocument)
    {
        currentUIDocument.panelSettings = setting;

        ref var globalUIComponent = ref entity.GetComponent<MUIPanelComponent>();
        globalUIComponent.uiDocument = currentUIDocument;

        entity.AddComponent<MUIPanelReadyToBuildComponent>();
    }
    
    private async Task<bool> KeyExists(string key)
    {
        var locations = await Addressables.LoadResourceLocationsAsync(key).Task;
        var keyExists = locations is {Count: > 0};
        Debug.Log($"Key {key} exists: {keyExists}");
        return keyExists;
    }
}
