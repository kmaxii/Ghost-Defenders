using System;
using System.Globalization;
using Interfaces;
using Scriptable_objects;
using TMPro;
using Towers;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour, IEventListenerInterface
{
    [Header("Monkey Info")] [SerializeField]
    private Image monkeyIcon;

    [SerializeField] private TextMeshProUGUI monkeyName;
    [SerializeField] private TextWriterFromIntValueWithStart balloonsPopped;
    [SerializeField] private TextMeshProUGUI aiState;
    [SerializeField] private TextMeshProUGUI sellPrice;

    [SerializeField] private GameObject childToToggleOnAndOff;

    [Header("Upgrade path 1")] [SerializeField]
    private DataForUpgrade dataForUpgrade1;

    [Header("Upgrade path 2")] [SerializeField]
    private DataForUpgrade dataForUpgrade2;

    [SerializeField] private Image upgradeBox1;
    [SerializeField] private Image upgradeBox2;

    [Header("Other")] [SerializeField] private IntVariable money;

    private ReachRing _activeReachRing;

    private static UpgradeMenu _instance;

    private bool _blockUpgrade1;
    private bool _blockUpgrade2;

    private FullUpgradeData _fullUpgradeData;

    [SerializeField] private GameObject clickBlocker;

    private void Start()
    {
        if (_instance != null)
        {
            Debug.LogError("There can only be one UpgradeMenu.", this);
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }


    public static void Show(DataForUpgradeScreen data, Tower tower, ReachRing reachRing)
    {
        _instance.ShowGui(data, tower, reachRing);
    }


    public void HideGui()
    {
        clickBlocker.SetActive(false);
        money.raiseOnValueChanged.UnregisterListener(this);

        RemoveOldReachRing();
        childToToggleOnAndOff.SetActive(false);
        _fullUpgradeData = null;
    }

    private void OnDisable()
    {
        money.raiseOnValueChanged.UnregisterListener(this);
    }

    private void ShowGui(DataForUpgradeScreen data, Tower tower, ReachRing reachRing)
    {
        clickBlocker.SetActive(true);
        _fullUpgradeData = new FullUpgradeData(data, tower, reachRing);
        RemoveOldReachRing();
        _activeReachRing = reachRing;

        childToToggleOnAndOff.SetActive(true);

        monkeyIcon.sprite = data.CharacterIcon;
        monkeyName.text = data.Name;
        balloonsPopped.Setup(tower.balloonsPopped);
        aiState.text = data.AiState;

        UpdateSellPrice();

        UpdateUpgrades();

        money.raiseOnValueChanged.RegisterListener(this);
    }


    private void RemoveOldReachRing()
    {
        if (_activeReachRing)
        {
            _activeReachRing.HideRing();
            _activeReachRing = null;
        }
    }

    void ShowUpgradeData(DataForUpgrade toUpdate, int upgradeLevel, StatsUpgrade[] data,
        ref bool otherPathIsAboveLevel2, Image image)
    {
        //Show last bought upgrade image if there is a last bought thing
        if (upgradeLevel > 1)
        {
            toUpdate.bought.gameObject.SetActive(true);
            toUpdate.bought.sprite = data[upgradeLevel - 2].upgradeImage;
        }
        else
        {
            toUpdate.bought.gameObject.SetActive(false);
        }

        //If at max upgrade
        if (upgradeLevel > data.Length)
        {
            toUpdate.upgradeName.text = "Maxed";
            toUpdate.upgradeCost.text = "";
            toUpdate.upgradeImage.enabled = false;
            image.color = Color.gray;
        }
        else if (otherPathIsAboveLevel2)
        {
            toUpdate.upgradeName.text = "Upgrade Path Closed";
            toUpdate.upgradeCost.text = "";
            toUpdate.upgradeImage.enabled = false;
            image.color = Color.gray;
        }
        else if (data[upgradeLevel - 1].price > money.Value)
        {
            StatsUpgrade upgrade = data[upgradeLevel - 1];
            toUpdate.upgradeName.text = "CAN'T AFFORD";
            toUpdate.upgradeCost.text = "$" + upgrade.price;
            toUpdate.upgradeImage.enabled = true;
            toUpdate.upgradeImage.sprite = upgrade.upgradeImage;
            image.color = Color.gray;
            otherPathIsAboveLevel2 = true;
        }
        else
        {
            StatsUpgrade upgrade = data[upgradeLevel - 1];
            toUpdate.upgradeImage.enabled = true;
            toUpdate.upgradeImage.sprite = upgrade.upgradeImage;
            toUpdate.upgradeName.text = upgrade.upgradeName;
            toUpdate.upgradeCost.text = "$" + upgrade.price;
            image.color = Color.white;
        }

        ShowBoxes(upgradeLevel, toUpdate);
    }

    private void ShowBoxes(int amount, DataForUpgrade dataForUpgrade)
    {
        for (int i = 0; i < dataForUpgrade.boxes.Length; i++)
        {
            if (i < amount - 1)
            {
                dataForUpgrade.boxes[i].enabled = true;
            }
            else
            {
                dataForUpgrade.boxes[i].enabled = false;
            }
        }
    }


    public void OnUpgradeOneClick()
    {
        if (!_blockUpgrade1)
            _fullUpgradeData.Tower.UpgradeOne();
    }

    public void OnUpgradeTwoClick()
    {
        if (!_blockUpgrade2)
            _fullUpgradeData.Tower.UpgradeTwo();
    }

    public void OnSellButtonClick()
    {
        money.Value += Mathf.RoundToInt(_fullUpgradeData.Tower._totalMoneySpentOnThisTower * 0.75f);
        Destroy(_fullUpgradeData.Tower.transform.parent.gameObject);
        HideGui();
    }

    public void WriteDebugMessage()
    {
        Debug.Log("Entered");
    }

    private void UpdateSellPrice()
    {
        sellPrice.text =
            (Mathf.RoundToInt(_fullUpgradeData.Tower._totalMoneySpentOnThisTower * 0.75f)).ToString(CultureInfo
                .CurrentCulture);
    }

    private void UpdateUpgrades()
    {
        if (_fullUpgradeData == null)

            return;

        DataForUpgradeScreen data = _fullUpgradeData.Data;
        _blockUpgrade2 = data.CurrentState.x > 2 && data.CurrentState.y == 2;
        _blockUpgrade1 = data.CurrentState.y > 2 && data.CurrentState.x == 2;

        ShowUpgradeData(dataForUpgrade1, data.CurrentState.x + 1, data.Upgrades.path1, ref _blockUpgrade1, upgradeBox1);
        ShowUpgradeData(dataForUpgrade2, data.CurrentState.y + 1, data.Upgrades.path2, ref _blockUpgrade2, upgradeBox2);
    }

    public void OnEventRaised()
    {
        UpdateUpgrades();
    }
}

[Serializable]
public class DataForUpgrade
{
    [SerializeField] public Image bought;
    [SerializeField] public Image[] boxes = new Image[4];
    [SerializeField] public TextMeshProUGUI upgradeName;
    [SerializeField] public Image upgradeImage;
    [SerializeField] public TextMeshProUGUI upgradeCost;
}


public struct DataForUpgradeScreen
{
    public Sprite CharacterIcon;
    public String Name;
    public int BalloonsPopped;
    public String AiState;
    public TowerUpgrade Upgrades;
    public Vector2Int CurrentState;

    public DataForUpgradeScreen(Sprite characterIcon, string name, int bloonsPopped, string aiState,
        TowerUpgrade upgrades, Vector2Int currentState)
    {
        CharacterIcon = characterIcon;
        Name = name;
        BalloonsPopped = bloonsPopped;
        AiState = aiState;
        Upgrades = upgrades;
        CurrentState = currentState;
    }
}

public class FullUpgradeData
{
    public DataForUpgradeScreen Data;
    public Tower Tower;
    public ReachRing ReachRing;

    public FullUpgradeData(DataForUpgradeScreen data, Tower tower, ReachRing reachRing)
    {
        this.Data = data;
        this.Tower = tower;
        this.ReachRing = reachRing;
    }
}