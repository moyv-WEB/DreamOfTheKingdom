using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class FadePanel : MonoBehaviour
{
    private VisualElement background;
 
    private void Awake()
    {
        background = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Background");
    }
    public void FadeIn(float duration)
    {
        DOVirtual.Float(from: 0, to: 1, duration, value => 
        {
            background.style.opacity = value;
        }).SetEase(Ease.InQuad);
    }
    public void FadeOut(float duration) 
    {
        DOVirtual.Float(from: 1, to: 0, duration, value =>
        {
            background.style.opacity = value;
        }).SetEase(Ease.InQuad);
    }
}
