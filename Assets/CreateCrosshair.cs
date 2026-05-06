using UnityEngine;
using UnityEngine.UI;

public class CreateCrosshair : MonoBehaviour
{
    void Start()
    {
        // Créer Canvas
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        // Créer le point
        GameObject crosshair = new GameObject("Crosshair");
        crosshair.transform.SetParent(canvasObj.transform);

        Image image = crosshair.AddComponent<Image>();
        image.color = Color.white;

        RectTransform rect = crosshair.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(10, 10);
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
    }
}