using System.Globalization;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace TNRD.Zeepkist.ObjectProperties.UI
{
    [PublicAPI]
    public class FloatInput : MonoBehaviour
    {
        public delegate void ValueChangedDelegate(float after);

        public delegate void GainedFocusDelegate(FloatInput sender);

        public delegate void LostFocusDelegate(FloatInput sender);

        public static CultureInfo Culture { get; set; }

        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private TMP_InputField input;

        private float equalityChecker = float.MaxValue;

        private bool previousFocus;

        public event ValueChangedDelegate ValueChanged;

        public event GainedFocusDelegate GainedFocus;
        public event LostFocusDelegate LostFocus;

        public string Label
        {
            get => label.text;
            set => label.text = value;
        }

        public float Value
        {
            get => Utilities.TryParseFloat(input.text, Culture);
            set
            {
                if (Mathf.Approximately(equalityChecker, value))
                    return;

                SetValueNoNotify(value);
            }
        }

        private void Awake()
        {
            input.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(string text)
        {
            equalityChecker = Utilities.TryParseFloat(input.text, Culture);
            ValueChanged?.Invoke(equalityChecker);
        }

        public void SetValueNoNotify(float value)
        {
            equalityChecker = value;
            input.SetTextWithoutNotify(value.ToString(Culture));
        }

        private void Update()
        {
            bool focus = input.isFocused;

            if (focus && !previousFocus)
            {
                GainedFocus?.Invoke(this);
            }
            else if (!focus && previousFocus)
            {
                LostFocus?.Invoke(this);
            }

            previousFocus = focus;
        }
    }
}
