using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Controllers
{
    public class PairGameController : Singleton<PairGameController>
    {
        [SerializeField] private Card cardPrefab;
        [SerializeField] private Transform cardsParent;
        [SerializeField] private GameObject completedLevelPanel;

        private Card _firstRevealed;
        private List<CardDatatype> _listCardData;
        private int _numOfPairedMatched;
        private Card _secondRevealed;

        public bool CanReveal => _secondRevealed == null;

        public void BackToHome()
        {
            GameManager.Instance.BackToHome();
        }

        public void Next()
        {
            InitLevel(_listCardData);
        }

        public void InitLevel(List<CardDatatype> listCardData)
        {
            foreach (Transform child in cardsParent.transform) Destroy(child.gameObject);
            completedLevelPanel.SetActive(false);
            _numOfPairedMatched = 0;
            _listCardData = listCardData;
            if (_listCardData == null) return;

            var cardIndexes = new List<int>();
            for (var i = 0; i < _listCardData.Count; i++)
            {
                cardIndexes.Add(i);
                cardIndexes.Add(i);
            }

            cardIndexes = ShuffleList(cardIndexes);
            foreach (var t in cardIndexes)
            {
                var newCard = Instantiate(cardPrefab, cardsParent);
                newCard.SetCard(_listCardData[t].id, _listCardData[t].image);
            }
        }

        private static List<int> ShuffleList(List<int> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var temp = list[i];
                var randomIndex = Random.Range(i, list.Count);
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }

            return list;
        }

        public void CardRevealed(Card card)
        {
            if (_firstRevealed == null)
            {
                _firstRevealed = card;
            }
            else
            {
                _secondRevealed = card;
                StartCoroutine(CheckMatch());
            }
        }

        private IEnumerator CheckMatch()
        {
            yield return new WaitForSeconds(0.5f);

            if (_firstRevealed.cardId == _secondRevealed.cardId)
            {
                HandlePairMatched();
            }
            else
            {
                _firstRevealed.UnReveal();
                _secondRevealed.UnReveal();
            }

            _firstRevealed = null;
            _secondRevealed = null;
        }

        private void HandlePairMatched()
        {
            _firstRevealed.Done();
            _secondRevealed.Done();

            _firstRevealed.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => { });
            _secondRevealed.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => { });

            _numOfPairedMatched++;

            CheckCompletedLevel();
        }

        private void CheckCompletedLevel()
        {
            if (_numOfPairedMatched == _listCardData.Count) completedLevelPanel.SetActive(true);
        }
    }
}