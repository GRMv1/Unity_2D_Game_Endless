using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreElement : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI NumberTxt;
    [SerializeField]
    private TMPro.TextMeshProUGUI TimeTxt;
    [SerializeField]
    private TMPro.TextMeshProUGUI ScoreTxt;
    [SerializeField]
    private TMPro.TextMeshProUGUI HitCountTxt;

    public void SetValues(int number, int time, int score, int hitCount)
    {
        NumberTxt.text = number.ToString();
        TimeTxt.text = time.ToString();
        ScoreTxt.text = score.ToString();
        HitCountTxt.text = hitCount.ToString();
    }
}
