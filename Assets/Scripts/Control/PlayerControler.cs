using TWQ.Movement;
using UnityEngine;
using TWQ.Combat;
using TWQ.Inventory;
using TWQ.Attributes;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using TMPro;

namespace TWQ.Control
{
    public class PlayerControler : MonoBehaviour
    {
        Health health;
        Fighter fighter;
        WeaponInventory weaponInventory;
        GameObject inventory;
        bool isPaused = false;

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMeshProjectionDistance = 1f;
        [SerializeField] float raycastRadius = 1f;
        [SerializeField] GameObject gameOverCanvas;
        [SerializeField] TextMeshProUGUI deathText;
        void Start()
        {
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            inventory = GameObject.FindGameObjectWithTag("Inventory");
            weaponInventory = inventory.GetComponent<WeaponInventory>();

        }

        // Update is called once per frame
        void Update()
        {
            if (health.IsDead)
            {
                SetCursor(CursorType.None);
                gameOverCanvas.SetActive(true);
                deathText.text = "You Die";

                return;
            }
            if (InteractWithPausing()) return;
            if (InteractWithMovement()) return;
            if (InteractWithComponent()) return;
            //if (InteractWithUI()) return;
            if (Input.GetKeyDown(KeyCode.C)) SwitchWeapons();
            SetCursor(CursorType.None);
        }

        private bool InteractWithPausing()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(!isPaused)
                {
                    gameOverCanvas.SetActive(true);
                    Time.timeScale = 0;
                    isPaused = true;
                }
                else 
                {
                    gameOverCanvas.SetActive(false);
                    Time.timeScale = 1;
                    isPaused = false;
                }
                return true;
            }
            return false;
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach(IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }

        private bool InteractWithMovement()
        {
            if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
            {
                GetComponent<NavMeshAgent>().ResetPath();
                float yAxis = Input.GetAxis("Vertical");
                float xAxis = Input.GetAxis("Horizontal");
                GetComponent<Mover>().StartMoveAction((transform.position + Camera.main.transform.forward * yAxis * Time.deltaTime * 100 + Camera.main.transform.right * xAxis * Time.deltaTime * 100), 1f);
                SetCursor(CursorType.None);
                return true;
            }/**/
             /* I'm disabling this part because i prefer moving with keys i'm also willing to make the combat system
             work with keys (letting only ranged weapons for clicking) */

            /*RaycastHit hit;
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(target, 1f);
                    SetCursor(CursorType.Movement);
                }
                return true;
            }*/

            return false;
            
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();

            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(
                hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;
            return true;
        }
        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }
        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        public void SwitchWeapons()
        {
            if (weaponInventory.StoredWeapons.Count == 0) return;
            int switchIndex = (GetCurrentWeaponIndex() + 1) % weaponInventory.StoredWeapons.Count;
            fighter.EquppingWeapon(weaponInventory.StoredWeapons[switchIndex]);
        }

        public int GetCurrentWeaponIndex()
        {
            int iterator = 0;
            foreach (WeaponConfig weapon in weaponInventory.StoredWeapons)
            {
                if (weapon.Equals(fighter.currentWeaponConfig))
                {
                    return iterator;
                }
                iterator++;
            }
            return 0;
        }/**/
    }
}



