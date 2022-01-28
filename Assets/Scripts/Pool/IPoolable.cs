using UnityEngine;

namespace Pool
{
    public interface IPoolable
    {
        void OnSpawn();
        void OnDespawn();
    }
}