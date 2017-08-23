using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class XmlParser : IDisposable
{
    public TextReader Stream { get; private set; }
    public string Text { get; private set; }
    public string Tag { get; private set; }

    public XmlParser(TextReader stream) { Stream = stream; }
    public XmlParser(string source) : this(new StringReader(source)) { }

    public void Dispose()
    {
        if (Stream != null) Stream.Dispose();
        Stream = null;
    }

    private Dictionary<string, string> values = new Dictionary<string, string>();

    public string this[string key]
    {
        get
        {
            string ret;
            if (values.TryGetValue(key, out ret)) return ret;
            return null;
        }
    }

    private string reserved;

    public bool Read()
    {
        Text = Tag = null;
        values.Clear();
        if (Stream == null) return false;

        if (reserved != null)
        {
            Tag = reserved;
            reserved = null;
        }
        else
            ReadText();
        return true;
    }

    public void Search(string tag, Func<bool> f, Action a)
    {
        while (Read())
        {
            if (Tag == tag && f())
            {
                a();
                break;
            }
        }
    }

    public void SearchEach(string tag, Func<bool> f, Action a)
    {
        var end = "/" + Tag;
        while (Read())
        {
            if (Tag == end)
                break;
            else if (Tag == tag && f())
                a();
        }
    }

    private int current;

    private int ReadChar()
    {
        if (Stream == null) return current = -1;
        current = Stream.Read();
        if (current == -1) Dispose();
        return current;
    }

    private void ReadText()
    {
        var text = new StringBuilder();
        int ch;
        while ((ch = ReadChar()) != -1)
        {
            if (ch == '<') break;
            text.Append((char)ch);
        }
        Text = FromEntity(text.ToString());
        if (ch == '<') ReadTag();
    }

    private void ReadTag()
    {
        int ch;
        var tag = new StringBuilder();
        while ((ch = ReadChar()) != -1)
        {
            if (ch == '>' || (ch == '/' && tag.Length > 0))
                break;
            else if (ch > ' ')
            {
                tag.Append((char)ch);
                if (tag.Length == 3 && tag.ToString() == "!--") break;
            }
            else if (tag.Length > 0)
                break;
        }
        Tag = tag.ToString().ToLower();
        if (ch == '/')
        {
            reserved = "/" + Tag;
            ch = ReadChar();
        }
        if (ch != '>')
        {
            if (Tag == "!--")
                ReadComment();
            else
                while (ReadAttribute()) ;
        }
    }

    private void ReadComment()
    {
        int ch, m = 0;
        var comment = new StringBuilder();
        while ((ch = ReadChar()) != -1)
        {
            if (ch == '>' && m >= 2)
            {
                comment.Length -= 2;
                break;
            }
            comment.Append((char)ch);
            if (ch == '-') m++; else m = 0;
        }
        values["comment"] = comment.ToString();
    }

    private bool ReadAttribute()
    {
        string name = null;
        do
        {
            var n = ReadValue();
            if (current == '>')
                return false;
            else if (current == '/')
                reserved = "/" + Tag;
            if (n != "") name = n;
        } while (current != '=');
        var value = ReadValue();
        if (name != null) values[name] = value;
        return current != '>';
    }

    private string ReadValue()
    {
        int ch;
        var value = new StringBuilder();
        while ((ch = ReadChar()) != -1)
        {
            if (ch == '>' || ch == '=' || ch == '/')
                break;
            else if (ch == '"')
            {
                while ((ch = ReadChar()) != -1)
                {
                    if (ch == '"') break;
                    value.Append((char)ch);
                }
                break;
            }
            else if (ch > ' ')
                value.Append((char)ch);
            else if (value.Length > 0)
                break;
        }
        return value.ToString();
    }

    public static string FromEntity(string s)
    {
        return s
            .Replace("&lt;", "<")
            .Replace("&gt;", ">")
            .Replace("&quot;", "\"")
            .Replace("&nbsp;", " ")
            .Replace("&amp;", "&");
    }
}


