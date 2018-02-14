using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TweenFadeAlphaAndScale2 : MonoBehaviour {

    private Text target;
    [SerializeField] private Shadow shadow;
    [SerializeField] private Outline outline;
    private RectTransform rectTrans;
    public bool viewing = false;

    public void StartTween() {
        shadow.enabled = false;
        outline.enabled = false;
        target = GetComponent <Text >();
        StartCoroutine(Tweener());
    }

    private IEnumerator Tweener() {
        viewing = true;
        rectTrans = target.gameObject.GetComponent<RectTransform>();
        target.color = new Color(
            target.color.r,
            target.color.g,
            target.color.b,
            0.0f
            );
        yield return null;
        yield return new WaitForSeconds(0.5f);
        this.transform.parent.parent.transform.DOLocalMoveY(1f, 2.0f);
        DOTween.ToAlpha(
            () => target.color,
            color => target.color = color,
            1.0f,
            0.1f
            );
        yield return new WaitForSeconds(0.25f);
        shadow.enabled = true;
        outline.enabled = true;
        yield return new WaitForSeconds(1f);
        DOTween.ToAlpha(
            () => target.color,
            color => target.color = color,
            0.0f,
            0.2f
            );
        shadow.enabled = false;
        outline.enabled = false;
        yield return new WaitForSeconds(0.3f);
        viewing = false;

    }

}
