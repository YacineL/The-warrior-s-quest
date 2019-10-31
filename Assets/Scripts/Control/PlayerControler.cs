using TWQ.Movement;
using UnityEngine;
using TWQ.Combat;
using TWQ.Core;

namespace TWQ.Control
{
    public class PlayerControler : MonoBehaviour
    {
        Health health;
        Fighter fighter;
        // Start is called before the first frame update
        void Start()
        {
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();

        }

        // Update is called once per frame
        void Update()
        {
            if (health.IsDead) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (!GetComponent<Fighter>().CanAttack(target.gameObject))
                {
                    continue;
                }

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
            {
                float yAxis = Input.GetAxis("Vertical");
                float xAxis = Input.GetAxis("Horizontal");
                GetComponent<Mover>().StartMoveAction((transform.position + Vector3.forward * yAxis + Vector3.right * xAxis), 1f);
                return true;
            }/**/
            /* I'm disabling this part because i prefer moving with keys i'm also willing to make the combat system
            work with keys (letting only ranged weapons for clicking) */
            
            /*RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }
                return true;
            }*/
           
            return false;
            
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}



