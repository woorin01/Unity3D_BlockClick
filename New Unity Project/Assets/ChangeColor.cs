using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    public Color color = new Color(0, 0, 0);
    public Image image;
    void Start()
    {
        image.color = color;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Insert))
            if (color.r < 1)
                color += new Color(1f / 255, 0, 0);
        if (Input.GetKey(KeyCode.Delete))
            if (color.r > 0)
                color += new Color(-1f / 255, 0, 0);

        if (Input.GetKey(KeyCode.Home))
            if (color.g < 1)
                color += new Color(0, 1f / 255, 0);
        if (Input.GetKey(KeyCode.End))
            if (color.g > 0)
                color += new Color(0, -1f / 255, 0);

        if (Input.GetKey(KeyCode.PageUp))
            if (color.b < 1)
                color += new Color(0, 0, 1f / 255);
        if (Input.GetKey(KeyCode.PageDown))
            if (color.b > 0)
                color += new Color(0, 0, -1f / 255);

        image.color = color;
    }
}
