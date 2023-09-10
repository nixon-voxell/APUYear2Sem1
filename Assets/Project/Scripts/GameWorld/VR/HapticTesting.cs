using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace GameWorld
{
    public class HapticTesting : MonoBehaviour
    {

        [SerializeField] private XRBaseControllerInteractor controller;
        public Haptic haptic;
        

        public void TestHaptic()
        {
            controller.SendHapticImpulse(haptic.intensity, haptic.duration);
        }
    }
}
