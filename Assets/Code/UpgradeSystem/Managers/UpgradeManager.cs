using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UpgradeManager : MonoBehaviour
{
    private int choiceCount = 3;

    public VoxelBody voxelBody;
    public Player player;
    public UpgradeLibrary upgradeLibrary;
    public WeaponManager weaponManager;

    public object fixedChoice;
    public string fixedChoiceName = "Fixed Choice";
    public string fixedChoiceDescription = "";
    public Sprite fixedChoiceIcon;

    public List<UpgradeDefinition> upgradePool = new List<UpgradeDefinition>();


    private void Start()
    {
        foreach(UpgradeDefinition upgrade in upgradeLibrary.upgrades)
        {
            upgradePool.Add(upgrade);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EnterUpgradeState();
        }

        ((CallbackState)GameManager.Instance.StateManager.GetState(GameState.Upgrading)).onEnterCallback += (s) => { EnterUpgradeState(); };
    }

    public class ChoiceBundle
    {
        // Replace fixed choice with the meatCube class?
        public object fixedChoice;
        
        public List<UpgradeDefinition> randomUpgrades;
    }

    private class ChoiceCard
    {
        public Button button;
        public Image icon;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI descriptionText;
        public object payload;
    }

    private GameObject choicesRoot;
    private readonly List<ChoiceCard> choiceCards = new List<ChoiceCard>(3);

    ChoiceBundle GetChoices()
    {
        ChoiceBundle bundle = new ChoiceBundle
        {
            fixedChoice = fixedChoice,
            randomUpgrades = new List<UpgradeDefinition>(2)
        };

        if (upgradePool.Count < 2)
        {
            throw new InvalidOperationException("UpgradePool must contain at least 2 items.");
        }

        int maxChoices = 2;
        HashSet<int> picked = new HashSet<int>();
        while (picked.Count < maxChoices)
        {
            int index = Random.Range(0, upgradePool.Count);
            if (picked.Add(index))
            {
                bundle.randomUpgrades.Add(upgradePool[index]);
            }
        }

        return bundle;
    }

    void ShowChoices(ChoiceBundle choices)
    {
        if (choices == null)
        {
            throw new ArgumentNullException(nameof(choices));
        }

        if (choices.randomUpgrades == null || choices.randomUpgrades.Count != 2)
        {
            throw new InvalidOperationException("ShowChoices expects exactly 2 random upgrades.");
        }

        EnsureChoicesUi();

        choiceCards[0].payload = choices.fixedChoice;
        choiceCards[0].icon.sprite = fixedChoiceIcon;
        choiceCards[0].nameText.text = fixedChoiceName;
        choiceCards[0].descriptionText.text = fixedChoiceDescription;

        UpgradeDefinition upgrade1 = choices.randomUpgrades[0];
        choiceCards[1].payload = upgrade1;
        choiceCards[1].icon.sprite = upgrade1.icon;
        choiceCards[1].nameText.text = upgrade1.upgradeName;
        choiceCards[1].descriptionText.text = "";

        UpgradeDefinition upgrade2 = choices.randomUpgrades[1];
        choiceCards[2].payload = upgrade2;
        choiceCards[2].icon.sprite = upgrade2.icon;
        choiceCards[2].nameText.text = upgrade2.upgradeName;
        choiceCards[2].descriptionText.text = "";

        choicesRoot.SetActive(true);
    }

    private void EnsureChoicesUi()
    {
        if (choicesRoot != null && choiceCards.Count == 3)
        {
            return;
        }

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGo = new GameObject("ChoicesCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
        }

        if (FindObjectOfType<EventSystem>() == null)
        {
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }

        choicesRoot = new GameObject("ChoicesPanel", typeof(RectTransform), typeof(Image), typeof(HorizontalLayoutGroup));
        choicesRoot.transform.SetParent(canvas.transform, false);
        RectTransform rootRect = choicesRoot.GetComponent<RectTransform>();
        rootRect.anchorMin = new Vector2(0.5f, 0.5f);
        rootRect.anchorMax = new Vector2(0.5f, 0.5f);
        rootRect.pivot = new Vector2(0.5f, 0.5f);
        rootRect.sizeDelta = new Vector2(900, 420);

        Image rootImage = choicesRoot.GetComponent<Image>();
        rootImage.color = new Color(0f, 0f, 0f, 0.6f);

        HorizontalLayoutGroup layout = choicesRoot.GetComponent<HorizontalLayoutGroup>();
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.spacing = 20f;
        layout.padding = new RectOffset(20, 20, 20, 20);
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = true;

        choiceCards.Clear();
        for (int i = 0; i < 3; i++)
        {
            choiceCards.Add(CreateChoiceCard(choicesRoot.transform, $"ChoiceCard_{i + 1}"));
        }

        choicesRoot.SetActive(false);
    }

    private ChoiceCard CreateChoiceCard(Transform parent, string name)
    {
        GameObject cardGo = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button), typeof(VerticalLayoutGroup));
        cardGo.transform.SetParent(parent, false);
        RectTransform cardRect = cardGo.GetComponent<RectTransform>();
        cardRect.sizeDelta = new Vector2(260, 360);

        Image cardImage = cardGo.GetComponent<Image>();
        cardImage.color = new Color(1f, 1f, 1f, 0.1f);

        VerticalLayoutGroup layout = cardGo.GetComponent<VerticalLayoutGroup>();
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.spacing = 8f;
        layout.padding = new RectOffset(10, 10, 10, 10);
        layout.childControlWidth = true;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        GameObject iconGo = new GameObject("Icon", typeof(RectTransform), typeof(Image), typeof(LayoutElement));
        iconGo.transform.SetParent(cardGo.transform, false);
        LayoutElement iconLayout = iconGo.GetComponent<LayoutElement>();
        iconLayout.preferredHeight = 200f;
        iconLayout.flexibleHeight = 0f;
        Image iconImage = iconGo.GetComponent<Image>();
        iconImage.preserveAspect = true;

        TextMeshProUGUI nameText = CreateText(cardGo.transform, "Name", 28, FontStyles.Bold);
        TextMeshProUGUI descText = CreateText(cardGo.transform, "Description", 20, FontStyles.Normal);

        Button button = cardGo.GetComponent<Button>();
        ChoiceCard card = new ChoiceCard
        {
            button = button,
            icon = iconImage,
            nameText = nameText,
            descriptionText = descText
        };

        button.onClick.AddListener(() => OnChoiceClicked(card));
        return card;
    }

    private TextMeshProUGUI CreateText(Transform parent, string name, int fontSize, FontStyles style)
    {
        GameObject textGo = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI), typeof(LayoutElement));
        textGo.transform.SetParent(parent, false);
        TextMeshProUGUI text = textGo.GetComponent<TextMeshProUGUI>();
        text.fontSize = fontSize;
        text.fontStyle = style;
        text.alignment = TextAlignmentOptions.Center;
        text.text = "";

        LayoutElement layout = textGo.GetComponent<LayoutElement>();
        layout.preferredHeight = fontSize + 16;
        return text;
    }

    /*
    private void OnChoiceClicked(ChoiceCard card)
    {
        Debug.Log($"Choice selected: {card.payload}");
        choicesRoot.SetActive(false);
    }
    
    private void OnChoiceClicked(ChoiceCard card)
    {
        if (card.payload is UpgradeDefinition upgrade)
        {
            foreach (UpgradeEffect effect in upgrade.effects)
            {
                if (effect is AddWeaponEffect addWeapon)
                {
                    GameObject prefabToBuild = addWeapon.weapon.prefab;

                    //BuildManager.Instance.EnterBuildMode(prefabToBuild);
                    break;
                }
            }
        }

        choicesRoot.SetActive(false);
    }
    */

    private void OnChoiceClicked(ChoiceCard card)
    {
        if (card.payload is UpgradeDefinition upgrade)
        {
            weaponManager.playerBuild.ApplyUpgrade(upgrade);

            BodyPart prefab = GetPrefab(upgrade);

            if (prefab != null)
            {
                player.Body.EnterEditMode(
                    () => { GameManager.Instance.StateManager.SetState(GameState.Playing); },
                    prefab
                );
            }
     
        }

        choicesRoot.SetActive(false);
    }

    public BodyPart GetPrefab(UpgradeDefinition upgrade)
    {
        if (upgrade == null) return null;
        
        foreach (UpgradeEffect effect in upgrade.effects)
        {
            if (effect is AddWeaponEffect addWeapon)
            {
                BodyPart prefab = addWeapon.weapon.weaponPrefab;
                return prefab;
            }
        }
        
        return null;
    }

    public void EnterUpgradeState()
    {
        ShowChoices(GetChoices());
    }
}
