using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Program : MonoBehaviour
{
    Dictionary<string, string> handClass;

    public CardLayout cardLayout;
    // Start is called before the first frame update
    void Start()
    {
        TextAsset mytxtData = (TextAsset)Resources.Load("lookupTable");
        string txt = mytxtData.text;

        // format hands into lookup table
        handClass = new Dictionary<string, string>();
        foreach(var handData in txt.Split('\n'))
        {
            var hands = handData.Split(",");
            for(int i = 1; i < hands.Length; i++)
            {
                handClass[hands[i]] = hands[0];
            }
        }

        Debug.Log("loaded");
        calculateCountsFromDeck(Deck.GetDeck());
        Debug.Log("with counts!");
    }

    // this part crashes on JS.
    public void calculateCountsFromDeck(List<Deck.Card> cards)
    {
        var combos = cards.DifferentCombinations(5);
        var result = new Dictionary<string, int>();
        int sum = 0;
        foreach (var v in combos)
        {
            var list = string.Join("",v.OrderBy(q => q).ToList());
            var test = handClass[list];
            if (!result.ContainsKey(test))
            {
                result.Add(test, 0);
            }
            result[test]++;
            sum++;
        }
        List<Tuple<string, int>> tuples = new List<Tuple<string, int>>();
        // Get all the keys and sort by the value
        var ordered = result.OrderBy(x => x.Value);
        foreach (var v in ordered)
        {
            Debug.Log(v.Key + " " + v.Value + " " + (v.Value / (double)sum));
            tuples.Add(new Tuple<string, int>(v.Key, v.Value));
        }
        cardLayout.setText(tuples, sum);

    }

    // Update is called once per frame
    void Update()
    {
    }
}
