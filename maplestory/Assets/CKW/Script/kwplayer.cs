using System.Collections;
using UnityEngine;

public class kwplayer : MonoBehaviour
{
    public static kwplayer instance;
    private MapChangeImage changeScene;
    private PlayerSFXManager playerSFX => GetComponent<PlayerSFXManager>();

    Collider2D col;
    Rigidbody2D rb;
    public float moveSpeed = 2f;
    public float jumpForce = 3f;

    public bool isInPortal = false;
    public bool isInTeleport = false;
    public string targetSceneName = "";



    void Awake()
    {
        if (instance != null && this != instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        changeScene = FindAnyObjectByType<MapChangeImage>();

    }

    void Update()
    {
        Movement();
        PortalAndTeleport();

    }

    private void PortalAndTeleport()
    {
        if (isInPortal && Input.GetKeyDown(KeyCode.UpArrow))
        {
            //SceneManager.LoadScene(targetSceneName);
            SoundManager.instance.PlaySFX(SoundManager.SFXType.portal);
            changeScene.FadeOutLoadScene(targetSceneName);
        }

        if (isInTeleport && Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position = TeleportPos.instance.PortalSetPos();
            playerSFX.PlayerJump();

        }
    }

    void Movement()
    {
        float xInput = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocityY);

        if (Input.GetKeyDown(KeyCode.W))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(DisableColliderTemporarily());
        }
    }

    IEnumerator DisableColliderTemporarily() // 아래점프 (콜라이더 비활성화임)
    {
        col.enabled = false;
        yield return new WaitForSeconds(0.2f); // 통과할 시간
        col.enabled = true;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Portal"))
        {
            isInPortal = true;
            Portal portal = collision.GetComponentInParent<Portal>();
            if (portal != null)
            {
                targetSceneName = portal.GetTargetSceneName();
            }
        }

        if (collision.CompareTag("Teleport"))
        {
            isInTeleport = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Portal"))
        {
            isInPortal = false;
            targetSceneName = "";
        }

        if (collision.CompareTag("Teleport"))
        {
            isInTeleport = false;
        }
    }
}



