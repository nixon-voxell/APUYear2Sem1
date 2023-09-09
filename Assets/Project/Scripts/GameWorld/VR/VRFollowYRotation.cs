using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld
{
    public class VRFollowYRotation : MonoBehaviour
    {
        [SerializeField] private Transform m_Target;

        // Update is called once per frame
        void Update()
        {
            Vector3 targetEuler = this.m_Target.localEulerAngles;

            this.transform.localEulerAngles = new Vector3(0, targetEuler.y, 0);
        }

    }
}
