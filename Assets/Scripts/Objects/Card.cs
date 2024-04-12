using Controllers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Sprite faceSprite;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private Image currentImage;

    public int cardId;
    public bool allowFlip = true;
    public bool isDone;

    private bool _isFlipped;

    public void OnClick()
    {
        Debug.Log($"{cardId} {isDone}");

        if (!isDone)
            OnCardClicked();
    }

    private void OnCardClicked()
    {
        if (PairGameController.Instance.CanReveal && !_isFlipped)
        {
            if (allowFlip) FlipCard();
            PairGameController.Instance.CardRevealed(this);
        }
    }

    private void FlipCard()
    {
        // allowFlip = false;
        // if (!_isFlipped)
        //     // for (var i = 0f; i <= 180f; i += 30f)
        //     // {
        //     //     transform.rotation = Quaternion.Euler(0f, i, 0f);
        //     //     if (Math.Abs(i - 90f) < 0.1)
        //     //     {
        //     //         Debug.Log($"{cardId} {i}");
        //     //         currentImage.sprite = backSprite;
        //     //     }
        //     //     
        //     //
        //     //     yield return new WaitForSeconds(0.001f);
        //     // }
        //     transform.DORotate(new Vector3(0, 90, 0), 0.25f).OnComplete(() =>
        //     {
        //         currentImage.sprite = backSprite;
        //         transform.DORotate(new Vector3(0, 0, 0), 0.25f).OnComplete(() => { });
        //     });
        //
        // else
        //     transform.DORotate(new Vector3(0, 90, 0), 0.25f).OnComplete(() =>
        //     {
        //         currentImage.sprite = faceSprite;
        //         transform.DORotate(new Vector3(0, 180, 0), 0.25f);
        //     });
        //
        // allowFlip = true;
        // _isFlipped = !_isFlipped;

        allowFlip = false;

        var endRotation = _isFlipped ? Vector3.zero : Vector3.up * 180;

        transform.DORotate(endRotation, 0.5f)
            .OnStart(() => { })
            .OnUpdate(() =>
            {
                currentImage.sprite = transform.eulerAngles.y is > 90 and < 270 ? backSprite : faceSprite;
            })
            .OnComplete(() =>
            {
                _isFlipped = !_isFlipped;
                allowFlip = true;
            });
    }

    public void UnReveal()
    {
        FlipCard();
    }

    public void Done()
    {
        isDone = true;
    }

    public void SetCard(int id, Sprite mainSprite)
    {
        cardId = id;
        currentImage.sprite = faceSprite;
        backSprite = mainSprite;
    }
}