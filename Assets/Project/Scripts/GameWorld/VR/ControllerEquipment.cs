using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace GameWorld
{
    public class ControllerEquipment : MonoBehaviour
    {
        private XRBaseControllerInteractor m_Controller;

        public XRBaseControllerInteractor Controller => this.m_Controller;

        public void OnEquip(SelectEnterEventArgs arg)
        {
            Debug.Log(arg.interactorObject);

            if (arg.interactorObject is XRBaseControllerInteractor controller)
            {
                this.m_Controller = controller;
            }
        }

        public void OnUnequip(SelectExitEventArgs arg)
        {
            if (arg.interactorObject is XRBaseControllerInteractor controller)
            {
                this.m_Controller = null;
            }
        }
    }
}
