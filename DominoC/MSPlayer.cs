using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominoC
{
    class MSPlayer
    {
        static public string PlayerName = "Бывалый";
        static private List<MTable.SBone> lHand;


        //=== Готовые функции =================
        // инициализация игрока
        static public void Initialize()
        {
            lHand = new List<MTable.SBone>();
        }

        // Вывод на экран
        static public void PrintAll()
        { MTable.PrintAll(lHand); }

        // дать количество доминушек
        static public int GetCount()
        { return lHand.Count; }

        //=== Функции для разработки =================
        // добавить доминушку в свою руку
        static public void AddItem(MTable.SBone sb)
        {
            lHand.Add(sb);
        }

        // дать сумму очков на руке
        static public int GetScore()
        {
            int _score = 0;
            if (lHand.Count == 1)
            {
                if (lHand[0].First == 0 && lHand[0].Second == 0)
                    return 25;
            }
            foreach (MTable.SBone bone in lHand)
                _score += bone.First + bone.Second;
            return _score;
        }

        // сделать ход
        static public bool MakeStep(out MTable.SBone sb, out bool End)
        {
            List<MTable.SBone> gameCollection = MTable.GetGameCollection();
            MTable.SBone firstBone = gameCollection[0];
            MTable.SBone lastBone = gameCollection[gameCollection.Count - 1];
            List<MTable.SBone> doubles = new List<MTable.SBone>();
            foreach (MTable.SBone bone in lHand)
            {
                if (bone.First == bone.Second)
                {
                    doubles.Add(bone);
                }
            }
            for (int i = 0; i < doubles.Count; i++)
            {
                if (firstBone.First == doubles[i].First)
                {
                    sb = doubles[i];
                    lHand.Remove(doubles[i]);
                    doubles.RemoveAt(i);
                    End = false;
                    return true;
                }
                if (lastBone.Second == doubles[i].Second)
                {
                    sb = doubles[i];
                    lHand.Remove(doubles[i]);
                    doubles.RemoveAt(i);
                    End = true;
                    return true;
                }
            }
            Dictionary<MTable.SBone, int> possibleSteps = new Dictionary<MTable.SBone, int>();
            sb = lHand[0];
            for (int i = 0; i < lHand.Count; i++)
            {
                if (lHand[i].Second == firstBone.First)
                {
                    try
                    {
                        possibleSteps.Add(lHand[i], CountSteps(lHand[i].First));
                    }
                    catch { }
                }
                else if (lHand[i].First == firstBone.First)
                {
                    lHand[i].Exchange();
                    try
                    {
                        possibleSteps.Add(lHand[i], CountSteps(lHand[i].First));
                    }
                    catch { }
                }
                if (lHand[i].First == lastBone.Second)
                {
                    try
                    {
                        possibleSteps.Add(lHand[i], CountSteps(lHand[i].Second));
                    }
                    catch { }
                }
                else if (lHand[i].Second == lastBone.Second)
                {
                    lHand[i].Exchange();
                    try
                    {
                        possibleSteps.Add(lHand[i], CountSteps(lHand[i].Second));
                    }
                    catch { }
                }
            }
            int maxSteps = 0;
            foreach (var pair in possibleSteps)
            {
                if (pair.Value > maxSteps)
                {
                    maxSteps = pair.Value;
                    sb = pair.Key;
                }
            }
            if (sb.Second == firstBone.First)
            {
                lHand.Remove(sb);
                End = false;
                return true;
            }
            else if (sb.First == firstBone.First)
            {
                int index = lHand.IndexOf(sb);
                sb.Exchange();
                lHand.RemoveAt(index);
                End = false;
                return true;
            }
            if (sb.First == lastBone.Second)
            {
                lHand.Remove(sb);
                End = true;
                return true;
            }
            else if (sb.Second == lastBone.Second)
            {
                int index = lHand.IndexOf(sb);
                sb.Exchange();
                lHand.RemoveAt(index);
                End = true;
                return true;
            }

            while (MTable.GetFromShop(out MTable.SBone newBone))
            {
                if (newBone.Second == firstBone.First || newBone.First == firstBone.First)
                {
                    sb = newBone;
                    End = false;
                    return true;
                }
                if (newBone.Second == lastBone.Second || newBone.First == lastBone.Second)
                {
                    sb = newBone;
                    End = true;
                    return true;
                }
                lHand.Add(newBone);
            }

            sb.First = 0;
            sb.Second = 0;
            End = true;
            return false;
        }
        public static int CountSteps(int target)
        {
            int res = -1;
            for (int i = 0; i < lHand.Count; i++)
            {
                if (lHand[i].First == target || lHand[i].Second == target)
                    res++;
            }
            return res;
        }
    }
}
