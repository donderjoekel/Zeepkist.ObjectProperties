using System;
using System.Globalization;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace TNRD.Zeepkist.ObjectProperties.UI
{
    [PublicAPI]
    public class Vector3Input : MonoBehaviour
    {
        public delegate void ValueChangedDelegate(Vector3 value);
        public delegate void GainedFocusDelegate(Vector3Input sender);
        public delegate void LostFocusDelegate(Vector3Input sender);

        public static CultureInfo Culture { get; set; }

        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private TMP_InputField xInput;
        [SerializeField] private TMP_InputField yInput;
        [SerializeField] private TMP_InputField zInput;

        private Vector3 equalityChecker = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

        private bool previousFocus;

        public event ValueChangedDelegate ValueChanged;

        public event GainedFocusDelegate GainedFocus;
        public event LostFocusDelegate LostFocus;

        public string Label
        {
            get => label.text;
            set => label.text = value;
        }

        public Vector3 Value
        {
            get => ParseValue();
            set
            {
                if (equalityChecker == value)
                    return;

                SetValueNoNotify(value);
            }
        }

        private Vector3 ParseValue()
        {
            return new Vector3(Utilities.TryParseFloat(xInput.text, Culture),
                Utilities.TryParseFloat(yInput.text, Culture),
                Utilities.TryParseFloat(zInput.text, Culture));
        }

        private void Awake()
        {
            xInput.onValueChanged.AddListener(OnValueChanged);
            yInput.onValueChanged.AddListener(OnValueChanged);
            zInput.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(string text)
        {
            equalityChecker = ParseValue();
            ValueChanged?.Invoke(equalityChecker);
        }

        public void SetValueNoNotify(Vector3 value)
        {
            equalityChecker = value;
            xInput.SetTextWithoutNotify(value.x.ToString(Culture));
            yInput.SetTextWithoutNotify(value.y.ToString(Culture));
            zInput.SetTextWithoutNotify(value.z.ToString(Culture));
        }

        private void Update()
        {
            bool focus = xInput.isFocused || yInput.isFocused || zInput.isFocused;

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
