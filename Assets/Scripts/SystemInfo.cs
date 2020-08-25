using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemInfo : MonoBehaviour
{
    public List<Image> images;
    public List<Text> texts;
    private Star star;

    private float drawTime, currentDrawTime;
    public bool show;

    public void SetStar(Star s) {
        star = s;
        texts.ToArray()[0].text = s.GetName();
    }

    // Start is called before the first frame update
    void Start()
    {
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

    public void Show(Star s, float time) {
        SetStar(s);
        drawTime = time;
        currentDrawTime = time;
        show = true;
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
    }
}
