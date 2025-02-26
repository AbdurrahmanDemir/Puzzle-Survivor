using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tabsil.ModernUISystem
{
    public class TalentButton : MonoBehaviour
    {
        [Header(" Elements ")]
        [SerializeField] private Button button;

        [Header(" Settings ")]
        [SerializeField] private string title;
        [SerializeField] private string description;

        // Start is called before the first frame update
        void Start()
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => FloatingNoteManager.Show(button.image.rectTransform.position, title, description));
        }
    }
}