using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld
{
    public interface IDamageable
    {
        public void OnDamage(int damage);
    }
}
