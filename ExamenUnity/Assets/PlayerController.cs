using UnityEngine;

public class PlayerController3D : MonoBehaviour
{
    public float moveSpeed = 5f;    // Velocidad de movimiento del jugador
    public float jumpForce = 10f;   // Fuerza de salto del jugador
    public float sprintSpeed = 10f; // Velocidad de sprint del jugador
    private Rigidbody rb;
    private bool isGrounded;
    private bool doubleJumpAvailable = true;
    private Vector3 originalGravity;
    private Vector3 startPosition;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalGravity = Physics.gravity;
        startPosition = new Vector3(0f, 3.45f, 0f);
    }

    void Update()
    {
        // Movimiento horizontal
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontalInput, 0f, 0f) * moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);

        // Sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movement *= moveSpeed / sprintSpeed; // Ajustar la velocidad de sprint
            rb.MovePosition(transform.position + movement);
        }

        // Saltar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
                doubleJumpAvailable = true;
            }
            else if (doubleJumpAvailable)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Resetear la velocidad vertical
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                doubleJumpAvailable = false;
            }
        }

        // Modificar la gravedad cuando el jugador no esté en el suelo
        if (!isGrounded)
        {
            Physics.gravity = originalGravity * 2f; // Duplicar la gravedad
        }
        else
        {
            Physics.gravity = originalGravity; // Restaurar la gravedad original
        }

        if (transform.position.y <= -7f)
        {
            transform.position = startPosition;
            rb.velocity = Vector3.zero;
            isGrounded = true;
            Physics.gravity = originalGravity;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Verificar si el jugador está en el suelo
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            doubleJumpAvailable = true;
        }
    }

}
