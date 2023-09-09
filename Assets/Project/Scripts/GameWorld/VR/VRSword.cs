using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld
{
    using System;
    using UnityEngine.XR.Interaction.Toolkit;

    public class VRSword : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
            //grabbable.activated.AddListener(GrabSword);
        }

    }
}
