using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PrecomputeHandValues : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        generateLookup();
    }

    public void generateLookup()
    {
        // generate the deck
        var deck = Deck.GetDeck();
        var combos = deck.DifferentCombinations(5);

        var dictFuckUnity = new Dictionary<string, List<string>>();

        foreach (var v in combos)
        {
            var list = string.Join("", v.OrderBy(q => q).ToList());
            var val = Deck.EvaluateHand(v.ToList());

            if (!dictFuckUnity.ContainsKey(val))
            {
                dictFuckUnity[val] = new List<string>();
            }
            dictFuckUnity[val].Add(list);
        }
        Debug.Log(dictFuckUnity.Values.Count);


        // convert to a list because it's 2024 and unity can't serialize a fucking dictionary
        using (StreamWriter writetext = new StreamWriter("Assets/Resources/lookupTable.csv"))
        {
            foreach (var v in dictFuckUnity.Keys)
            {
                var list = dictFuckUnity[v];
                writetext.Write(v + ",");
                foreach (var k in list)
                {
                    writetext.Write(k + ",");
                }
                writetext.Write('\n');
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

public static class Ex
{
    public static IEnumerable<IEnumerable<T>> DifferentCombinations<T>(this IEnumerable<T> elements, int k)
    {
        return k == 0 ? new[] { new T[0] } :
          elements.SelectMany((e, i) =>
            elements.Skip(i + 1).DifferentCombinations(k - 1).Select(c => (new[] { e }).Concat(c)));
    }
}