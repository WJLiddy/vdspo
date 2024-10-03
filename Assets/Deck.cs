using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck
{
    public enum Suit
    {
        Clubs = 0,
        Diamonds,
        Hearts,
        Spades
    }

    public enum Rank
    {
        Two = 2,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }

    public class Card : IComparable
    {
        public Suit Suit { get; }
        public Rank Rank { get; }
        public Card(Suit suit, Rank rank)
        {
            Suit = suit;
            Rank = rank;
        }
        public int GetMinRank()
        {
            if (Rank == Rank.Ace) return 1;
            return (int)Rank;
        }

        public int CompareTo(object obj)
        {
            if(!(obj is Card)) return -1;
            return Suit == ((Card)(obj)).Suit ? Rank - ((Card)(obj)).Rank : Suit - ((Card)(obj)).Suit;
        }

        public override string ToString()
        {
            return "" + (char)(65 + (((int)Suit) * 13) + (Rank-2));
        }
    }

    public static string EvaluateHand(List<Card> hand)
    {
        var groupByRank = hand.GroupBy(card => card.Rank);

        if (IsRoyalFlush(hand)) return "RoyalFlush";
        if (IsStraightFlush(hand)) return "StraightFlush";
        if (IsFourOfAKind(groupByRank)) return "FourOfAKind";
        if (IsFullHouse(groupByRank)) return "FullHouse";
        if (IsFlush(hand)) return "Flush";
        if (IsStraight(hand)) return "Straight";
        if (IsThreeOfAKind(groupByRank)) return "ThreeOfAKind";
        if (IsTwoPair(groupByRank)) return "TwoPair";
        if (IsOnePair(groupByRank)) return "OnePair";
        return "HighCard";
    }

    private static bool IsRoyalFlush(IReadOnlyCollection<Card> hand)
    {
        return IsStraightFlush(hand) && hand.All(card => card.Rank >= Rank.Ten);
    }

    private static bool IsStraightFlush(IReadOnlyCollection<Card> hand)
    {
        return IsFlush(hand) && IsStraight(hand);
    }

    private static bool IsFlush(IReadOnlyCollection<Card> hand)
    {
        return hand.GroupBy(card => card.Suit).Count() == 1;
    }

    private static bool IsStraight(IReadOnlyCollection<Card> hand)
    {
        var maxRanks = hand.Select(card => (int)card.Rank).OrderBy(rank => rank).ToList();
        if (AscendingOrder(maxRanks)) return true;

        var minRanks = hand.Select(card => (int)card.GetMinRank()).OrderBy(rank => rank).ToList();
        return AscendingOrder(minRanks);
    }

    private static bool AscendingOrder(IList<int> ranks)
    {
        for (int i = 1; i < ranks.Count; i++)
        {
            if (ranks[i] != ranks[i - 1] + 1)
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsFourOfAKind(IEnumerable<IGrouping<Rank, Card>> hand)
    {
        return CountByRank(hand, 4) == 1;
    }

    private static bool IsThreeOfAKind(IEnumerable<IGrouping<Rank, Card>> hand)
    {
        return CountByRank(hand, 3) == 1;
    }

    private static bool IsTwoPair(IEnumerable<IGrouping<Rank, Card>> hand)
    {
        return CountByRank(hand, 2) == 2;
    }

    private static bool IsOnePair(IEnumerable<IGrouping<Rank, Card>> hand)
    {
        return CountByRank(hand, 2) == 1;
    }

    private static bool IsFullHouse(IEnumerable<IGrouping<Rank, Card>> hand)
    {
        return CountByRank(hand, 3) == 1 && CountByRank(hand, 2) == 1;
    }

    private static int CountByRank(IEnumerable<IGrouping<Rank, Card>> hand, int count)
    {
        return hand.Count(group => group.Count() == count);
    }
    public static void Shuffle<T>(IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static List<Card> GetDeck()
    {
        var deck = new List<Card>();
        foreach (Suit suit in System.Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in System.Enum.GetValues(typeof(Rank)))
            {
                deck.Add(new Card(suit, rank));
            }
        }
        Shuffle(deck);
        return deck;
    }
}