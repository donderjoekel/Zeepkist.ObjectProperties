// using System;
// using System.Collections.Generic;
// using System.Linq;
// using BepInEx.Logging;
// using TNRD.Zeepkist.ObjectProperties.UI;
// using UnityEngine;
// using ZeepSDK.LevelEditor;
// using ZeepSDK.UI;
//
// namespace TNRD.Zeepkist.ObjectProperties;
//
// internal class Drawer : MonoBehaviour
// {
//     private readonly ManualLogSource logger = Plugin.CreateLogger(nameof(Drawer));
//     
//     private LEV_Inspector inspector;
//     private LEV_LevelEditorCentral central;
//
//     private GameObject canvas;
//     private ObjectPropertiesWindow window;
//
//     private bool wasMouseOver;
//
//     private BlockProperties properties;
//     private BlockEdit_Booster blockEditBooster;
//     private BlockEdit_Fan blockEditFan;
//     private BlockEdit_Slowster blockEditSlowster;
//     private BlockEdit_CycleOffset blockEditCycleOffset;
//     private BlockEdit_FeatherForcefield blockEditFeatherForceField;
//
//     public void Initialize(LEV_LevelEditorCentral central)
//     {
//         this.central = central;
//         this.central.selection.ThingsJustGotSelected.AddListener(OnSelected);
//         this.central.selection.ThingsJustGotDeselected.AddListener(OnDeselected);
//         this.inspector = central.inspector;
//         
//         Plugin.ConfigWindowVisible.SettingChanged += ToggleVisibility;
//     }
//
//     private void OnDestroy()
//     {
//         Plugin.ConfigWindowVisible.SettingChanged -= ToggleVisibility;
//         
//         central.selection.ThingsJustGotSelected.RemoveListener(OnSelected);
//         central.selection.ThingsJustGotDeselected.RemoveListener(OnDeselected);
//     }
//     
//     private void ToggleVisibility(object sender, EventArgs e)
//     {
//         if (canvas == null)
//             return;
//
//         canvas.gameObject.SetActive(Plugin.ConfigWindowVisible.Value);
//     }
//
//     private void Update()
//     {
//         bool flag = IsMouseOver();
//
//         if (flag && !wasMouseOver)
//             LevelEditorApi.BlockMouseInput(this);
//         else if (!flag && wasMouseOver)
//             LevelEditorApi.UnblockMouseInput(this);
//
//         wasMouseOver = flag;
//
//         if (properties == null)
//             return;
//
//         Transform propertiesTransform = properties.transform;
//         window.PositionInput.Value = propertiesTransform.position;
//         window.RotationInput.Value = propertiesTransform.rotation.eulerAngles;
//         window.ScaleInput.Value = propertiesTransform.localScale;
//
//         if (blockEditBooster != null)
//         {
//             window.BoosterAccelerationInput.Value =
//                 blockEditBooster.bridge.GetFloatValue(blockEditBooster.boosterAccelerationNR);
//             window.BoosterValueInput.Value = blockEditBooster.bridge.GetFloatValue(blockEditBooster.boosterValueNR);
//         }
//
//         if (blockEditFan != null)
//         {
//             window.FanForceInput.Value = blockEditFan.bridge.GetFloatValue(blockEditFan.fanForceNR);
//             window.FanRangeInput.Value = blockEditFan.bridge.GetFloatValue(blockEditFan.fanRangeNR);
//         }
//
//         if (blockEditSlowster != null)
//         {
//             window.SlowsterAccelerationInput.Value =
//                 blockEditSlowster.bridge.GetFloatValue(blockEditSlowster.boosterAccelerationNR);
//             window.SlowsterValueInput.Value = blockEditSlowster.bridge.GetFloatValue(blockEditSlowster.boosterValueNR);
//         }
//
//         if (blockEditCycleOffset != null)
//         {
//             window.CycleValueInput.Value = blockEditCycleOffset.bridge.GetFloatValue(blockEditCycleOffset.cycleValueNR);
//         }
//
//         if (blockEditFeatherForceField != null)
//         {
//             window.FeatherDampingInput.Value =
//                 blockEditFeatherForceField.bridge.GetFloatValue(blockEditFeatherForceField.velocityValueNR);
//             window.FeatherHeightInput.Value =
//                 blockEditFeatherForceField.bridge.GetFloatValue(blockEditFeatherForceField.heightValueNR);
//             window.FeatherVelocityInput.Value =
//                 blockEditFeatherForceField.bridge.GetFloatValue(blockEditFeatherForceField.dampingValueNR);
//         }
//     }
//
//     private void OnSelected()
//     {
//         if (central.selection.list.Count > 1)
//         {
//             RemoveUI();
//         }
//         else
//         {
//             properties = central.selection.list.First();
//
//             CreateUI();
//             UpdateLabels();
//
//             window.ShowTransformInputs(true);
//
//             blockEditBooster = GetBlockEdit<BlockEdit_Booster>();
//             window.ShowBoosterInputs(blockEditBooster != null);
//
//             blockEditFan = GetBlockEdit<BlockEdit_Fan>();
//             window.ShowFanInputs(blockEditFan != null);
//
//             blockEditSlowster = GetBlockEdit<BlockEdit_Slowster>();
//             window.ShowSlowsterInputs(blockEditSlowster != null);
//
//             blockEditCycleOffset = GetBlockEdit<BlockEdit_CycleOffset>();
//             window.ShowCycleInputs(blockEditCycleOffset != null && blockEditFan == null);
//
//             blockEditFeatherForceField = GetBlockEdit<BlockEdit_FeatherForcefield>();
//             window.ShowFeatherInputs(blockEditFeatherForceField != null);
//         }
//     }
//
//     private T GetBlockEdit<T>()
//         where T : BlockEdit
//     {
//         return (T)properties.propertyScripts.FirstOrDefault(x => x is T);
//     }
//
//     private void OnDeselected() => RemoveUI();
//
//     private void CreateUI()
//     {
//         if (canvas != null)
//             Destroy(canvas);
//
//         GameObject canvasPrefab = Plugin.AssetBundle.LoadAsset<GameObject>("ObjectPropertiesCanvas");
//         canvas = Instantiate(canvasPrefab);
//         canvas.gameObject.SetActive(Plugin.ConfigWindowVisible.Value);
//         window = canvas.GetComponentInChildren<ObjectPropertiesWindow>();
//         UIApi.AddToConfigurator(window.GetComponent<RectTransform>());
//
//         RegisterChangeEvents();
//         RegisterFocusEvents();
//     }
//
//     private void RegisterChangeEvents()
//     {
//         window.PositionInput.ValueChanged += PositionValueChanged;
//         window.RotationInput.ValueChanged += RotationValueChanged;
//         window.ScaleInput.ValueChanged += ScaleValueChanged;
//         window.BoosterAccelerationInput.ValueChanged += BoosterAccelerationValueChanged;
//         window.BoosterValueInput.ValueChanged += BoosterValueValueChanged;
//
//         window.SlowsterAccelerationInput.ValueChanged += SlowsterAccelerationValueChanged;
//         window.SlowsterValueInput.ValueChanged += SlowsterValueValueChanged;
//
//         window.FanForceInput.ValueChanged += FanForceValueChanged;
//         window.FanRangeInput.ValueChanged += FanRangeValueChanged;
//
//         window.CycleValueInput.ValueChanged += CycleValueValueChanged;
//
//         window.FeatherDampingInput.ValueChanged += FeatherDampingValueChanged;
//         window.FeatherHeightInput.ValueChanged += FeatherHeightValueChanged;
//         window.FeatherVelocityInput.ValueChanged += FeatherVelocityValueChanged;
//     }
//
//     private void RegisterFocusEvents()
//     {
//         RegisterFocusEvents(window.PositionInput);
//         RegisterFocusEvents(window.RotationInput);
//         RegisterFocusEvents(window.ScaleInput);
//         RegisterFocusEvents(window.BoosterAccelerationInput);
//         RegisterFocusEvents(window.BoosterValueInput);
//         RegisterFocusEvents(window.SlowsterAccelerationInput);
//         RegisterFocusEvents(window.SlowsterValueInput);
//         RegisterFocusEvents(window.FanForceInput);
//         RegisterFocusEvents(window.FanRangeInput);
//         RegisterFocusEvents(window.CycleValueInput);
//         RegisterFocusEvents(window.FeatherDampingInput);
//         RegisterFocusEvents(window.FeatherHeightInput);
//         RegisterFocusEvents(window.FeatherVelocityInput);
//     }
//
//     private void RegisterFocusEvents(Vector3Input vector3Input)
//     {
//         vector3Input.GainedFocus += _ => LevelEditorApi.BlockKeyboardInput(this);
//         vector3Input.LostFocus += _ => LevelEditorApi.UnblockKeyboardInput(this);
//     }
//
//     private void RegisterFocusEvents(FloatInput floatInput)
//     {
//         floatInput.GainedFocus += _ => LevelEditorApi.BlockKeyboardInput(this);
//         floatInput.LostFocus += _ => LevelEditorApi.UnblockKeyboardInput(this);
//     }
//
//     private void UpdateLabels()
//     {
//         window.BoosterAccelerationInput.Label = GetTranslation(6);
//         window.BoosterValueInput.Label = GetTranslation(8);
//         window.SlowsterAccelerationInput.Label = GetTranslation(6);
//         window.SlowsterValueInput.Label = GetTranslation(8);
//         window.FanForceInput.Label = GetTranslation(8);
//         window.FanRangeInput.Label = GetTranslation(6);
//         window.CycleValueInput.Label = GetTranslation(8);
//         window.FeatherDampingInput.Label = GetTranslation(8);
//         window.FeatherHeightInput.Label = GetTranslation(0);
//         window.FeatherVelocityInput.Label = GetTranslation(6);
//     }
//
//     private string GetTranslation(int index)
//     {
//         return I2.Loc.LocalizationManager.GetTranslation(properties.bridge.GetLocalizedText(index));
//     }
//
//     private void PositionValueChanged(Vector3 value)
//     {
//         List<string> before = central.undoRedo.ConvertBlockListToJSONList(central.selection.list);
//
//         properties.gameObject.transform.position = value;
//
//         List<string> after = central.undoRedo.ConvertBlockListToJSONList(central.selection.list);
//         List<string> selection = central.undoRedo.ConvertSelectionToStringList(central.selection.list);
//         central.validation.BreakLock(
//             central.undoRedo.ConvertBeforeAndAfterListToCollection(before,
//                 after,
//                 central.selection.list,
//                 selection,
//                 selection),
//             "ObjectPropertiesMod");
//     }
//
//     private void RotationValueChanged(Vector3 value)
//     {
//         List<string> before = central.undoRedo.ConvertBlockListToJSONList(central.selection.list);
//         properties.gameObject.transform.rotation = Quaternion.Euler(value);
//         List<string> after = central.undoRedo.ConvertBlockListToJSONList(central.selection.list);
//         List<string> selection = central.undoRedo.ConvertSelectionToStringList(central.selection.list);
//         central.validation.BreakLock(
//             central.undoRedo.ConvertBeforeAndAfterListToCollection(before,
//                 after,
//                 central.selection.list,
//                 selection,
//                 selection),
//             "ObjectPropertiesMod");
//     }
//
//     private void ScaleValueChanged(Vector3 value)
//     {
//         List<string> before = central.undoRedo.ConvertBlockListToJSONList(central.selection.list);
//         properties.gameObject.transform.localScale = value;
//         List<string> after = central.undoRedo.ConvertBlockListToJSONList(central.selection.list);
//         List<string> selection = central.undoRedo.ConvertSelectionToStringList(central.selection.list);
//         central.validation.BreakLock(
//             central.undoRedo.ConvertBeforeAndAfterListToCollection(before,
//                 after,
//                 central.selection.list,
//                 selection,
//                 selection),
//             "ObjectPropertiesMod");
//     }
//
//     private void BoosterAccelerationValueChanged(float value)
//     {
//         blockEditBooster.bridge.SetFloatValue(blockEditBooster.boosterAccelerationNR, value);
//         blockEditBooster.BoosterValueChanged();
//         inspector.CreateInspectorUI();
//     }
//
//     private void BoosterValueValueChanged(float value)
//     {
//         blockEditBooster.bridge.SetFloatValue(blockEditBooster.boosterValueNR, value);
//         blockEditBooster.BoosterValueChanged();
//         inspector.CreateInspectorUI();
//     }
//
//     private void SlowsterAccelerationValueChanged(float value)
//     {
//         blockEditSlowster.bridge.SetFloatValue(blockEditSlowster.boosterAccelerationNR, value);
//         blockEditSlowster.BoosterValueChanged();
//         inspector.CreateInspectorUI();
//     }
//
//     private void SlowsterValueValueChanged(float value)
//     {
//         blockEditSlowster.bridge.SetFloatValue(blockEditSlowster.boosterValueNR, value);
//         blockEditSlowster.BoosterValueChanged();
//         inspector.CreateInspectorUI();
//     }
//
//     private void FanForceValueChanged(float value)
//     {
//         blockEditFan.bridge.SetFloatValue(blockEditFan.fanForceNR, value);
//         blockEditFan.BoosterValueChanged();
//         inspector.CreateInspectorUI();
//     }
//
//     private void FanRangeValueChanged(float value)
//     {
//         blockEditFan.bridge.SetFloatValue(blockEditFan.fanRangeNR, value);
//         blockEditFan.BoosterValueChanged();
//         inspector.CreateInspectorUI();
//     }
//
//     private void CycleValueValueChanged(float value)
//     {
//         blockEditCycleOffset.bridge.SetFloatValue(blockEditCycleOffset.cycleValueNR, value);
//         blockEditCycleOffset.BoosterValueChanged();
//         inspector.CreateInspectorUI();
//     }
//
//     private void FeatherDampingValueChanged(float value)
//     {
//         blockEditFeatherForceField.bridge.SetFloatValue(blockEditFeatherForceField.velocityValueNR, value);
//         blockEditFeatherForceField.BoosterValueChanged();
//         inspector.CreateInspectorUI();
//     }
//
//     private void FeatherHeightValueChanged(float value)
//     {
//         blockEditFeatherForceField.bridge.SetFloatValue(blockEditFeatherForceField.heightValueNR, value);
//         blockEditFeatherForceField.BoosterValueChanged();
//         inspector.CreateInspectorUI();
//     }
//
//     private void FeatherVelocityValueChanged(float value)
//     {
//         blockEditFeatherForceField.bridge.SetFloatValue(blockEditFeatherForceField.dampingValueNR, value);
//         blockEditFeatherForceField.BoosterValueChanged();
//         inspector.CreateInspectorUI();
//     }
//
//     private void RemoveUI()
//     {
//         LevelEditorApi.UnblockKeyboardInput(this);
//         LevelEditorApi.UnblockMouseInput(this);
//
//         Destroy(canvas);
//         properties = null;
//     }
//
//     private bool IsMouseOver()
//     {
//         return canvas != null && window != null &&
//                RectTransformUtility.RectangleContainsScreenPoint((RectTransform)window.transform, Input.mousePosition);
//     }
// }
