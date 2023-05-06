using PlayerIOClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Transform cardSpawnPosition;
    public CardIdentity cardPrefab;
    private List<CardIdentity> cardPrefabs;
    private void Start()
    {
        cardPrefabs = new List<CardIdentity>();
    }
    public void SpawnCards(Message message)
    {
        int difference = -50;
        for (uint i = 0; i < message.Count; i++)
        {
            string card = message.GetString(i);

            CardIdentity identity = Instantiate(cardPrefab , cardSpawnPosition);
            identity.name = card;
            identity.SetCardFace(card);

            cardPrefabs.Add(identity);

            Vector3 position = identity.GetComponent<RectTransform>().localPosition;
            identity.GetComponent<RectTransform>().localPosition = new Vector3(position.x + difference + 50 * i, position.y, position.z);



        }
    }
    public void DestroyCards()
    {
        for (int i = 0; i < cardPrefabs.Count; i++)
        {
            Destroy(cardPrefabs[i].gameObject);
        }
        cardPrefabs.Clear();
    }
}
