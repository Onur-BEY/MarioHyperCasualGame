using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// RigidBody, Player varl���na fiziksel �zellikler ekler.
// Box Collider, Tile varl���na kutu �zelli�i verip sabit durmas�n� sa�lar.
// FPS: Frame Per Second --> Saniye ba��na olu�an kare say�s�
// Player_Controller, MonoBehaviour'dan miras al�r.
public class Player_Controller : MonoBehaviour
{
    Rigidbody2D playerRB;
    Animator playerAnimator;
    public float moveSpeed = 1f;
    public float jumpSpeed = 1f, jumpFrequency=1f, nextJumpTime;
    bool facingRight = true;
    public bool isGrounded = false;
    public Transform groundCheckPosition;
    public float groundCheckRadius;
    public LayerMask groundCheckLayer;

    // Awake, Player_Controller script'i aktif olsa da olmasa da �al���r.
    // Ama Start metodu script aktif olmazsa �al��maz.
    private void Awake()
    {
       
    }

    // Start, ilk kare g�ncellemesinden �nce �a�r�l�r.
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update, kare ba��na bir kez �a�r�l�r.
    // Sahne de�i�imlerinde kullan�l�r.
    void Update()
    {
        HorizontalMove();
        OnGroundCheck();

        if (playerRB.velocity.x<0 && facingRight)
        {
            FlipFace();
        }
        else if(playerRB.velocity.x>0 && !facingRight)
        {
            FlipFace();
        }

        if (Input.GetAxis("Vertical") > 0 && isGrounded && (nextJumpTime<Time.timeSinceLevelLoad))
        {
            nextJumpTime = Time.timeSinceLevelLoad+jumpFrequency;
            Jump();
        }
    }

    // FixedUpdate, sabit g�ncelleme.
    // Fizik gibi de�i�meyenleri g�ncellemek i�in kullan�l�r.
    private void FixedUpdate()
    {
        
    }

    // Horizontal: Yatay Hareket
    // H�z� de�i�tirir.
    // H�z� al�r.
    void HorizontalMove()
    {
        playerRB.velocity = new Vector2(Input.GetAxis("Horizontal")*moveSpeed,playerRB.velocity.y);
        playerAnimator.SetFloat("playerSpeed",Mathf.Abs(playerRB.velocity.x));
    }

    // Y�z� gitti�i yere �evirir.
    void FlipFace()
    {
        facingRight = !facingRight;
        Vector3 tempLocalScale= transform.localScale;
        tempLocalScale.x *= -1;
        transform.localScale = tempLocalScale;
    }

    // Z�plama i�leminde;
    // 1- Gerekli kuvvet kullan�c�dan al�n�r.
    // 2- Kuvvet uygulan�r.
    // 3- Karakterin sadece zemindeyken z�plamas�n�n kontrol� yap�l�r.
    // 4- Karakterin ne kadar s�kl�kla z�playabilece�i dikkate al�n�r.
    void Jump()
    {
        playerRB.AddForce(new Vector2(0f,jumpSpeed));
    }

    // Karakter sadece yerdeyken z�plamas�n� sa�lar.
    // Yani karakterin yere de�ip de�medi�ini kontrol eder.
    void OnGroundCheck()
    {
        // Physics2D.OverlapCircle(pozisyon,�ap,layer) �eklinde kullan�l�r.
        // Layer, objelerin Collider'lar�n�n etkile�imini sa�lar.
        isGrounded = Physics2D.OverlapCircle(groundCheckPosition.position,groundCheckRadius,groundCheckLayer);
        playerAnimator.SetBool("isGroundAnim", isGrounded);
    }
}
