using Scriptable_objects;
using UnityEngine;
using UnityEngine.Events;

public class DragShopObject : MonoBehaviour
{

    private SpriteRenderer _spriteRenderer;
    [SerializeField] private IntVariable money;
    public DefaultReachRing reachRing;

    private ShopElement _shopElement;


    private int _collidingWithTower;

    private bool _canSpawn = false;

    [SerializeField] private UnityEvent unityEvent;
    
    public ShopElement ShopElement
    {
        set
        {
         _shopElement = value;
         _spriteRenderer = GetComponent<SpriteRenderer>();

         _spriteRenderer.sprite = _shopElement.shopIcon;
         GetComponent<CircleCollider2D>().enabled = true;
        }

    }
    
    // Update is called once per frame
    void Update()
    {
        if (_shopElement)
        {
            _canSpawn = CanSpawnOnPosition();
            
            if (_canSpawn)
            {
                reachRing.MakeBlack();
            }
            else
            {
                reachRing.MakeRed();
            }
        }
    }

    private void OnDestroy()
    {
        if (_canSpawn)
        {
            Instantiate(_shopElement.prefabToSpawn, transform.position, Quaternion.identity);
            money.Value -= _shopElement.cost;
            unityEvent.Invoke();
        }
    }

   


    private bool CanSpawnOnPosition()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return !GridManager.Instance.HasTile(_shopElement.cantSpawnOn.list, pos) && _collidingWithTower == 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.gameObject.tag.Equals("TowerCollider")) return;

        _collidingWithTower++;

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(!other.gameObject.tag.Equals("TowerCollider")) return;

        _collidingWithTower--;
        
    }
}
