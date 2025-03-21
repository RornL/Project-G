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
        rb.freezeRotation = true; // ĳ���Ͱ� ���������� ȸ���ϴ� �� ����
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
            // ĳ���Ͱ� �̵� ������ ���ϵ��� ȸ�� (�ε巯�� ȸ��)
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);

            // �̵� �ӵ� ����
            Vector3 moveVelocity = moveDirection * speed;
            rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
        }
        else
        {
            // �̵� Ű�� ������ X, Z ���� �ӵ��� 0���� �����Ͽ� ���߰� ��
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // ���� Y�� �ӵ� �ʱ�ȭ
        rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
    }

    IEnumerator Dash()
    {
        isDashing = true;

        Vector3 dashDirection = transform.forward; // ���� �ٶ󺸴� �������� ���
        rb.velocity = dashDirection * dashPower;

        yield return new WaitForSeconds(0.2f); // ��� ���� �ð�

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
