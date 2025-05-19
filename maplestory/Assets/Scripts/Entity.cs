using TreeEditor;
using UnityEditorInternal;
using UnityEngine;

public class Entity : MonoBehaviour
{
    //모든 움직이는 물체는 애니메이터와 리지드바디2d가 있어야하니까
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Collider2D col { get; private set; }
    #endregion

    [Header("체력 정보")]
    public int maxHp;
    public int currentHp;
    public bool isDead;
    public bool isInvincible;

    [Header("이동 정보")]
    public float moveSpeed;
    //가만히 있다가 행동 전환에 필요한 시간
    public float idleTime;


    [Header("충돌 정보")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected LayerMask whatIsWall;


    public int facingDir { get; protected set; } = -1;
    protected bool facingRight = false;

    public System.Action onFlipped; // 플립 시 호출 되는 델리게이트

    protected virtual void Awake()
    {
        isDead = false;
        isInvincible = false;
    }

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }


    protected virtual void Update()
    {
    }


    public virtual void GetDamage(int damage)
    {
        if (!isInvincible && currentHp > 0)
        {
            Debug.Log(damage + "만큼 데미지 받음");
            currentHp -= damage;
        }
        else
            return;
    }


    public virtual void Die()
    {
        isDead = true;
        ZeroVelocity();
        anim.SetTrigger("Die");
    }


    #region 충돌
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsWall);


    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
    }
    #endregion



    #region 플립
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        onFlipped?.Invoke();

    }


    /*public virtual void FlipController(float _x)
    {
        if (_x < 0 && !facingRight)
            Flip();
        else if (_x > 0 && facingRight)
            Flip();

    }*/
    //입력받을때 쓰는건데 그냥 자동으로 움직이는 애한테 박으니까 계속 Flip하지
    //GetAxisRaw("Horizontal")로 입력받는 애들한테 쓰는거임

    #endregion


    #region 속력
    public void ZeroVelocity() => rb.linearVelocity = new Vector2(0, 0);

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        //FlipController(_xVelocity);
    }
    #endregion
}