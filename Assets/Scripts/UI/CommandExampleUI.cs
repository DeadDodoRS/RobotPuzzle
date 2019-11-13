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
#if UNITY_EDITOR
                if(_args != null && _args.Length > 0)
                    EditorGUIUtility.systemCopyBuffer = $"{_methodName}({_args[0]})";
                else
                    EditorGUIUtility.systemCopyBuffer = _methodName;
#endif
            });
        }

        private void OnDisable()
        {
            _buttonCopy.onClick.RemoveAllListeners();
        }

    }
}
