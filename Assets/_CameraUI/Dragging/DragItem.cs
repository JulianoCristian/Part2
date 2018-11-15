﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RPG.InventorySystem;

namespace RPG.CameraUI.Dragging
{
    public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        Vector3 _startPosition;
        Transform _originalParent;

        Canvas _parentCanvas;
        IDragContainer _parentContainer;

        private void Awake()
        {
            _parentCanvas = GetComponentInParent<Canvas>();
            _parentContainer = GetComponentInParent<IDragContainer>();
        }

        public IDragContainer parentContainer { get { return _parentContainer; } }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startPosition = transform.position;
            _originalParent = transform.parent;
            transform.parent = _parentCanvas.transform;
            // Else won't get the drop event.
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.position = _startPosition;
            transform.parent = _originalParent;
            GetComponent<CanvasGroup>().blocksRaycasts = true;

            // Not over UI we should drop.
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                DropItem();
            }

            var container = GetContainer(eventData);
            if (container != null)
            {
                DropItemIntoContainer(container);
            }
        }

        private void DropItem()
        {
            var item = _parentContainer.ReplaceItem(null);
            var inventory = Inventory.GetPlayerInventory();
            inventory.DropItem(item);
        }

        private IDragContainer GetContainer(PointerEventData eventData)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            foreach (var raycastResult in results)
            {
                var container = raycastResult.gameObject.GetComponent<IDragContainer>();

                if (container != null) 
                {
                    return container;
                }
            }
            return null;
        }

        private void DropItemIntoContainer(IDragContainer receivingContainer)
        {
            var draggingItem = _parentContainer.ReplaceItem(null);
            var swappedItem = receivingContainer.ReplaceItem(draggingItem);
            if (swappedItem != null)
            {
                _parentContainer.ReplaceItem(swappedItem);
            }
        }
    }
}