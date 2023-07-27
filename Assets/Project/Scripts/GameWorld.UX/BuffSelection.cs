using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld.UX
{
    public class BuffSelection : UXBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            this.InitializeDoc();
            UXManager.Instance.BuffSelection = this;

            this.m_Root.visible = false;
        }

        public void DisplayCard()
        {
            this.m_Root.visible = true;
        }
    }
}
