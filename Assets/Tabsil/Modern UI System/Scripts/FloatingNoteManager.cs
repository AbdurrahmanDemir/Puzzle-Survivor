using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabsil.ModernUISystem
{
    public class FloatingNoteManager : MonoBehaviour
    {
        public static FloatingNoteManager instance;

        [Header(" Elements ")]
        [SerializeField] private UIFloatingNote notePrefab;
        [SerializeField] private Transform defaultParent;
        private UIFloatingNote note;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            UIManager.staticPageChanged += Hide;
        }

        private void OnDestroy()
        {
            UIManager.staticPageChanged -= Hide;    
        }

        public static void Show(Vector3 worldSpacePosition)
        {
            Show(worldSpacePosition, "");
        }

        public static void Show(Vector3 worldSpacePosition, string primaryString)
        {
            Show(worldSpacePosition, primaryString, "");
        }

        public static void Show(Vector3 worldSpacePosition, string primaryString, string secondaryString)
        {
            Show(worldSpacePosition, primaryString, secondaryString, instance.defaultParent);
        }

        public static void Show(Vector3 worldSpacePosition, string primaryString, string secondaryString, Transform parent)
        {
            if (instance.note == null)
                instance.note = Instantiate(instance.notePrefab, parent);

            if (parent == null)
                parent = instance.defaultParent;

            instance.note.gameObject.SetActive(true);
            instance.note.transform.SetParent(parent);

            instance.note.Configure(worldSpacePosition, primaryString, secondaryString);
        }

        public void Hide()
        {
            if(note != null)
                note.gameObject.SetActive(false);
        }
    }
}