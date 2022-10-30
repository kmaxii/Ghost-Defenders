using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] private GameObject damageIndicatorPrefab;
    private static ObjectPool<TextMeshPro> _damageIndicators;

    [SerializeField] private int defaultCapacity = 200;
    [SerializeField] private int maxSize = 500;


    private void Awake()
    {
        SetUpPool();
    }

    private void SetUpPool()
    {
        _damageIndicators = new ObjectPool<TextMeshPro>(
            () => Instantiate(damageIndicatorPrefab).transform.GetChild(0).GetComponent<TextMeshPro>()
            , text => { text.transform.parent.gameObject.SetActive(true); },
            text => { text.transform.parent.gameObject.SetActive(false); },
            text => Destroy(text.transform.parent.gameObject)
            , false
            , defaultCapacity,
            maxSize);
    }

    public static TextMeshPro Get()
    {
        return _damageIndicators.Get();
    }
    
    public static void Return(TextMeshPro text)
    {
        _damageIndicators.Release(text);
    }
}