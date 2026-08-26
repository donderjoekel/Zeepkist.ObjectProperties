using System;
using System.Globalization;
using System.Linq;
using BepInEx;
using HarmonyLib;
using I2.Loc;
using TMPro;
using TNRD.Zeepkist.ObjectProperties.Patches;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ZeepSDK.LevelEditor;

namespace TNRD.Zeepkist.ObjectProperties
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency("ZeepSDK")]
    public class Plugin : BaseUnityPlugin
    {
        private Harmony _harmony;

        private void Awake()
        {
            _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            _harmony.PatchAll();

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
            
            LEV_InspectorGUICreator_CreateGUI.OnPostfix += HandlePostfix;
        }

        private void OnDestroy()
        {
            LEV_InspectorGUICreator_CreateGUI.OnPostfix -= HandlePostfix;
            
            _harmony?.UnpatchSelf();
            _harmony = null;
        }

        private void HandlePostfix(LEV_InspectorGUICreator guiCreator)
        {
            Transform existingExtra = guiCreator.advancedProperties.transform.Find("ExtraProperties");
            if (existingExtra != null)
                Destroy(existingExtra.gameObject);

            guiCreator.isAdvanced = true;
            guiCreator.bridge.GetBlockProperties().linkScalingAxes = false;
            guiCreator.linkedImage.gameObject.SetActive(false);
            guiCreator.linkedImage2.gameObject.SetActive(false);
            guiCreator.linkedImage3.gameObject.SetActive(false);
            
            RectTransform regularTransform = (RectTransform)guiCreator.regularProperties.transform;
            RectTransform advancedTransform = (RectTransform)guiCreator.advancedProperties.transform;

            Transform template = advancedTransform.Find("Transform Panel/Vector3s/Vector3Panel - Position");

            regularTransform.anchorMax = new Vector2(1, 0.9f);
            regularTransform.anchorMin = new Vector2(0, 0.4f);
            advancedTransform.anchorMax = new Vector2(1, 0.55f);
            
            guiCreator.regularProperties.gameObject.SetActive(true);
            guiCreator.advancedProperties.gameObject.SetActive(true);
            guiCreator.LoadAllTransformValues();

            GameObject extraPropertiesObject = new("ExtraProperties", typeof(RectTransform));
            RectTransform rTransform = extraPropertiesObject.GetComponent<RectTransform>();
            rTransform.SetParent(guiCreator.advancedProperties.transform, false);
            rTransform.anchorMin = new Vector2(0.05f, 0.05f);
            rTransform.anchorMax = new Vector2(0.95f, 0.39f);
            rTransform.pivot = new Vector2(0.5f, 0.5f);
            rTransform.sizeDelta = Vector2.zero;
            rTransform.anchoredPosition = Vector2.zero;

            Transform intangiblePanel = guiCreator.advancedProperties.transform.Find("Intangible Panel");
            RectTransform intangibleTransform = (RectTransform)intangiblePanel.transform;
            intangibleTransform.anchorMin = new Vector2(0.05f, 0.41f);
            intangibleTransform.anchorMax = new Vector2(0.95f, 0.46f);
            intangibleTransform.sizeDelta = Vector2.zero;
            intangibleTransform.anchoredPosition = Vector2.zero;

            VerticalLayoutGroup verticalLayoutGroup = extraPropertiesObject.AddComponent<VerticalLayoutGroup>();
            verticalLayoutGroup.childControlWidth = true;
            verticalLayoutGroup.childForceExpandWidth = true;
            verticalLayoutGroup.childScaleWidth = false;
            verticalLayoutGroup.childControlHeight = false;
            verticalLayoutGroup.childForceExpandHeight = false;
            verticalLayoutGroup.childScaleHeight = false;
            verticalLayoutGroup.childAlignment = TextAnchor.UpperLeft;
            verticalLayoutGroup.spacing = 0;

            BlockProperties blockProperties = guiCreator.GetCentral().selection.list.First();
            
            SetupBooster(blockProperties, template, extraPropertiesObject.transform);
            SetupSlowster(blockProperties, template, extraPropertiesObject.transform);
            SetupFan(blockProperties, template, extraPropertiesObject.transform);
            SetupFeatherForcefield(blockProperties, template, extraPropertiesObject.transform);
            SetupCycleOffset(blockProperties, template, extraPropertiesObject.transform);
            SetupLogicGate(blockProperties, template, extraPropertiesObject.transform);
            SetupMonorail(blockProperties, template, extraPropertiesObject.transform);
            SetupCustomLight(blockProperties, template, extraPropertiesObject.transform);
        }

        private void SetupBooster(BlockProperties blockProperties, Transform template, Transform parent)
        {
            BlockEdit_Booster blockEdit = blockProperties.GetComponentInChildren<BlockEdit_Booster>();
            if (blockEdit == null)
                return;

            InstantiateTemplate(
                template,
                parent,
                I2.Loc.LocalizationManager.GetTranslation(blockEdit.bridge.GetLocalizedText(6)),
                () => blockEdit.properties2.bridge.GetFloatValue(blockEdit.boosterAccelerationNR),
                value =>
                {
                    blockEdit.properties2.bridge.SetFloatValue(blockEdit.boosterAccelerationNR, value);
                    blockEdit.BoosterValueChanged();
                    blockEdit.central.inspector.CreateInspectorUI();
                },
                GetValueChangedEvent(blockEdit.bridge, blockEdit.boosterAccelerationNR));

            InstantiateTemplate(
                template,
                parent,
                I2.Loc.LocalizationManager.GetTranslation(blockEdit.bridge.GetLocalizedText(8)),
                () => blockEdit.properties2.bridge.GetFloatValue(blockEdit.boosterValueNR),
                value =>
                {
                    blockEdit.properties2.bridge.SetFloatValue(blockEdit.boosterValueNR, value);
                    blockEdit.BoosterValueChanged();
                    blockEdit.central.inspector.CreateInspectorUI();
                },
                GetValueChangedEvent(blockEdit.bridge, blockEdit.boosterValueNR));
        }
        
        private void SetupSlowster(BlockProperties blockProperties, Transform template, Transform parent)
        {
            BlockEdit_Slowster blockEdit = blockProperties.GetComponentInChildren<BlockEdit_Slowster>();
            if (blockEdit == null)
                return;

            InstantiateTemplate(
                template,
                parent,
                I2.Loc.LocalizationManager.GetTranslation(blockEdit.bridge.GetLocalizedText(6)),
                () => blockEdit.properties2.bridge.GetFloatValue(blockEdit.boosterAccelerationNR),
                value =>
                {
                    blockEdit.properties2.bridge.SetFloatValue(blockEdit.boosterAccelerationNR, value);
                    blockEdit.BoosterValueChanged();
                    blockEdit.central.inspector.CreateInspectorUI();
                },
                GetValueChangedEvent(blockEdit.bridge, blockEdit.boosterAccelerationNR));

            InstantiateTemplate(
                template,
                parent,
                I2.Loc.LocalizationManager.GetTranslation(blockEdit.bridge.GetLocalizedText(8)),
                () => blockEdit.properties2.bridge.GetFloatValue(blockEdit.boosterValueNR),
                value =>
                {
                    blockEdit.properties2.bridge.SetFloatValue(blockEdit.boosterValueNR, value);
                    blockEdit.BoosterValueChanged();
                    blockEdit.central.inspector.CreateInspectorUI();
                },
                GetValueChangedEvent(blockEdit.bridge, blockEdit.boosterValueNR));
        }
        
        private void SetupFan(BlockProperties blockProperties, Transform template, Transform parent)
        {
            BlockEdit_Fan blockEdit = blockProperties.GetComponentInChildren<BlockEdit_Fan>();
            if (blockEdit == null)
                return;

            InstantiateTemplate(
                template,
                parent,
                I2.Loc.LocalizationManager.GetTranslation(blockEdit.bridge.GetLocalizedText(6)),
                () => blockEdit.properties2.bridge.GetFloatValue(blockEdit.fanRangeNR),
                value =>
                {
                    blockEdit.properties2.bridge.SetFloatValue(blockEdit.fanRangeNR, value);
                    blockEdit.BoosterValueChanged();
                    blockEdit.central.inspector.CreateInspectorUI();
                },
                GetValueChangedEvent(blockEdit.bridge, blockEdit.fanRangeNR));

            InstantiateTemplate(
                template,
                parent,
                I2.Loc.LocalizationManager.GetTranslation(blockEdit.bridge.GetLocalizedText(8)),
                () => blockEdit.properties2.bridge.GetFloatValue(blockEdit.fanForceNR),
                value =>
                {
                    blockEdit.properties2.bridge.SetFloatValue(blockEdit.fanForceNR, value);
                    blockEdit.BoosterValueChanged();
                    blockEdit.central.inspector.CreateInspectorUI();
                },
                GetValueChangedEvent(blockEdit.bridge, blockEdit.fanForceNR));
        }
        
        private void SetupFeatherForcefield(BlockProperties blockProperties, Transform template, Transform parent)
        {
            var blockEdit = blockProperties.GetComponentInChildren<BlockEdit_FeatherForcefield>();
            if (blockEdit == null)
                return;

            InstantiateTemplate(
                template,
                parent,
                I2.Loc.LocalizationManager.GetTranslation(blockEdit.bridge.GetLocalizedText(0)),
                () => blockEdit.properties2.bridge.GetFloatValue(blockEdit.heightValueNR),
                value =>
                {
                    blockEdit.properties2.bridge.SetFloatValue(blockEdit.heightValueNR, value);
                    blockEdit.BoosterValueChanged();
                    blockEdit.central.inspector.CreateInspectorUI();
                },
                GetValueChangedEvent(blockEdit.bridge, blockEdit.heightValueNR));

            InstantiateTemplate(
                template,
                parent,
                I2.Loc.LocalizationManager.GetTranslation(blockEdit.bridge.GetLocalizedText(6)),
                () => blockEdit.properties2.bridge.GetFloatValue(blockEdit.dampingValueNR),
                value =>
                {
                    blockEdit.properties2.bridge.SetFloatValue(blockEdit.dampingValueNR, value);
                    blockEdit.BoosterValueChanged();
                    blockEdit.central.inspector.CreateInspectorUI();
                },
                GetValueChangedEvent(blockEdit.bridge, blockEdit.dampingValueNR));
            
            InstantiateTemplate(
                template,
                parent,
                I2.Loc.LocalizationManager.GetTranslation(blockEdit.bridge.GetLocalizedText(8)),
                () => blockEdit.properties2.bridge.GetFloatValue(blockEdit.velocityValueNR),
                value =>
                {
                    blockEdit.properties2.bridge.SetFloatValue(blockEdit.velocityValueNR, value);
                    blockEdit.BoosterValueChanged();
                    blockEdit.central.inspector.CreateInspectorUI();
                },
                GetValueChangedEvent(blockEdit.bridge, blockEdit.velocityValueNR));
        }
        
        private void SetupCycleOffset(BlockProperties blockProperties, Transform template, Transform parent)
        {
            BlockEdit_CycleOffset blockEdit = blockProperties.GetComponentInChildren<BlockEdit_CycleOffset>();
            if (blockEdit == null)
                return;

            // if we have a fan, don't show this
            if (blockProperties.GetComponentInChildren<BlockEdit_Fan>() != null)
                return;

            InstantiateTemplate(
                template,
                parent,
                I2.Loc.LocalizationManager.GetTranslation(blockEdit.bridge.GetLocalizedText(8)),
                () => blockEdit.properties2.bridge.GetFloatValue(blockEdit.cycleValueNR),
                value =>
                {
                    blockEdit.properties2.bridge.SetFloatValue(blockEdit.cycleValueNR, value);
                    blockEdit.BoosterValueChanged();
                    blockEdit.central.inspector.CreateInspectorUI();
                },
                GetValueChangedEvent(blockEdit.bridge, blockEdit.cycleValueNR));
        }
        
        private void SetupLogicGate(BlockProperties blockProperties, Transform template, Transform parent)
        {
            BlockEdit_LogicGate blockEdit = blockProperties.GetComponentInChildren<BlockEdit_LogicGate>();
            if (blockEdit == null)
                return;
            
            InstantiateTemplate(
                template,
                parent,
                I2.Loc.LocalizationManager.GetTranslation( "ABC_Height1" ),
                () => blockEdit.properties2.bridge.GetFloatValue(blockEdit.NUMBER_height1),
                value =>
                {
                    blockEdit.properties2.bridge.SetFloatValue(blockEdit.NUMBER_height1, value);
                    blockEdit.LogicValueChanged();
                    blockEdit.central.inspector.CreateInspectorUI();
                },
                GetValueChangedEvent(blockEdit.bridge, blockEdit.NUMBER_height1));

            InstantiateTemplate(
                template,
                parent,
                I2.Loc.LocalizationManager.GetTranslation( "ABC_Distance1" ),
                () => blockEdit.properties2.bridge.GetFloatValue(blockEdit.NUMBER_distance1),
                value =>
                {
                    blockEdit.properties2.bridge.SetFloatValue(blockEdit.NUMBER_distance1, value);
                    blockEdit.LogicValueChanged();
                    blockEdit.central.inspector.CreateInspectorUI();
                },
                GetValueChangedEvent(blockEdit.bridge, blockEdit.NUMBER_distance1));

            if (!blockEdit.ballLogicBrain.useTwoInputs) 
                return;
            
            InstantiateTemplate(
                template,
                parent,
                I2.Loc.LocalizationManager.GetTranslation( "ABC_Height2" ),
                () => blockEdit.properties2.bridge.GetFloatValue(blockEdit.NUMBER_height2),
                value =>
                {
                    blockEdit.properties2.bridge.SetFloatValue(blockEdit.NUMBER_height2, value);
                    blockEdit.LogicValueChanged();
                    blockEdit.central.inspector.CreateInspectorUI();
                },
                GetValueChangedEvent(blockEdit.bridge, blockEdit.NUMBER_height2));
            
            InstantiateTemplate(
                    template,
                    parent,
                    I2.Loc.LocalizationManager.GetTranslation( "ABC_Distance2" ),
                    () => blockEdit.properties2.bridge.GetFloatValue(blockEdit.NUMBER_distance2),
                    value =>
                    {
                        blockEdit.properties2.bridge.SetFloatValue(blockEdit.NUMBER_distance2, value);
                        blockEdit.LogicValueChanged();
                        blockEdit.central.inspector.CreateInspectorUI();
                    },
                    GetValueChangedEvent(blockEdit.bridge, blockEdit.NUMBER_distance2));
        }
        
        private void SetupMonorail(BlockProperties blockProperties, Transform template, Transform parent)
        {
            BlockEdit_DynamicColorSelector blockEdit = blockProperties.GetComponentInChildren<BlockEdit_DynamicColorSelector>();
            if (blockEdit == null)
                return;

            InstantiateTemplate(
                template,
                parent,
                I2.Loc.LocalizationManager.GetTranslation( blockEdit.bridge.GetLocalizedText(2)),
                () => blockEdit.properties2.bridge.GetFloatValue(blockEdit.number_monorailSpeed),
                value =>
                {
                    blockEdit.properties2.bridge.SetFloatValue(blockEdit.number_monorailSpeed, value);
                    blockEdit.ColorValueChanged();
                    blockEdit.central.inspector.CreateInspectorUI();
                },
                GetValueChangedEvent(blockEdit.bridge, blockEdit.number_monorailSpeed));
        }
        
        private void SetupCustomLight(BlockProperties blockProperties, Transform template, Transform parent)
        {
            BlockEdit_CustomLight blockEdit = blockProperties.GetComponentInChildren<BlockEdit_CustomLight>();
            if (blockEdit == null)
                return;

            InstantiateTemplate(
                template,
                parent,
                I2.Loc.LocalizationManager.GetTranslation( "ABC_Color"),
                () => blockEdit.properties2.bridge.GetFloatValue(blockEdit.NUMBER_color),
                value =>
                {
                    blockEdit.properties2.bridge.SetFloatValue(blockEdit.NUMBER_color, value);
                    blockEdit.LightValueChanged();
                    blockEdit.central.inspector.CreateInspectorUI();
                },
                GetValueChangedEvent(blockEdit.bridge, blockEdit.NUMBER_color));
            
            InstantiateTemplate(
                template,
                parent,
                I2.Loc.LocalizationManager.GetTranslation( "ABC_Saturation"),
                () => blockEdit.properties2.bridge.GetFloatValue(blockEdit.NUMBER_saturation),
                value =>
                {
                    blockEdit.properties2.bridge.SetFloatValue(blockEdit.NUMBER_saturation, value);
                    blockEdit.LightValueChanged();
                    blockEdit.central.inspector.CreateInspectorUI();
                },
                GetValueChangedEvent(blockEdit.bridge, blockEdit.NUMBER_saturation));
            
            InstantiateTemplate(
                template,
                parent,
                I2.Loc.LocalizationManager.GetTranslation( "ABC_Brightness"),
                () => blockEdit.properties2.bridge.GetFloatValue(blockEdit.NUMBER_brightness),
                value =>
                {
                    blockEdit.properties2.bridge.SetFloatValue(blockEdit.NUMBER_brightness, value);
                    blockEdit.LightValueChanged();
                    blockEdit.central.inspector.CreateInspectorUI();
                },
                GetValueChangedEvent(blockEdit.bridge, blockEdit.NUMBER_brightness));
        }

        // To deal with the ridiculous event setup
        private static UnityEvent GetValueChangedEvent(LEV_InspectorBridge bridge, int which)
        {
            switch (which)
            {
                case 0:
                    return bridge.valueChanged0;
                case 1:
                    return bridge.valueChanged1;
                case 2:
                    return bridge.valueChanged2;
                case 3:
                    return bridge.valueChanged3;
                case 4:
                    return bridge.valueChanged4;
                case 5:
                    return bridge.valueChanged5;
                case 6:
                    return bridge.valueChanged6;
                case 7:
                    return bridge.valueChanged7;
                case 8:
                    return bridge.valueChanged8;
                case 9:
                    return bridge.valueChanged9;
                case 10:
                    return bridge.valueChanged10;
                default:
                    Debug.Log("Trying to add listener to invalid which");
                    return null;
            }
        }

        private void InstantiateTemplate(
            Transform template,
            Transform parent,
            string label,
            Func<float> getValue,
            Action<float> setValue,
            UnityEvent valueChanged)
        {
            RectTransform instance = (RectTransform)Instantiate(template, parent);
            Vector2 size = instance.sizeDelta;
            size.y = 50;
            instance.sizeDelta = size;

            Destroy(instance.Find("Title").GetComponent<Localize>());
            instance.Find("Title").GetComponent<TextMeshProUGUI>().text = label;
            RectTransform titleTransform = instance.Find("Title").GetComponent<RectTransform>();
            titleTransform.anchorMax = new Vector2(1, 1);
            titleTransform.sizeDelta = Vector2.zero;
            titleTransform.anchoredPosition = Vector2.zero;

            RectTransform xTransform = instance.Find("Inputfield Pos X").GetComponent<RectTransform>();
            xTransform.anchorMax = new Vector2(1, 0.65f);
            xTransform.anchoredPosition = Vector2.zero;
            xTransform.sizeDelta = Vector2.zero;
            TMP_InputField inputField = xTransform.GetComponent<TMP_InputField>();
            inputField.onValueChanged.RemoveAllListeners();
            inputField.onEndEdit.RemoveAllListeners();
            inputField.text = getValue().ToString(CultureInfo.InvariantCulture);
            inputField.onSelect.AddListener(_ => LevelEditorApi.BlockKeyboardInput(this));
            inputField.onDeselect.AddListener(_ => LevelEditorApi.UnblockKeyboardInput(this));
            inputField.onEndEdit.AddListener(
                value =>
                {
                    LevelEditorApi.UnblockKeyboardInput(this);
                    if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out float newValue))
                    {
                        setValue?.Invoke(newValue);
                    }
                });

            valueChanged.AddListener(
                () => inputField.SetTextWithoutNotify(getValue().ToString(CultureInfo.InvariantCulture)));

            RectTransform placeholder = xTransform.Find("Text Area/Placeholder").GetComponent<RectTransform>();
            placeholder.anchorMin = Vector2.zero;
            placeholder.anchorMax = new Vector2(1, 0.8f);
            placeholder.sizeDelta = new Vector2(-26, 0);

            RectTransform text = xTransform.Find("Text Area/Text").GetComponent<RectTransform>();
            text.anchorMin = Vector2.zero;
            text.anchorMax = new Vector2(1, 0.8f);
            text.sizeDelta = new Vector2(-26, 0);

            Destroy(instance.Find("Inputfield Pos Y").gameObject);
            Destroy(instance.Find("Inputfield Pos Z").gameObject);
            Destroy(instance.Find("x").gameObject);
            Destroy(instance.Find("y").gameObject);
            Destroy(instance.Find("z").gameObject);
            Destroy(instance.Find("linked").gameObject);
            Destroy(instance.Find("linked (1)").gameObject);
            Destroy(instance.Find("linked (2)").gameObject);
        }
    }
}
