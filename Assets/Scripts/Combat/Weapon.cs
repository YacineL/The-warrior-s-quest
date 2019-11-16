using UnityEngine;
using UnityEngine.Events;

namespace TWQ.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] UnityEvent onHit;
        public void OnHit()
        {
            onHit.Invoke();
            print(this.gameObject.name);
        }
    }
}
