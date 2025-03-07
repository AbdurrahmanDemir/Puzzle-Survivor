using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballBall : MonoBehaviour
{
    public GameObject basketball; // Basketbol topu
    public Transform launchPoint; // Topun f�rlat�laca�� nokta
    public Transform hoop; // Potan�n konumu
    public float launchForce = 10f; // F�rlatma kuvveti
    public float launchAngle = 45f; // F�rlatma a��s�

    private void Update()
    {
        // Sol mouse t�klamas�yla topu f�rlat
        //if (Input.GetMouseButtonDown(0))
        //{
        //    ShootBasketball();
        //}
    }

    public void ShootBasketball()
    {
        // Topun bir kopyas�n� olu�tur
        GameObject ball = Instantiate(basketball, launchPoint.position, Quaternion.identity);

        // Topa Rigidbody bile�eni eklenmi� mi kontrol et
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = ball.AddComponent<Rigidbody>();
        }

        // Hedef noktaya do�ru bir vekt�r hesapla
        Vector3 direction = hoop.position - launchPoint.position;

        // Yatay ve dikey bile�enleri hesapla
        float horizontalForce = launchForce * Mathf.Cos(launchAngle * Mathf.Deg2Rad);
        float verticalForce = launchForce * Mathf.Sin(launchAngle * Mathf.Deg2Rad);

        // Topa kuvvet uygula
        rb.linearVelocity = new Vector3(direction.normalized.x * horizontalForce, verticalForce, direction.normalized.z * horizontalForce);

        Destroy(ball, 4f);
        
    }

}
