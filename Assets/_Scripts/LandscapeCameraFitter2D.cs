using UnityEngine;

[RequireComponent(typeof(Camera))]
public class LandscapeCameraFitter2D : MonoBehaviour
{
    [Header("Reference (landscape)")]
    public float referenceAspect = 16f / 9f;  // 기준 화면비
    public float referenceOrthoSize = 5f;     // 기준 화면비에서의 ortho size

    Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
        Apply();
    }

    void OnEnable() => Apply();

    int lastW, lastH;
    void Update()
    {
        if (Screen.width != lastW || Screen.height != lastH)
            Apply();
    }

    void Apply()
    {
        lastW = Screen.width;
        lastH = Screen.height;

        float currentAspect = (float)Screen.width / Screen.height;

        // 기본: 세로(높이) 고정
        float size = referenceOrthoSize;

        // 화면이 기준보다 "좁으면"(예: 4:3) 좌우가 덜 보이므로
        // 높이를 조금 늘려(=줌아웃) 가로 시야를 확보
        if (currentAspect < referenceAspect)
        {
            float scale = referenceAspect / currentAspect;
            size = referenceOrthoSize * scale;
        }

        cam.orthographicSize = size;
    }
}
