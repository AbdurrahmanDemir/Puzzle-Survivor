using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tabsil.ModernUISystem
{
    [RequireComponent(typeof(Button))]
    public class FloatingNoteTrigger : MonoBehaviour
    {
        [Header(" Settings ")]
        [SerializeField] private string primaryText;
        [SerializeField] private string secondaryText;
        [SerializeField] private Transform parent;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            FloatingNoteManager.Show(transform.position, primaryText, secondaryText, parent));
        }
    }
}