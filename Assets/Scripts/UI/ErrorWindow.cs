using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ErrorWindowConfig : BaseWindowConfig
    {
        public List<CommandErrorStruct> errors;
    }

    public class ErrorWindow : BaseWindow
    {
        [SerializeField] private TextMeshProUGUI _errorText;
        [SerializeField] private Button _buttonClose;

        private ErrorWindowConfig _errorConfig;

        public override void SetWindowConfig(BaseWindowConfig config)
        {
            base.SetWindowConfig(config);

            _errorConfig = config as ErrorWindowConfig;

            if (_errorConfig == null)
                return;

            StringBuilder sb = new StringBuilder();
            sb.Append("Find errors: ");

            foreach (var s in _errorConfig.errors)
            {
                sb.Append(Environment.NewLine);
                sb.Append($"{s.Type} in line {s.LineNumber}");
            }

            _errorText.SetText(sb);
        }

        public override void Open()
        {
            base.Open();
            _buttonClose.onClick.AddListener(() => Close());
        }

        public override void Close()
        {
            base.Close();
            _buttonClose.onClick.RemoveAllListeners();
        }
    }
}
