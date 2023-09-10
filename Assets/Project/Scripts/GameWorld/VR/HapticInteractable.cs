using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace GameWorld
{
    [System.Serializable]
    public class Haptic
    {
        [Range(0.0f, 1.0f)]
        public float intensity;
        public float duration;

        public void TriggerHaptic(BaseInteractionEventArgs eventArgs)
        {
            if (eventArgs.interactorObject is XRBaseControllerInteractor controllerInteractor)
            {
                TriggerHaptic(controllerInteractor.xrController);
            }
        }

        public void TriggerHaptic(XRBaseController xrController)
        {
            if (intensity > 0)
            {
                xrController.SendHapticImpulse(intensity, duration);
            }
        }
    }

    public class HapticInteractable : MonoBehaviour
    {
        public Haptic hapticOnActivated;
        public Haptic hapticHoverEntered;
        public Haptic hapticHoverExited;
        public Haptic hapticSelectEntered;
        public Haptic hapticSelectExited;

        private void Start()
        {
            XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
            interactable.activated.AddListener(hapticOnActivated.TriggerHaptic);
            interactable.hoverEntered.AddListener(hapticHoverEntered.TriggerHaptic);
            interactable.hoverExited.AddListener(hapticHoverExited.TriggerHaptic);
            interactable.selectEntered.AddListener(hapticSelectEntered.TriggerHaptic);
            interactable.selectExited.AddListener(hapticSelectExited.TriggerHaptic);
        }
    }
}
