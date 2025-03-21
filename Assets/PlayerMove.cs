using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 5f;
    public float jumpHeight = 5f;
    public float dashPower = 10f;
    public float rotSpeed = 10f;

    private Vector3 moveDirection = Vector3.zero;
    private bool isGrounded = false;
    private bool isDashing = false;

    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // 캐릭터가 물리적으로 회전하는 것 방지
    }

    void Update()
    {
        HandleInput();
        CheckGround();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        if (Input.GetButtonDown("Dash") && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            Move();
        }
    }

    void HandleInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(moveX, 0, moveZ).normalized;
    }

    void Move()
    {
        if (moveDirection.magnitude > 0)
        {
            // 캐릭터가 이동 방향을 향하도록 회전 (부드러운 회전)
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);

            // 이동 속도 적용
            Vector3 moveVelocity = moveDirection * speed;
            rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
        }
        else
        {
            // 이동 키를 놓으면 X, Z 방향 속도를 0으로 설정하여 멈추게 함
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // 기존 Y축 속도 초기화
        rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
    }

    IEnumerator Dash()
    {
        isDashing = true;

        Vector3 dashDirection = transform.forward; // 현재 바라보는 방향으로 대시
        rb.velocity = dashDirection * dashPower;

        yield return new WaitForSeconds(0.2f); // 대시 지속 시간

        isDashing = false;
    }

    void CheckGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
