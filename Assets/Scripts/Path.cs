using UnityEngine;

public class Path : MonoBehaviour {
    private Star start, end;
    private TradeItem tradeItem;
    Vector3 direction;

    private float size = 0.5f;

    public void SetupPath(Star start, Star end, TradeItem t) {
        this.start = start;
        this.end = end;
        tradeItem = t;
        transform.position = (end.transform.position + start.transform.position) * 0.5f;
        direction = end.transform.position - start.transform.position;
        float scale = direction.magnitude;
        transform.localScale = new Vector3(size, scale, size);
        transform.localRotation = Quaternion.FromToRotation(Vector3.up, direction);
        foreach (Transform quad in transform) {
            Material m = quad.GetComponent<MeshRenderer>().materials[0];
            m.color = GetColour();
            m.mainTextureScale = new Vector2(1f, scale/size);
        }
    }

    public Color GetColour() {
        return Trading.TradeColors[(int)tradeItem];
    }

    public bool IsDestination(Star star) {
        return star == end;
    }

    private void Update() {
        if (start == null || end == null || start == end) {
            return;
        }
        foreach (Transform quad in transform) {
            Material m = quad.GetComponent<MeshRenderer>().materials[0];
            m.mainTextureOffset += new Vector2(0f, -Time.deltaTime);
        }
    }
}
