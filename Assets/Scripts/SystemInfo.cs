using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemInfo : MonoBehaviour
{
    public List<Image> images;
    public List<Text> texts;
    private Star star;

    private Button productionButton, factoryButton;
    private Slider productionSlider;

    private float drawTime, currentDrawTime;
    public bool show;

    private float currentTimeToProduce;
    private Universe universe;

    public void SetStar(Star s) {
        star = s;
        texts.ToArray()[0].text = s.GetName();
    }

    // Start is called before the first frame update
    void Start()
    {
        images = new List<Image>();
        texts = new List<Text>();
        AddSubTransforms(transform);
        foreach (Transform t in transform) {
            switch (t.name) {
                case "ProduceButton":
                    productionButton = t.GetComponent<Button>();
                    productionButton.onClick.AddListener(StartProduction);
                    break;
                case "FactoryButton":
                    factoryButton = t.GetComponent<Button>();
                    factoryButton.onClick.AddListener(AddFactories);
                    break;
                case "ProduceSlider":
                    productionSlider = t.GetComponent<Slider>();
                    break;
            }
        }
        universe = FindObjectOfType<Universe>();
    }

    private void AddSubTransforms(Transform transform) {
        foreach (Transform t in transform) {
            Image image = t.GetComponent<Image>();
            if (image) {
                images.Add(image);
                Color imageColour = image.color;
                imageColour.a = 0f;
                image.color = imageColour;
            }
            Text text = t.GetComponent<Text>();
            if (text) {
                texts.Add(text);
                Color textColour = text.color;
                textColour.a = 0f;
                text.color = textColour;
            }
            AddSubTransforms(t);
        }
    }

    public void Show(Star s, float time) {
        SetStar(s);
        drawTime = time;
        currentDrawTime = time;
        show = true;
        Text[] textArray = texts.ToArray();
        textArray[2].text = s.GetTradeItem().ToString();
        textArray[5].text = star.GetFactoryRequirement() + " Available";
        textArray[8].text = "Build " + star.GetFactoryName();
        UpdateOwned();
    }

    public void UpdateOwned() {
        // We are never going to enter this when the star isn't set!
        if (!star) {
            return;
        }
        Text[] textArray = texts.ToArray();
        if (star.IsAutoProduction()) {
            textArray[4].text = star.GetResource(TradeItem.WORKERS).ToString() 
                + " ✕ " + star.GetFactories().ToString() + " = "
                + star.ProductionRate().ToString();
        } else {
            textArray[4].text = star.ProductionRate().ToString();
        }
        long buildableFactories = star.GetBuildableFactories();
        textArray[6].text = buildableFactories.ToString();
        productionButton.interactable = !star.IsAutoProduction() && 
            star.ProductionRate() > 0 && currentTimeToProduce == 0;
        factoryButton.interactable = buildableFactories > 0;
        textArray[14].text = star.GetResource(TradeItem.WORKERS).ToString();
        textArray[15].text = star.GetResource(TradeItem.PARTS).ToString();
        textArray[16].text = star.GetResource(TradeItem.FUEL).ToString();
        textArray[17].text = star.GetResource(TradeItem.FOOD).ToString();
        textArray[18].text = star.GetResource(TradeItem.LUXURY).ToString();
    }

    public void Hide(float time) {
        drawTime = time;
        currentDrawTime = time;
        show = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDrawTime > 0f) {
            currentDrawTime -= Time.deltaTime;
            currentDrawTime = Mathf.Clamp(currentDrawTime, 0f, drawTime);
            float transparency = currentDrawTime / drawTime;
            if (show) {
                transparency = 1f - transparency;
            }
            foreach (Image i in images) {
                Color panelColour = i.color;
                panelColour.a = transparency * 0.5f;
                i.color = panelColour;
            }
            foreach (Text t in texts) {
                Color textColour = t.color;
                textColour.a = transparency;
                t.color = textColour;
            }
        }
        if (star && star.IsAutoProduction()) {
            currentTimeToProduce = universe.currentTimeToProduce;
        }
        if (currentTimeToProduce > 0f) {
            currentTimeToProduce -= Time.deltaTime;
            currentTimeToProduce = Mathf.Clamp(currentTimeToProduce, 0f, universe.timeToProduce);
            float completion = 1.0f - (currentTimeToProduce / universe.timeToProduce);
            productionSlider.value = completion;
            if (completion == 1.0f && !star.IsAutoProduction()) {
                star.RunProduction();
                UpdateOwned();
            }
        }
    }

    private void AddFactories() {
        star.AddFactories();
        UpdateOwned();
    }

    private void StartProduction() {
        currentTimeToProduce = universe.timeToProduce;
        UpdateOwned();
    }
}
