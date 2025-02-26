using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabsil.ModernUISystem
{
    public class TestCursor : MonoBehaviour
    {

        [Header(" Elements ")]
        [SerializeField] private GameObject cursor;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            cursor.transform.position = Input.mousePosition;
        }
    }
}