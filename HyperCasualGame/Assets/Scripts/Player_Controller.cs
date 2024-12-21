using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// RigidBody, Player varlýðýna fiziksel özellikler ekler.
// Box Collider, Tile varlýðýna kutu özelliði verip sabit durmasýný saðlar.
// FPS: Frame Per Second --> Saniye baþýna oluþan kare sayýsý
// Player_Controller, MonoBehaviour'dan miras alýr.
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

    // Awake, Player_Controller script'i aktif olsa da olmasa da çalýþýr.
    // Ama Start metodu script aktif olmazsa çalýþmaz.
    private void Awake()
    {
       
    }

    // Start, ilk kare güncellemesinden önce çaðrýlýr.
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update, kare baþýna bir kez çaðrýlýr.
    // Sahne deðiþimlerinde kullanýlýr.
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

    // FixedUpdate, sabit güncelleme.
    // Fizik gibi deðiþmeyenleri güncellemek için kullanýlýr.
    private void FixedUpdate()
    {
        
    }

    // Horizontal: Yatay Hareket
    // Hýzý deðiþtirir.
    // Hýzý alýr.
    void HorizontalMove()
    {
        playerRB.velocity = new Vector2(Input.GetAxis("Horizontal")*moveSpeed,playerRB.velocity.y);
        playerAnimator.SetFloat("playerSpeed",Mathf.Abs(playerRB.velocity.x));
    }

    // Yüzü gittiði yere çevirir.
    void FlipFace()
    {
        facingRight = !facingRight;
        Vector3 tempLocalScale= transform.localScale;
        tempLocalScale.x *= -1;
        transform.localScale = tempLocalScale;
    }

    // Zýplama iþleminde;
    // 1- Gerekli kuvvet kullanýcýdan alýnýr.
    // 2- Kuvvet uygulanýr.
    // 3- Karakterin sadece zemindeyken zýplamasýnýn kontrolü yapýlýr.
    // 4- Karakterin ne kadar sýklýkla zýplayabileceði dikkate alýnýr.
    void Jump()
    {
        playerRB.AddForce(new Vector2(0f,jumpSpeed));
    }

    // Karakter sadece yerdeyken zýplamasýný saðlar.
    // Yani karakterin yere deðip deðmediðini kontrol eder.
    void OnGroundCheck()
    {
        // Physics2D.OverlapCircle(pozisyon,çap,layer) þeklinde kullanýlýr.
        // Layer, objelerin Collider'larýnýn etkileþimini saðlar.
        isGrounded = Physics2D.OverlapCircle(groundCheckPosition.position,groundCheckRadius,groundCheckLayer);
        playerAnimator.SetBool("isGroundAnim", isGrounded);
    }
}
