using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool FacingLeft { get {  return facingLeft; }  set { facingLeft = value; } }
    public static PlayerController Instance;

    [SerializeField] private float moveSpeed = 1f;  // Tốc độ di chuyển của nhân vật, có thể chỉnh sửa từ Inspector.

    private PlayerControls playerControls;  // Đối tượng để xử lý input từ người chơi.
    private Vector2 movement;  // Lưu trữ hướng di chuyển.
    private Rigidbody2D rb;  // Thành phần Rigidbody2D để xử lý vật lý.
    private Animator myAnimator;  // Thành phần Animator để quản lý hoạt ảnh.
    private SpriteRenderer mySpriteRender;  // Thành phần SpriteRenderer để thay đổi hướng nhân vật.

    private bool facingLeft = false;

    private void Awake()
    {
        Instance = this;
        // Khởi tạo hệ thống điều khiển của người chơi.
        playerControls = new PlayerControls();

        // Lấy component Rigidbody2D từ GameObject.
        rb = GetComponent<Rigidbody2D>();

        // Lấy component Animator từ GameObject.
        myAnimator = GetComponent<Animator>();

        // Lấy component SpriteRenderer từ GameObject.
        mySpriteRender = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        // Kích hoạt hệ thống điều khiển của người chơi.
        playerControls.Enable();
    }

    private void Update()
    {
        // Gọi hàm xử lý input của người chơi.
        PlayerInput();
    }

    private void FixedUpdate()
    {
        // Điều chỉnh hướng nhân vật dựa vào vị trí chuột.
        AdjustPlayerFacingDirection();

        // Di chuyển nhân vật.
        Move();
    }

    private void PlayerInput()
    {
        // Lấy giá trị input di chuyển từ hệ thống điều khiển.
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        // Cập nhật giá trị cho Animator để điều chỉnh animation di chuyển.
        myAnimator.SetFloat("move_X", movement.x);
        myAnimator.SetFloat("move_Y", movement.y);
    }

    private void Move()
    {
        // Di chuyển nhân vật bằng cách cập nhật vị trí Rigidbody2D.
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        // Lấy vị trí chuột trên màn hình.
        Vector3 mousePos = Input.mousePosition;

        // Chuyển vị trí nhân vật sang tọa độ màn hình.
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        // Nếu chuột nằm bên trái nhân vật, lật sprite để nhân vật nhìn sang trái.
        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRender.flipX = true;
            FacingLeft = true;
        }
        else
        {
            mySpriteRender.flipX = false;
            FacingLeft = false;
        }
    }
}
