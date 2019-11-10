using TWQ.Movement;
using UnityEngine;
using TWQ.Combat;
using TWQ.Inventory;
using TWQ.Resources;
using System;
using UnityEngine.EventSystems;

namespace TWQ.Control
{
    public class PlayerControler : MonoBehaviour
    {
        Health health;
        Fighter fighter;
        WeaponInventory weaponInventory;
        Transform camera;
        GameObject inventory;
        enum CursorType
        {
            None,
            Movement,
            Combat,
            UI,
        }

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        // Start is called before the first frame update
        void Start()
        {
            camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            inventory = GameObject.FindGameObjectWithTag("Inventory");
            weaponInventory = inventory.GetComponent<WeaponInventory>();

        }

        // Update is called once per frame
        void Update()
        {
            
            if (InteractWithUI()) return;
            if (health.IsDead)
            {
                SetCursor(CursorType.None);
                return;
            }
            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;
            if (Input.GetKeyDown(KeyCode.C)) SwitchWeapons();
            SetCursor(CursorType.None);
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
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach(IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(CursorType.Combat);
                        return true;
                    }
                }
            }
            return false;
        }


        private bool InteractWithMovement()
        {
            if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
            {
                float yAxis = Input.GetAxis("Vertical");
                float xAxis = Input.GetAxis("Horizontal");
                GetComponent<Mover>().StartMoveAction((transform.position + camera.forward * yAxis * Time.deltaTime * 100 + camera.right * xAxis * Time.deltaTime * 100), 1f);
                SetCursor(CursorType.Movement);
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
            foreach (Weapon weapon in weaponInventory.StoredWeapons)
            {
                if (weapon.Equals(fighter.currentWeapon))
                {
                    return iterator;
                }
                iterator++;
            }
            return 0;
        }/**/
    }
}



