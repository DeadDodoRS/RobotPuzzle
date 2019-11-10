using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class BaseWindow : MonoBehaviour
    {
        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void SetWindowConfig(BaseWindowConfig config)
        {

        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
