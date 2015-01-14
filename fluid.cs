using System;
using System.IO;

//this will hold all the information about each body
public class Node
{
    object data;
    Node next;

    public Node(object val, Node nxt)
    {
        data = val;
        next = nxt;
    }
    public object Data
    {
        get { return data; }
        set { data = value; }
    }
    public Node Next
    {
        get { return next; }
        set { next = value; }
    }
}

//this is the linked list for the body class
class Body
{
    Node head;
    Node tail;

    public Body()
    {
        head = tail = null;
    }

    public void Append(object obj)
    {
        Node temp = new Node(obj, null);
        if (head == null)
        {
            head = tail = temp;
        }
        else
        {
            tail.Next = temp;
            tail = temp;
        }
    }

    public Node removeSecond()
    {
        if (head.Next == null) return null;
        Node second = head.Next;
        head.Next = head.Next.Next;
        return second;
    }
    public Node returnSecond()
    {
        if (head.Next == null) return null;
        return head.Next;
    }

    public Node returnFirst()
    {
        if (head == null) return null;
        return head;
    }

    public bool isAlmostEmpty()
    {
        return (head.Next == null);
    }
}

class BodyNode
{
    string menu;
    string title;

    public BodyNode(string a, string b)
    {
        menu = a;
        title = b;
    }
    public string Menu
    {
        get { return menu; }
        set { menu = value; }
    }
    public string Title
    {
        get { return title; }
        set { title = value; }
    }
}

class ContentNode
{
    char type;
    string title;
    string content;

    public ContentNode (char t, string a, string b)
    {
        type = t;
        title = a;
        content = b;
    }
    public char Type
    {
        get { return type; }
        set { type = value; }
    }
    public string Title
    {
        get { return title; }
        set { title = value; }
    }
    public string Content
    {
        get { return content; }
        set { content = value; }
    }
}

class Fluid
{
    static void Main()
    {
        string bf = @"body.bd";
        StreamReader bodyFile = new StreamReader(bf);

        //this takes the entire file and turns it into a single string
        string file = null;
        while (!bodyFile.EndOfStream)
        {
            file += bodyFile.ReadLine();
        }
        Console.WriteLine("File successfully read");
        string[] bodies = file.Split(new string[] {">body"}, StringSplitOptions.None);
        Console.WriteLine("File successfully split by {0}", ">body");

        //the first bodies will be empty, so i just shifted everything down 1
        for (int i = 0; i < bodies.Length - 1; i++)
        {
            bodies[i] = bodies[i + 1];
            bodies[i] = bodies[i].Trim();
        }
        bodies[bodies.Length - 1] = null;

        Body[] list = new Body[bodies.Length - 1];

        for (int i = 0; i < bodies.Length - 1; i++)
        {
            //this takes out the body
            string title = null;
            string subtitle = null;
            body(ref bodies[i], out title, out subtitle);
            BodyNode tmp = new BodyNode(title, subtitle);

            //i initialize the linked list before i append stuff to it
            list[i] = new Body();
            list[i].Append(tmp);

            //this takes care of the content of the body
            while (bodies[i] != null)
            {
                if (bodies[i].StartsWith(">content"))
                {
                    string head = null;
                    string stuff = null;
                    content(ref bodies[i], out head, out stuff);
                    ContentNode temp = new ContentNode('c', head, stuff);
                    list[i].Append(temp);
                }
                else if (bodies[i].StartsWith(">picture"))
                {
                    ContentNode temp1 = new ContentNode('p',"",picture(ref bodies[i]));
                    list[i].Append(temp1);
                }
                else
                {
                    break;
                }
            }
            Console.WriteLine("Done reading body " + i);
        }

        //make the index redirect
        BodyNode b = list[0].returnFirst().Data as BodyNode;
        StreamWriter wf = new StreamWriter(@"index.html");

        wf.WriteLine("<html>");
        wf.WriteLine("<head>");
        wf.WriteLine("<META HTTP-EQUIV=\"Refresh\" CONTENT=\"0; URL=/" + b.Menu + "/index.html\">");
        wf.WriteLine("</head>");
        wf.WriteLine("</html>");

        wf.Close();
        //reading the body file
        for (int i = 0; i < bodies.Length - 1; i++)
        {
            //removes the first line
            BodyNode bod = list[i].returnFirst().Data as BodyNode;

            //make a new folder
            string outFile = bod.Menu + @"\";
            Directory.CreateDirectory(outFile);
            //make the index file
            outFile += @"index.html";
            Console.WriteLine("\"{0}\" folder created successfully", bod.Menu);

            StreamWriter writeFile = new StreamWriter(outFile);

            //for reading the template file
            string tf = @"template.tmpl";
            StreamReader tempFile = new StreamReader(tf);

            while (!tempFile.EndOfStream)
            {
                string lin = tempFile.ReadLine();
                string trimlin = lin.Trim();
                if (trimlin == ">navigation<")
                {
                    for (int j = 0; j < bodies.Length - 1; j++)
                    {
                        BodyNode tempBody = list[j].returnFirst().Data as BodyNode;
                        writeFile.Write("                   ");
                        if (i == j)
                        {
                            writeFile.WriteLine("<li class=\"active\"><a href=\"\">" + tempBody.Menu + "</a></li>");
                        }
                        else
                        {
                            writeFile.WriteLine("<li><a href=\"../" + tempBody.Menu + "\">" + tempBody.Menu + "</a></li>");
                        }
                    }
                }
                else if (trimlin == ">content<")
                {
                    writeFile.WriteLine("<p id=\"content-title\">");
                    writeFile.WriteLine("<b>" + bod.Title + "</b>");
                    writeFile.WriteLine("</p>");

                    while (!list[i].isAlmostEmpty())
                    {
                        ContentNode tc = list[i].returnSecond().Data as ContentNode;
                        list[i].removeSecond();
                        
                        if (tc.Type == 'c')
                        {
                            writeFile.WriteLine("<h2>" + tc.Title + "</h2>");
                            writeFile.WriteLine("<p id=\"content-text\">");
                            writeFile.WriteLine(tc.Content);
                            writeFile.WriteLine("</p>");
                        }
                        
                        if (tc.Type == 'p')
                        {
                            writeFile.WriteLine(tc.Content);
                        }
                        
                    }
                }
                else
                {
                    writeFile.WriteLine(lin);
                }
            }
            writeFile.Close();
        }

        Console.WriteLine("done");
    }

    static void body(ref string body, out string first, out string second)
    {
        //finds the first instance of a close bracket
        int i = 0;
        while (body[i] != ')') { i++; }

        //copies the parameters of the body
        string sub = body.Substring(0, i + 1);
        body = body.Replace(sub, "").Trim();

        //removes all the unnecessary stuff
        sub = sub.Replace("(", "").Replace(")", "").Replace("\"", "");

        //assigns the other parameters
        string[] subs = sub.Split(',');
        first = subs[0].Trim();
        second = subs[1].Trim();
    }

    static void content(ref string body, out string first, out string second)
    {
        //finds the end of the content section
        int i = 0;
        while (body[i] != '<') { i++; }

        //copies the content
        string sub = body.Substring(0, i + 1);
        body = body.Replace(sub, "").Trim();

        //removes all the unnecesasry stuff
        sub = sub.Replace(">content(", "").Replace("\"", "").Replace("<","").Trim();

        //assignts the other parameters
        string[] subs = sub.Split(')');
        first = subs[0].Trim();
        second = subs[1].Trim();
    }

    static string picture(ref string body)
    {
        //finds the end of the content section
        int i = 0;
        while (body[i] != '<') { i++; }

        //copies the content
        string sub = body.Substring(0, i + 1);
        body = body.Replace(sub, "").Trim();

        //removes all the unnecessary stuff
        sub = sub.Replace(">picture(", "").Replace("<", "").Trim();
        string[] subs = sub.Split(')');
        return "<img src=\"" + subs[1].Trim() + "\" style=\"" + subs[0].Trim() + ";\"></img>";
    }
}