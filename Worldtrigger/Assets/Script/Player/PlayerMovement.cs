using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float mouseSensitivity = 2f;
    public Transform headTransform;
    public Transform leftFootTransform;
    public Transform rightFootTransform;

    public bool isMovingForward = false;
    public bool isMovingBackward = false;
    public bool isMovingLeft = false;
    public bool isMovingRight = false;
    public bool isJumping = false;

    private Rigidbody rb;
    private bool isGrounded;
    private float rotationX = 0f;
    private bool canJump = true;
    private Vector3 moveDirection;
    private float jumpCooldown = 0.3f;
    private float lastJumpTime = -0.3f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on this GameObject.");
        }
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 地面チェック
        bool leftGrounded = leftFootTransform != null && Physics.Raycast(leftFootTransform.position, Vector3.down, 1.1f);
        bool rightGrounded = rightFootTransform != null && Physics.Raycast(rightFootTransform.position, Vector3.down, 1.1f);
        isGrounded = leftGrounded || rightGrounded;

        // クールタイムチェック
        canJump = isGrounded && (Time.time - lastJumpTime >= jumpCooldown);

        // InputManagerからアクションの状態を取得
        isMovingForward = InputManager.Instance.actionStates.ContainsKey("前進") && InputManager.Instance.actionStates["前進"];
        isMovingBackward = InputManager.Instance.actionStates.ContainsKey("後退") && InputManager.Instance.actionStates["後退"];
        isMovingLeft = InputManager.Instance.actionStates.ContainsKey("左移動") && InputManager.Instance.actionStates["左移動"];
        isMovingRight = InputManager.Instance.actionStates.ContainsKey("右移動") && InputManager.Instance.actionStates["右移動"];
        isJumping = InputManager.Instance.actionStates.ContainsKey("ジャンプ") && InputManager.Instance.actionStates["ジャンプ"];

        // 移動ベクトルを計算
        Vector3 move = Vector3.zero;
        if (isMovingForward) move += transform.forward;
        if (isMovingBackward) move -= transform.forward;
        if (isMovingLeft) move -= transform.right;
        if (isMovingRight) move += transform.right;

        // 移動を正規化して速度を適用
        if (move != Vector3.zero)
        {
            moveDirection = move.normalized * moveSpeed;
        }
        else
        {
            moveDirection = Vector3.zero;
        }

        // ジャンプ
        if (isJumping && canJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            lastJumpTime = Time.time;
        }

        // カメラ回転 (頭をマウスで振り向く)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);  // プレイヤー本体をY軸回転

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        if (headTransform != null)
        {
            headTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDirection * Time.fixedDeltaTime);
    }
}