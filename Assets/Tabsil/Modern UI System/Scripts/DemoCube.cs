using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabsil.ModernUISystem
{
    public class DemoCube : MonoBehaviour
    {
        [Header(" Settings ")]
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float moveMagnitude;
        [SerializeField] private float moveSpeed;

        // Update is called once per frame
        void Update()
        {
            transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * rotationSpeed);
            transform.position = transform.position.With(y: moveMagnitude * Mathf.Sin(moveSpeed * Time.time));
        }
    }
}