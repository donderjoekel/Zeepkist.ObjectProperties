using JetBrains.Annotations;
using UnityEngine;

namespace TNRD.Zeepkist.ObjectProperties.UI
{
    [PublicAPI]
    public class ObjectPropertiesWindow : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private int smallHeight;
        [SerializeField] private int largeHeight;

        [Header("Transform")] [SerializeField] private Vector3Input positionInput;
        [SerializeField] private Vector3Input rotationInput;
        [SerializeField] private Vector3Input scaleInput;

        [Header("Booster")] [SerializeField] private FloatInput boosterAccelerationInput;
        [SerializeField] private FloatInput boosterValueInput;

        [Header("Fan")] [SerializeField] private FloatInput fanRangeInput;
        [SerializeField] private FloatInput fanForceInput;

        [Header("Slowster")] [SerializeField] private FloatInput slowsterAccelerationInput;
        [SerializeField] private FloatInput slowsterValueInput;

        [Header("Cycle")] [SerializeField] private FloatInput cycleValueInput;

        [Header("Feather")] [SerializeField] private FloatInput featherHeightInput;
        [SerializeField] private FloatInput featherVelocityInput;
        [SerializeField] private FloatInput featherDampingInput;

        public Vector3Input PositionInput => positionInput;

        public Vector3Input RotationInput => rotationInput;

        public Vector3Input ScaleInput => scaleInput;

        public FloatInput BoosterAccelerationInput => boosterAccelerationInput;

        public FloatInput BoosterValueInput => boosterValueInput;

        public FloatInput FanRangeInput => fanRangeInput;

        public FloatInput FanForceInput => fanForceInput;

        public FloatInput SlowsterAccelerationInput => slowsterAccelerationInput;

        public FloatInput SlowsterValueInput => slowsterValueInput;

        public FloatInput CycleValueInput => cycleValueInput;

        public FloatInput FeatherHeightInput => featherHeightInput;

        public FloatInput FeatherVelocityInput => featherVelocityInput;

        public FloatInput FeatherDampingInput => featherDampingInput;

        public void HideAllInputs()
        {
            ShowTransformInputs(false);
            ShowBoosterInputs(false);
            ShowFanInputs(false);
            ShowSlowsterInputs(false);
            ShowCycleInputs(false);
            ShowFeatherInputs(false);
        }

        public void ShowTransformInputs(bool shouldShow)
        {
            positionInput.gameObject.SetActive(shouldShow);
            rotationInput.gameObject.SetActive(shouldShow);
            scaleInput.gameObject.SetActive(shouldShow);
        }

        public void ShowBoosterInputs(bool shouldShow)
        {
            boosterAccelerationInput.gameObject.SetActive(shouldShow);
            boosterValueInput.gameObject.SetActive(shouldShow);
        }

        public void ShowFanInputs(bool shouldShow)
        {
            fanRangeInput.gameObject.SetActive(shouldShow);
            fanForceInput.gameObject.SetActive(shouldShow);
        }

        public void ShowSlowsterInputs(bool shouldShow)
        {
            slowsterAccelerationInput.gameObject.SetActive(shouldShow);
            slowsterValueInput.gameObject.SetActive(shouldShow);
        }

        public void ShowCycleInputs(bool shouldShow)
        {
            cycleValueInput.gameObject.SetActive(shouldShow);
        }

        public void ShowFeatherInputs(bool shouldShow)
        {
            featherHeightInput.gameObject.SetActive(shouldShow);
            featherVelocityInput.gameObject.SetActive(shouldShow);
            featherDampingInput.gameObject.SetActive(shouldShow);
        }
    }
}
