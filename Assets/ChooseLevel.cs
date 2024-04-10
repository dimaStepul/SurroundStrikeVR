using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement; // Добавляем пространство имен для работы со сценами

namespace Oculus.Interaction
{
    /// <summary>
    /// Override Toggle to clear state on drag while still bubbling events up through
    /// the hierarchy. Particularly useful for buttons inside of scroll views.
    /// </summary>
    public class ToggleDeselect : Toggle
    {
        [SerializeField]
        private bool _clearStateOnDrag = false;

        /// <summary>
        /// Gets or sets a value indicating whether to clear state on drag.
        /// </summary>
        public bool ClearStateOnDrag
        {
            get { return _clearStateOnDrag; }
            set { _clearStateOnDrag = value; }
        }

        /// <summary>
        /// Event handler for when dragging begins.
        /// </summary>
        public void OnBeginDrag(PointerEventData pointerEventData)
        {
            if (!_clearStateOnDrag)
            {
                return;
            }
            InstantClearState();
            DoStateTransition(SelectionState.Normal, true);
            ExecuteEvents.ExecuteHierarchy(
                transform.parent.gameObject,
                pointerEventData,
                ExecuteEvents.beginDragHandler
            );
        }

        // Функция для открытия сцены (уровня) при нажатии на тоггл
        private void OnToggleClicked(bool isOn)
        {
            if (isOn)
            {
                // Открываем сцену "SampleScene"
                SceneManager.LoadScene("SampleScene");
            }
        }

        protected override void Start()
        {
            base.Start();
            // Добавляем слушателя событий к тогглу
            onValueChanged.AddListener(OnToggleClicked);
        }
    }
}
