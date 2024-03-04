using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score Instance { get; private set; }

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI scoreExample;

    [SerializeField]
    private float updateSpeed = 100;

    [SerializeField]
    private int upTravel = 100;

    [SerializeField]
    private float bounceTime = 0.2f;

    [SerializeField]
    private float bounceForce = 1.5f;

    [SerializeField]
    private float fadeTime = 1f;

    [SerializeField]
    private Color color1 = Color.yellow;

    [SerializeField]
    private Color color2 = Color.white;

    private float displayedScore = 0F;

    private int score = 0;

    private void Awake()
    {
        score = 0;
        displayedScore = 0f;
        RefreshScore();
        scoreExample.gameObject.SetActive(false);
        Instance = this;
    }

    public void IncreaseScore(Vector3 atPosition, int by)
    {
        score += by;

        {
            // Anim
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(atPosition);

            TextMeshProUGUI inst = Instantiate(scoreExample, scoreExample.transform.parent);
            inst.transform.position = screenPoint;
            inst.gameObject.SetActive(true);
            inst.alpha = 1f;
            inst.color = color1;
            inst.text = $"+{by}";

            Vector2 target = inst.rectTransform.anchoredPosition + Vector2.up * upTravel;


            DOTween.Sequence()
                .Append(inst.rectTransform.DOPunchScale(Vector3.one * bounceForce, bounceTime))
                .Join(inst.rectTransform.DOAnchorPos(target, fadeTime))
                .Join(inst.DOColor(color2, fadeTime))
                .Append(inst.DOFade(0f, fadeTime))
                .OnComplete(() =>
                {
                    Destroy(inst.gameObject);
                })
                .SetUpdate(true)
                .Play()
            ;
        }
    }

    void Update()
    {
        if (displayedScore < score)
        {
            displayedScore += Time.deltaTime * updateSpeed;
            RefreshScore();
        }
    }

    void RefreshScore()
    {
        scoreText.text = $"SCORE\n{Mathf.FloorToInt(displayedScore)}";
    }
}
