using Interfaces;
using Scriptable_objects;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IEventListenerInterface
{
    [SerializeField] private ShopElement shopElement;
    [SerializeField] private GameObject imagePrefab;
    [SerializeField] private IntVariable money;
    private Image _image;

    private DragShopObject _spawnedDragElement;

    private bool _dragging;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _image.sprite = shopElement.shopIcon;
        
        money.raiseOnValueChanged.RegisterListener(this);
        OnEventRaised();
    }

    

    private void OnDisable()
    {
        money.raiseOnValueChanged.UnregisterListener(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_spawnedDragElement)
            return;

        _spawnedDragElement.transform.position = GetCursorWorldPos();

    }

    public void OnButtonPressed()
    {
        if (!CanAfford())
            return;

        _dragging = true;
        //Removes this icon
        _image.color = new Color(255, 255, 255, 0);

        _spawnedDragElement = Instantiate(imagePrefab, GetCursorWorldPos(), Quaternion.identity).GetComponent<DragShopObject>();
        _spawnedDragElement.ShopElement = shopElement;
        _spawnedDragElement.reachRing.SetDefault(shopElement.defaultRange);
    }

    public void OnButtonUp()
    {
        if (!_dragging) return;
        _dragging = false;
        //Shows this icon again
        _image.color = new Color(255, 255, 255, 1);
        
        Destroy(_spawnedDragElement.gameObject);
    }

    private Vector3 GetCursorWorldPos()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }


    public void OnEventRaised()
    {
        _image.color = CanAfford() ? Color.white : Color.gray;
    }

    private bool CanAfford()
    {
        return shopElement.cost <= money.Value;
    }

    public void ShowPrice()
    {
        ToolTip.ShowToolTip($"${shopElement.cost}");
    }

    public void HidePrice()
    {
        ToolTip.HideToolTip();
    }
}