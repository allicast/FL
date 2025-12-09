using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool sprint;

        // --- SOLO LO NECESARIO ---
        public bool interact;  // Para tecla 'E' (Agarrar Y Limpiar)
        public bool inventory; // Para tecla 'TAB'
        public bool pause;     // Para tecla 'ESC'
        // -------------------------

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }

        // --- NUEVOS INPUTS ---

        // Asigna la tecla "E" a esta acción en el Input System
        public void OnInteract(InputValue value)
        {
            interact = value.isPressed;
        }

        // Asigna la tecla "Tab" a esta acción
        public void OnInventory(InputValue value)
        {
            if (value.isPressed) inventory = !inventory;
        }

        // Asigna la tecla "Esc" a esta acción
        public void OnPause(InputValue value)
        {
            if (value.isPressed) pause = !pause;
        }
        // ---------------------
#endif

        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}