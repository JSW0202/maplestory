using System.Collections;
using Unity.Cinemachine;
using Unity.Collections;
using UnityEngine;

public class Prologue_Camera_Controller : MonoBehaviour
{
    private CinemachineCamera mainCamera;
    private GameObject cameraPos;
    private Transform cameraTarget;
    [SerializeField] Transform barlogPos;
    [SerializeField] Transform pidgeonPos;
    [SerializeField] Transform[] charPos;
    [SerializeField] private CinemachineBasicMultiChannelPerlin noise;
    [SerializeField] private Collider2D firstCollider;
    [SerializeField] private Collider2D secondCollider;



    public void Start()
    {
        mainCamera = GetComponent<CinemachineCamera>();
        noise = mainCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();

        ScreenComposerSettings composition = GetComponent<CinemachinePositionComposer>().Composition;

        mainCamera.Follow = barlogPos;
    }

    public void TargetBarlog()
    {
        mainCamera.Follow = barlogPos;
    }

    public void TargetChar(int numb)
    {
        mainCamera.Follow = charPos[numb - 1];
    }

    public void StopFollow()
    {
        mainCamera.Follow = null;
    }

    public void TargetPidgeon()
    {
        mainCamera.Follow = pidgeonPos;
    }

    public void TightenCamera()
    {
        StartCoroutine(TightenCameraCoroutine());
    }

    private IEnumerator TightenCameraCoroutine()
    {
        float elapsedTime = 0f;
        float duration = 4f; // 줌인에 걸리는 시간
        float startSize = mainCamera.Lens.OrthographicSize;
        float targetSize = 3.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            
            // 이징 함수를 사용하여 더 자연스러운 움직임 구현
            float smoothT = Mathf.SmoothStep(0, 1, t);
            
            mainCamera.Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, smoothT);
            yield return null;
        }

        // 정확한 최종값 설정
        mainCamera.Lens.OrthographicSize = targetSize;
    }

    public void LoosenCamera()
    {
        StartCoroutine(LoosenCameraCoroutine());
    }

    private IEnumerator LoosenCameraCoroutine()
    {
        float elapsedTime = 0f;
        float duration = 1f; // 줌아웃에 걸리는 시간
        float startSize = mainCamera.Lens.OrthographicSize;
        float targetSize = 5.0f; // 원하는 줌아웃 크기로 조정하세요

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            
            // 이징 함수를 사용하여 더 자연스러운 움직임 구현
            float smoothT = Mathf.SmoothStep(0, 1, t);
            
            mainCamera.Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, smoothT);
            yield return null;
        }

        // 정확한 최종값 설정
        mainCamera.Lens.OrthographicSize = targetSize;
    }

    public void SetCameraLens(float size)
    {
        mainCamera.Lens.OrthographicSize = size;
    }

    public void SwithCollider()
    {
        mainCamera.GetComponent<CinemachineConfiner2D>().BoundingShape2D = secondCollider;
    }

   //카메라 쉐이크 함수
}


