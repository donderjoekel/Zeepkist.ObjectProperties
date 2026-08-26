using System.Globalization;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace TNRD.Zeepkist.ObjectProperties.UI
{
    [PublicAPI]
    public class IntInput : MonoBehaviour
    {
        public delegate void ValueChangedDelegate(int after);
        
        public static CultureInfo Culture { get; set; }

        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private TMP_InputField input;

        private int equalityChecker = int.MaxValue;

        public event ValueChangedDelegate ValueChanged;

        public string Label
        {
            get => label.text;
            set => label.text = value;
        }

        public int Value
        {
            get => Utilities.TryParseInt(input.text, Culture);
            set
            {
                if (value == equalityChecker)
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
            equalityChecker = Utilities.TryParseInt(input.text, Culture);
            ValueChanged?.Invoke(equalityChecker);
        }

        public void SetValueNoNotify(int value)
        {
            equalityChecker = value;
            input.SetTextWithoutNotify(value.ToString(Culture));
        }
    }
}
