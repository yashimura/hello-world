using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tuto_yajirusi : MonoBehaviour
{
    public GameObject whoite;
    private Vector3 defaultPos;
    public float speed = 4f;
    public float yajirusipow = 4f;
    public float interval = 0.1f;

    private Image image;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        image = whoite.gameObject.GetComponent<Image>();
        defaultPos = this.gameObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //矢印上下運動
        this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, defaultPos.y + Mathf.PingPong(Time.time * speed, yajirusipow), this.gameObject.transform.localPosition.z);

        //明滅
        image.color = GetAlphaColor(image.color);
    }

    private Color GetAlphaColor(Color color)
    {
        //time += Time.deltaTime * 5.0f * speed;
        time += Time.deltaTime * speed;

        color.a = Mathf.Sin(time) * 0.5f + 0.5f;
        //color.a = Mathf.Abs(Mathf.Sin(time));
        return color;
    }

    
}
