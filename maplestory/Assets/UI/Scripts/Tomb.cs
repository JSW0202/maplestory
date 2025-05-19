using UnityEngine;
using System.Collections;

public class Tomb : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private float speed = 12f;
    [SerializeField] private float xOffset = 0.15f;
    [SerializeField] private float yOffset = 0.45f;
    [SerializeField] private float fallHeight = 5.5f;
    [SerializeField] private float destroyDelay = 1.2f;

    private Vector3 targetPosition;
    private bool hasDropped = false;

    private Enemy enemyToReturn;
    private AdventureClass jobTypeToReturn;

    void Start()
    {
        anim = GetComponent<Animator>();

        Vector3 startPos = transform.position;
        targetPosition = new Vector3(
            startPos.x + xOffset,
            startPos.y - yOffset,
            0);

        transform.position = new Vector3(
            startPos.x + xOffset,
            startPos.y + fallHeight,
            0);

        if (SoundManager.instance != null)
            SoundManager.instance.PlaySFX(SoundManager.SFXType.death);
    }

    void Update()
    {
        if (!hasDropped)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {
                hasDropped = true;
                anim.SetBool("drop", true);
                StartCoroutine(RemoveAfterAnimation());
            }
        }
    }

    public void InitAndReturn(Enemy enemy, AdventureClass jobType)
    {
        this.enemyToReturn = enemy;
        this.jobTypeToReturn = jobType;
    }

    private IEnumerator RemoveAfterAnimation()
    {
        yield return new WaitForSeconds(destroyDelay);

        if (enemyToReturn != null)
            EnemyPool.Instance.Return(enemyToReturn.gameObject, jobTypeToReturn);

        Destroy(gameObject); // 항상 묘비도 삭제
    }

}
