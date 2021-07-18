using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_ui : MonoBehaviour
{

    public List<Image> UI_elements = new List<Image>();
    private float duration = 2f;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (Image image in UI_elements)
        {
            Color temp = image.color;
            temp.a = 0f;
            image.color = temp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(FadeInImage(UI_elements[0]));
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(FadeOutImage(UI_elements[0]));
        }
        */
    }

    private IEnumerator FadeInImage(Image image)
    {
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            Color temp = image.color;
            // temp.a = 0f;

            temp.a = Mathf.Lerp(0, 1, currentTime / duration);
            image.color = temp;
            yield return null;
        }
        yield break;
    }

    public void FadeInImage(int i)
    {
        StartCoroutine(FadeInImage(UI_elements[i]));
    }

    private IEnumerator FadeOutImage(Image image)
    {
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            Color temp = image.color;
            // temp.a = 0f;

            temp.a = Mathf.Lerp(1, 0, currentTime / duration);
            image.color = temp;
            yield return null;
        }
        yield break;
    }

    public void FadeOutImage(int i)
    {
        StartCoroutine(FadeOutImage(UI_elements[i]));
    }

}