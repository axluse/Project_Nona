using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TweenFadeAlphaAndScale3 : MonoBehaviour {

    private Image target;
    private RectTransform rectTrans;
    public bool viewing = false;

    public void StartTween() {
        target = GetComponent < Image >();
        StartCoroutine(Tweener());
    }

    private IEnumerator Tweener() {
        viewing = true;
        rectTrans = target.gameObject.GetComponent<RectTransform>();
        rectTrans.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        target.color = new Color(
            target.color.r,
            target.color.g,
            target.color.b,
            0.0f
            );
        yield return null;

        rectTrans.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 2.0f);
        DOTween.ToAlpha(
            () => target.color,
            color => target.color = color,
            1.0f,
            0.5f
            );
        viewing = false;
    }

}
