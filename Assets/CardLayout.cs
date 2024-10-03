using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardLayout : MonoBehaviour
{

    public Sprite[] cards;
    public GameObject cardButton;

    public TMP_Text leftText;
    public TMP_Text middleText;
    public TMP_Text rightText;

    private Dictionary<int, Image> buttons = new Dictionary<int, Image>();
    private HashSet<int> selected = new HashSet<int>();

    public Program program;

    // Start is called before the first frame update
    void Start()
    {
        for (int rank = 0; rank < 13; rank++)
        {
            for (int suit = 0; suit < 4; suit++)
            {
                var v = GameObject.Instantiate(cardButton);
                v.transform.SetParent(this.transform);
                v.transform.localPosition = new Vector3(rank * 37 * 1.5f, suit * 52 * 1.5f, 0);
                v.GetComponent<UnityEngine.UI.Image>().sprite = cards[rank + (13*suit)];
                int val = rank + 13 * suit;
                buttons[val] = v.GetComponent<UnityEngine.UI.Image>();
                v.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { this.cardToggle(val); });

            }
        }
    }

    public void cardToggle(int t)
    {
        buttons[t].color = buttons[t].color == Color.white ? Color.gray : Color.white;
        if (selected.Contains(t))
        {
            selected.Remove(t);
        }
        else
        {
            selected.Add(t);
        }
    }

    public void compute()
    {
        // get all the cards in the deck
        var c = Deck.GetDeck();
        foreach (var card in selected)
        {
            c.RemoveAll(x => ((int)x.Rank == (2+(card%13)) && (int)x.Suit == (int)(card/13)));
        }
        program.calculateCountsFromDeck(c);

    }

    public void setText(List<Tuple<string, int>> t, int sum)
    {
        leftText.text = "";
        middleText.text = "";
        rightText.text = "";

        foreach(var v in t)
        {
            leftText.text += v.Item1 + "\n";
            middleText.text += ((100 * v.Item2) / (double)sum).ToString("##.####") + "%\n";
            rightText.text += v.Item2 + "\n";
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
