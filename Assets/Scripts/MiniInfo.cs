using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniInfo : MonoBehaviour {

    public float showTime;
    private float currentShowTime;
    private bool show;

    public List<Image> images;
    public List<Text> texts;

    private Star currentStar;
    // Start is called before the first frame update
    void Start() { 
        images = new List<Image>();
        texts = new List<Text>();
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
        }
    }

    public void ShowStar(Star star) {
        if (star == currentStar) {
            show = true;
            return;
        }
        currentShowTime = showTime;
        show = true;
        Text[] textArray = texts.ToArray();
        textArray[0].text = star.GetName();
        textArray[1].text = star.GetTradeItem().ToString();
        textArray[2].text = star.ProductionRate().ToString();
        if (star.IsAutoProduction()) {
            textArray[3].text = "Automated";
        } else {
            textArray[3].text = "Unautomated";
        }
        
        textArray[4].text = star.GetResource(star.GetTradeItem()).ToString();
        currentStar = star;
    }

    public void Hide() {
        if (!show) {
            return;
        }
        currentShowTime = showTime;
        show = false;
    }

    // Update is called once per frame
    void Update() {
        if (currentShowTime > 0f) {
            currentShowTime -= Time.deltaTime;
            currentShowTime = Mathf.Clamp(currentShowTime, 0f, showTime);
            float transparency = currentShowTime / showTime;
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
        Text[] textArray = texts.ToArray();
        if (currentStar) {
            textArray[4].text = currentStar.GetResource(currentStar.GetTradeItem()).ToString();
        }
    }
}
