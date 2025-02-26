using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballBall : MonoBehaviour
{
    public GameObject basketball; // Basketbol topu
    public Transform launchPoint; // Topun fýrlatýlacaðý nokta
    public Transform hoop; // Potanýn konumu
    public float launchForce = 10f; // Fýrlatma kuvveti
    public float launchAngle = 45f; // Fýrlatma açýsý

    private void Update()
    {
        // Sol mouse týklamasýyla topu fýrlat
        //if (Input.GetMouseButtonDown(0))
        //{
        //    ShootBasketball();
        //}
    }

    public void ShootBasketball()
    {
        // Topun bir kopyasýný oluþtur
        GameObject ball = Instantiate(basketball, launchPoint.position, Quaternion.identity);

        // Topa Rigidbody bileþeni eklenmiþ mi kontrol et
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = ball.AddComponent<Rigidbody>();
        }

        // Hedef noktaya doðru bir vektör hesapla
        Vector3 direction = hoop.position - launchPoint.position;

        // Yatay ve dikey bileþenleri hesapla
        float horizontalForce = launchForce * Mathf.Cos(launchAngle * Mathf.Deg2Rad);
        float verticalForce = launchForce * Mathf.Sin(launchAngle * Mathf.Deg2Rad);

        // Topa kuvvet uygula
        rb.velocity = new Vector3(direction.normalized.x * horizontalForce, verticalForce, direction.normalized.z * horizontalForce);

        Destroy(ball, 4f);
        
    }

}
