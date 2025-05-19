using UnityEngine;
using UnityEngine.Playables;
using Unity.Cinemachine;

public class start_sign : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayableDirector tutorialTL;

    private BoxCollider2D boxCollider;
    private LayerMask interactableLayer;  // 상호작용 가능한 레이어


    void Start()
    {
        anim = GetComponent<Animator>();
        mainCamera = Camera.main;
        boxCollider = GetComponent<BoxCollider2D>();

        interactableLayer = LayerMask.GetMask("Interactable");
        gameObject.layer = LayerMask.NameToLayer("Interactable");

    }

    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        Collider2D hit = Physics2D.OverlapPoint(worldPosition, interactableLayer);
        
        
        if (hit != null && hit.gameObject == gameObject)
        {
            if (!anim.GetBool("hover"))
            {
                anim.SetBool("hover", true);
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                anim.SetBool("clicked", true);
            }

        }

        else if (anim.GetBool("hover"))
        {
            anim.SetBool("hover", false);
        }

        if (Input.GetMouseButtonUp(0))
        {
            anim.SetBool("clicked", false);
            if(hit != null && hit.gameObject == gameObject)
            {
                tutorialTL.Play();
            }
        }
    }
}



