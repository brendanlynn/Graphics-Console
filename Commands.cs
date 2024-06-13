using System.Text;
using static GraphicsConsole.Interpretation;
using static System.Console;
namespace GraphicsConsole;
public static class Commands
{
    public static bool New(ref Bitmap? Bitmap, ref Graphics? Graphics, ref Size? Size, Span<string> Tokens)
    {
        switch (Tokens.Length)
        {
            case 2:
                if (GetInt32(Tokens[0], out int? width0) && GetInt32(Tokens[1], out int? height0))
                {
                    Bitmap?.Dispose();
                    Graphics?.Dispose();
                    Size = new Size((int)width0!, (int)height0!);
                    Bitmap = new(((Size)Size!).Width, ((Size)Size!).Height);
                    Graphics = Graphics.FromImage(Bitmap);
                }
                else
                    return false;
                return true;
            case 3:
                if (GetInt32(Tokens[0], out int? width1) && GetInt32(Tokens[1], out int? height1) && GetColor(Tokens[2], out Color? color))
                {
                    Bitmap?.Dispose();
                    Graphics?.Dispose();
                    Size = new Size((int)width1!, (int)height1!);
                    Bitmap = new(((Size)Size!).Width, ((Size)Size!).Height);
                    Graphics = Graphics.FromImage(Bitmap);
                    Graphics.Clear((Color)color!);
                }
                else
                    return false;
                return true;
            default:
                return false;
        }
    }
    public static bool Fill(ref Graphics? Graphics, Span<string> Tokens)
    {
        if (Graphics is null)
        {
            BitNotNull();
            return true;
        }
        if (Tokens.Length != 1)
            return false;
        if (GetColor(Tokens[0], out Color? color))
        {
            Graphics!.Clear((Color)color!);
            return true;
        }
        return false;
    }
    public static bool Wipe(ref Graphics? Graphics, Span<string> Tokens)
    {
        if (Graphics is null)
        {
            BitNotNull();
            return true;
        }
        if (Tokens.Length != 0)
            return false;
        Graphics!.Clear(Color.Transparent);
        return true;
    }
    public static bool DrawRect(ref Graphics? Graphics, ref Size? Size, Span<string> Tokens)
    {
        if (Graphics is null)
        {
            BitNotNull();
            return true;
        }
        if (Tokens.Length != 6)
            return false;
        if (GetColor(Tokens[0], out Color? color)
            && GetInt32(Tokens[1], out int? lineWidth, (Size)Size!)
            && GetInt32(Tokens[2], out int? x, (Size)Size!)
            && GetInt32(Tokens[3], out int? y, (Size)Size!)
            && GetInt32(Tokens[4], out int? width, (Size)Size!)
            && GetInt32(Tokens[5], out int? height, (Size)Size!))
        {
            Graphics!.DrawRectangle(new Pen(new SolidBrush((Color)color!), (int)lineWidth!), (int)x!, (int)y!, (int)width!, (int)height!);
            return true;
        }
        return false;
    }
    public static bool DrawElps(ref Graphics? Graphics, ref Size? Size, Span<string> Tokens)
    {
        if (Graphics is null)
        {
            BitNotNull();
            return true;
        }
        if (Tokens.Length != 6)
            return false;
        if (GetColor(Tokens[0], out Color? color)
            && GetInt32(Tokens[1], out int? lineWidth, (Size)Size!)
            && GetInt32(Tokens[2], out int? x, (Size)Size!)
            && GetInt32(Tokens[3], out int? y, (Size)Size!)
            && GetInt32(Tokens[4], out int? width, (Size)Size!)
            && GetInt32(Tokens[5], out int? height, (Size)Size!))
        {
            Graphics!.DrawEllipse(new Pen(new SolidBrush((Color)color!), (int)lineWidth!), (int)x!, (int)y!, (int)width!, (int)height!);
            return true;
        }
        return false;
    }
    public static bool DrawLine(ref Graphics? Graphics, ref Size? Size, Span<string> Tokens)
    {
        if (Graphics is null)
        {
            BitNotNull();
            return true;
        }
        if (Tokens.Length != 6)
            return false;
        if (GetColor(Tokens[0], out Color? color)
            && GetInt32(Tokens[1], out int? lineWidth, (Size)Size!)
            && GetInt32(Tokens[2], out int? x0, (Size)Size!)
            && GetInt32(Tokens[3], out int? y0, (Size)Size!)
            && GetInt32(Tokens[4], out int? x1, (Size)Size!)
            && GetInt32(Tokens[5], out int? y1, (Size)Size!))
        {
            Graphics!.DrawLine(new Pen(new SolidBrush((Color)color!), (int)lineWidth!), (int)x0!, (int)y0!, (int)x1!, (int)y1!);
            return true;
        }
        return false;
    }
    public static bool DrawPoly(ref Graphics? Graphics, ref Size? Size, Span<string> Tokens)
    {
        if (Graphics is null)
        {
            BitNotNull();
            return true;
        }
        if (Tokens.Length > 0 && (Tokens.Length & 1) != 0)
            return false;
        if (GetColor(Tokens[0], out Color? color)
            && GetInt32(Tokens[1], out int? lineWidth, (Size)Size!))
        {
            Point[] points = new Point[(Tokens.Length >> 1) - 1];
            for (int i = 0; i < points.Length; i++)
            {
                int index = i << 1;
                if (GetInt32(Tokens[index + 2], out int? x, (Size)Size!)
                    && GetInt32(Tokens[index + 3], out int? y, (Size)Size!))
                    points[i] = new((int)x!, (int)y!);
                else
                    return false;
            }
            Graphics!.DrawPolygon(new Pen(new SolidBrush((Color)color!), (int)lineWidth!), points);
            return true;
        }
        return false;
    }
    public static bool FillRect(ref Graphics? Graphics, ref Size? Size, Span<string> Tokens)
    {
        if (Graphics is null)
        {
            BitNotNull();
            return true;
        }
        if (Tokens.Length != 5)
            return false;
        if (GetColor(Tokens[0], out Color? color)
            && GetInt32(Tokens[1], out int? x, (Size)Size!)
            && GetInt32(Tokens[2], out int? y, (Size)Size!)
            && GetInt32(Tokens[3], out int? width, (Size)Size!)
            && GetInt32(Tokens[4], out int? height, (Size)Size!))
        {
            Graphics!.FillRectangle(new SolidBrush((Color)color!), (int)x!, (int)y!, (int)width!, (int)height!);
            return true;
        }
        return false;
    }
    public static bool FillElps(ref Graphics? Graphics, ref Size? Size, Span<string> Tokens)
    {
        if (Graphics is null)
        {
            BitNotNull();
            return true;
        }
        if (Tokens.Length != 5)
            return false;
        if (GetColor(Tokens[0], out Color? color)
            && GetInt32(Tokens[1], out int? x, (Size)Size!)
            && GetInt32(Tokens[2], out int? y, (Size)Size!)
            && GetInt32(Tokens[3], out int? width, (Size)Size!)
            && GetInt32(Tokens[4], out int? height, (Size)Size!))
        {
            Graphics!.FillEllipse(new SolidBrush((Color)color!), (int)x!, (int)y!, (int)width!, (int)height!);
            return true;
        }
        return false;
    }
    public static bool FillPoly(ref Graphics? Graphics, ref Size? Size, Span<string> Tokens)
    {
        if (Graphics is null)
        {
            BitNotNull();
            return true;
        }
        if ((Tokens.Length & 1) == 0)
            return false;
        if (GetColor(Tokens[0], out Color? color))
        {
            Point[] points = new Point[Tokens.Length >> 1];
            for (int i = 0; i < points.Length; i++)
            {
                int index = i << 1;
                if (GetInt32(Tokens[index + 1], out int? x, (Size)Size!)
                    && GetInt32(Tokens[index + 2], out int? y, (Size)Size!))
                    points[i] = new((int)x!, (int)y!);
                else
                    return false;
            }
            Graphics!.FillPolygon(new SolidBrush((Color)color!), points);
            return true;
        }
        return false;
    }
    public static bool Save(ref Bitmap? Bitmap, Span<string> Tokens)
    {
        if (Bitmap is null)
        {
            BitNotNull();
            return true;
        }
        if (Tokens.Length != 1)
            return false;
        try
        {
            Bitmap.Save(Tokens[0]);
            WriteLine("Saved successfully.");
        }
        catch
        {
            WriteLine("An error occured in saving.");
        }
        return true;
    }
    public static bool BackDrop(Span<string> Tokens)
    {
        if (Tokens.Length != 1)
            return false;
        if (!GetColor(Tokens[0], out Color? color))
            return false;
        Program.BackDrop = (Color)color!;
        return true;
    }
    public static bool Help(Span<string> Tokens)
    {
        switch (Tokens.Length)
        {
            case 0:
                WriteLine("Be aware that all commands are case-sensitive.");
                WriteLine("Here is a list of all possible commands:");
                foreach (string key in Documentation.Keys)
                {
                    WriteLine($"  - {key}:");
                    PrintCommand(key, 8);
                }
                return true;
            case 1:
                string? name = GetDictKey(Tokens[0]);
                if (name is null)
                {
                    WriteLine("Command does not exist.");
                    return true;
                }
                WriteLine($"{name}:");
                PrintCommand(name, 4);
                return true;
            default:
                return false;
        }
    }
    public static void PrintCommand(string Name, int WhiteSpace)
    {
        (string description, string[] aliases, string?[][] syntax) = Documentation[Name];
        string whiteSpaceString = new(' ', WhiteSpace);
        WriteLine($"{whiteSpaceString}Description: {description}");
        WriteLine($"{whiteSpaceString}Aliases: {string.Join(", ", aliases)}");
        if (syntax.Length == 1)
            WriteLine($"{whiteSpaceString}Syntax: {aliases[0]}{GetSyntax(syntax[0])}");
        else
        {
            WriteLine($"{whiteSpaceString}Syntax:");
            foreach (string?[] overloadSyntax in syntax)
                WriteLine($"{whiteSpaceString}    {aliases[0]}{GetSyntax(overloadSyntax)}");
        }
    }
    public static string? GetDictKey(string Alias)
    {
        foreach (string key in Documentation.Keys)
        {
            if (Documentation[key].Aliases.Contains(Alias))
                return key;
        }
        return null;
    }
    public static string GetSyntax(string?[] Syntax)
    {
        StringBuilder sb = new();
        foreach (string? value in Syntax)
            if (value is null)
                _ = sb.Append(" ...");
            else
            {
                _ = sb.Append(" <");
                _ = sb.Append(value);
                _ = sb.Append('>');
            }
        return sb.ToString();
    }
    public static void BitNotNull()
        => WriteLine("Bitmap cannot be null.");
}