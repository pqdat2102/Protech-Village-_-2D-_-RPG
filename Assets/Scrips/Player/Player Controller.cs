using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public bool FacingLeft { get { return facingLeft; } }

    [SerializeField] private float moveSpeed = 1f;  // Tốc độ di chuyển của nhân vật, có thể chỉnh sửa từ Inspector.
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;

    private PlayerControls playerControls;  // Đối tượng để xử lý input từ người chơi.
    private Vector2 movement;  // Lưu trữ hướng di chuyển.
    private Rigidbody2D rb;  // Thành phần Rigidbody2D để xử lý vật lý.
    private Animator myAnimator;  // Thành phần Animator để quản lý hoạt ảnh.
    private SpriteRenderer mySpriteRender;  // Thành phần SpriteRenderer để thay đổi hướng nhân vật.
    private float startingMoveSpeed;

    private bool facingLeft = false;
    private bool isDashing = false;

    protected override void Awake()
    {
        base.Awake();

        // Khởi tạo hệ thống điều khiển của người chơi.
        playerControls = new PlayerControls();

        // Lấy component Rigidbody2D từ GameObject.
        rb = GetComponent<Rigidbody2D>();

        // Lấy component Animator từ GameObject.
        myAnimator = GetComponent<Animator>();

        // Lấy component SpriteRenderer từ GameObject.
        mySpriteRender = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        playerControls.Combat.Dash.performed += _ => Dash();

        startingMoveSpeed = moveSpeed;
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
            facingLeft = true;
        }
        else
        {
            mySpriteRender.flipX = false;
            facingLeft = false;
        }
    }

    private void Dash()
    {
        if (!isDashing)
        {
            isDashing = true;
            moveSpeed *= dashSpeed;
            myTrailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        float dashTime = 0.2f;
        float dashCD = 0.25f;
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startingMoveSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }
}
