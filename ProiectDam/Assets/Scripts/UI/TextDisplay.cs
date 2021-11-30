using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Values;

namespace UI
{
    [RequireComponent(typeof(Text)), ExecuteInEditMode]
    public class TextDisplay : MonoBehaviour
    {
        private static Regex Regex = new Regex(@"{\d+}");

        [SerializeField] private Value[] _values;

        private Text _text;

        private void Start()
        {
            _text = GetComponent<Text>();

#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                FormatText();
            }
#else
            FormatText();
#endif
        }

        private void FormatText()
        {
            string text = _text.text;

            _text.text = string.Format(text, (object[])_values);
        }

#if UNITY_EDITOR

        private string lastText;

        private void Update()
        {
            string text = GetComponent<Text>().text;

            if (lastText != null && lastText == text)
            {
                return;
            }

            int count = GetUniqueCount(text);

            if (count == _values.Length)
            {
                return;
            }

            Value[] lastValues = _values;
            _values = new Value[count];

            Array.Copy(lastValues, _values, Math.Min(lastValues.Length, count));

            lastText = text;
        }

        private int GetUniqueCount(string text)
        {
            MatchCollection matches = Regex.Matches(text);
            HashSet<int> numbers = new HashSet<int>();

            foreach (Match match in matches)
            {
                int number = int.Parse(match.Value.Substring(1, match.Value.Length - 2));

                if (!numbers.Contains(number))
                {
                    numbers.Add(number);
                }
            }

            return numbers.Count;
        }

#endif
    }
}
