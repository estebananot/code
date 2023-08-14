using System;
using System.Collections.Generic;

class Program
{
    static Random rand = new Random();

    class Node
    {
        public int val, size, pri;
        public Node l, r;
        
        public void Mconcat()
        {
            size = Size(l) + 1 + Size(r);
        }
    }

    static int Size(Node x)
    {
        return x != null ? x.size : 0;
    }

    static Node Join(Node x, Node y)
    {
        if (x == null) return y;
        if (y == null) return x;
        if (x.pri < y.pri)
        {
            x.r = Join(x.r, y);
            x.Mconcat();
            return x;
        }
        else
        {
            y.l = Join(x, y.l);
            y.Mconcat();
            return y;
        }
    }

    static void Split(Node x, int k, out Node l, out Node r)
    {
        if (x == null)
        {
            l = r = null;
        }
        else
        {
            int c = Size(x.l) + 1;
            if (k < c)
            {
                Split(x.l, k, out l, out x.l);
                r = x;
            }
            else
            {
                Split(x.r, k - c, out x.r, out r);
                l = x;
            }
            x.Mconcat();
        }
    }

    static Node Extract(ref Node x, int from, int to)
    {
        Node l, m, r;
        Split(x, to, out m, out r);
        Split(m, from, out l, out m);
        x = Join(l, r);
        return m;
    }

    static void Inorder(Node x)
    {
        List<int> a = new List<int>();
        Stack<Node> s = new Stack<Node>();
        while (true)
        {
            while (x != null)
            {
                s.Push(x);
                x = x.l;
            }
            if (s.Count == 0) break;
            x = s.Pop();
            a.Add(x.val);
            x = x.r;
        }
        Console.WriteLine(Math.Abs(a[0] - a[a.Count - 1]));
        foreach (int val in a)
        {
            Console.Write(val + " ");
        }
        Console.WriteLine();
    }

    static void Main(string[] args)
    {
        string[] nm = Console.ReadLine().Split();
        int n = int.Parse(nm[0]);
        int m = int.Parse(nm[1]);
        Node rt = null;
        for (int i = 0; i < n; i++)
        {
            string[] valPri = Console.ReadLine().Split();
            int val = int.Parse(valPri[0]);
            Node newNode = new Node
            {
                val = val,
                pri = rand.Next(),
                size = 1
            };
            rt = Join(rt, newNode);
        }
        while (m-- > 0)
        {
            string[] opLr = Console.ReadLine().Split();
            int op = int.Parse(opLr[0]);
            int l = int.Parse(opLr[1]) - 1;
            int r = int.Parse(opLr[2]);
            Node sub = Extract(ref rt, l, r);
            rt = op == 1 ? Join(sub, rt) : Join(rt, sub);
        }
        Inorder(rt);
    }
}
