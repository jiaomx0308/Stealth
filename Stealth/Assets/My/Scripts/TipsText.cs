using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsText : MonoBehaviour
{
    private Text tipsText;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        tipsText = this.GetComponent<Text>();
        canvasGroup = this.GetComponent<CanvasGroup>();
        this.gameObject.SetActive(false);
    }

    private void  OnEnable()
    {
        canvasGroup.alpha = 1;
        StartCoroutine(WaitSomeTimeBeforFadeOut(3));
    }

    IEnumerator WaitSomeTimeBeforFadeOut(int time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        canvasGroup.alpha -= 0.2f;
        if (canvasGroup.alpha > 0)
        {
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(FadeOut());
        }
        else
        {
            canvasGroup.alpha = 0;
            this.gameObject.SetActive(false);
        }     
    }
}
