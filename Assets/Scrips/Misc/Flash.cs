using UnityEngine;
using System.Collections;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material whiteFlashMat; // Material hiện lên khi quái bị tấn công ( hiện tại là màu trắng )
    [SerializeField] private float restoreDefaultMatTime = 0.2f; // Thời gian mỗi lần bị 2 flash liên tiếp

    private Material defaultMat; // Material mặc định của quái  
    private SpriteRenderer spriteRenderer;
    
    private void Awake() // Gọi các Component
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMat = spriteRenderer.material;
    }

    public float GetRestoreMatTime()
    {
        return restoreDefaultMatTime;
    }

    public IEnumerator FlashRoutine() // Thời gian giữa 2 lần bị flash ( hiện trắng ) liên tiếp và xử lý khi máu quái về 0
    {
        spriteRenderer.material = whiteFlashMat;
        yield return new WaitForSeconds(restoreDefaultMatTime);
        spriteRenderer.material = defaultMat;
    }
}
