using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CommandExampleUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _commandExample;
        [SerializeField] private TextMeshProUGUI _argsExample;
        [SerializeField] private Button _buttonCopy;

        private string _methodName;
        private string[] _args;

        public void Initialize(string command, string[] args = null)
        {
            _methodName = command;
            _args = args;

            _commandExample.SetText(command);

            if (args == null)
            {
                _argsExample.SetText(string.Empty);
                return;
            }

            StringBuilder st = new StringBuilder(); 
            foreach (var arg in args)
                st.Append($"- {arg + Environment.NewLine}");

            _argsExample.SetText(st);
        }

        private void OnEnable()
        {
            _buttonCopy.onClick.AddListener(() => 
            {
                string textToCopy;

                if (_args != null && _args.Length > 0)
                    textToCopy = $"{_methodName}({_args[0]})";
                else
                    textToCopy = $"{_methodName}()";

#if UNITY_EDITOR
                EditorGUIUtility.systemCopyBuffer = textToCopy;
#endif

#if UNITY_ANDROID
                TextEditor editor = new TextEditor
                {
                    text = textToCopy
                };
                editor.SelectAll();
                editor.Copy();
#endif
            });
        }

        private void OnDisable()
        {
            _buttonCopy.onClick.RemoveAllListeners();
        }

    }
}
