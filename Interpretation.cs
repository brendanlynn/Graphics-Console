using System.Text;
using static GraphicsConsole.Commands;
using static System.Console;
namespace GraphicsConsole;
public static class Interpretation
{
    public static bool Interpret(ref Bitmap? Bitmap, ref Graphics? Graphics, ref Size? Size, string Command)
    {
        string[] tokens = Tokenize(Command);
        if (tokens.Length == 0)
        {
            InvalidCommand();
            return true;
        }
        string token = tokens[0];
        Span<string> nextTokens = new(tokens, 1, tokens.Length - 1);
        switch (token)
        {
            case "exit" or "ex" or "x":
                return false;
            case "clear" or "cl" or "c":
                Clear();
                return true;
            case "save" or "sv" or "s":
                ResolveCommand(Save(ref Bitmap, nextTokens));
                return true;
            case "new" or "nw" or "n":
                ResolveCommand(New(ref Bitmap, ref Graphics, ref Size, nextTokens));
                return true;
            case "wipe" or "wp" or "w":
                ResolveCommand(Wipe(ref Graphics, nextTokens));
                return true;
            case "drawrect" or "drawrectangle" or "dr":
                ResolveCommand(DrawRect(ref Graphics, ref Size, nextTokens));
                return true;
            case "drawelps" or "drawellipse" or "de":
                ResolveCommand(DrawElps(ref Graphics, ref Size, nextTokens));
                return true;
            case "drawline" or "dl":
                ResolveCommand(DrawLine(ref Graphics, ref Size, nextTokens));
                return true;
            case "drawpoly" or "drawpolygon" or "dp":
                ResolveCommand(DrawPoly(ref Graphics, ref Size, nextTokens));
                return true;
            case "fill" or "fi" or "f":
                ResolveCommand(Fill(ref Graphics, nextTokens));
                return true;
            case "fillrect" or "fillrectangle" or "fr":
                ResolveCommand(FillRect(ref Graphics, ref Size, nextTokens));
                return true;
            case "fillelps" or "fillellipse" or "fe":
                ResolveCommand(FillElps(ref Graphics, ref Size, nextTokens));
                return true;
            case "fillpoly" or "fillpolygon" or "fp":
                ResolveCommand(FillPoly(ref Graphics, ref Size, nextTokens));
                return true;
            case "setbackdrop" or "backdrop" or "setback" or "back" or "setdrop" or "drop" or "sbd" or "bd" or "sb" or "b" or "sd" or "d":
                ResolveCommand(BackDrop(nextTokens));
                return true;
            case "help" or "hlp" or "hp" or "h":
                ResolveCommand(Help(nextTokens));
                return true;
            default:
                InvalidCommand();
                return true;
        }
        static void ResolveCommand(bool Output)
        {
            if (!Output)
                InvalidCommand();
        }
    }
    public static string[] Tokenize(string Command)
    {
        List<string> tokens = new();
        StringBuilder sb = new();
        bool freeze = false;
        bool justFroze = false;
        foreach (char c in Command)
        {
            if ((!freeze) && (c == ' '))
            {
                justFroze = false;
                if (sb.Length != 0)
                {
                    tokens.Add(sb.ToString());
                    _ = sb.Clear();
                }
                continue;
            }
            if (justFroze && c == '"')
            {
                freeze = false;
                justFroze = false;
                _ = sb.Append('"');
                continue;
            }
            if (c == '"')
            {
                freeze = !freeze;
                justFroze = freeze;
                continue;
            }
            justFroze = false;
            _ = sb.Append(c);
        }
        if (sb.Length != 0)
            tokens.Add(sb.ToString());
        return tokens.ToArray();
    }
    public static bool GetColor(string Input, out Color? Output)
    {
        try
        {
            Output = ColorTranslator.FromHtml(Input);
            return true;
        }
        catch
        {
            Output = null;
            return false;
        }
    }
    public static bool GetInt32(string Input, out int? Output, Size Size)
    {
        bool ParseSubstring(out double? Output)
        {
            if (double.TryParse(Input[0..^1], out double result))
            {
                Output = result;
                return true;
            }
            Output = null;
            return false;
        }
        if (Input.Length != 0)
            switch (Input[^1])
            {
                case 'w':
                    if (ParseSubstring(out double? output0))
                    {
                        Output = int.Clamp((int)(output0! * Size.Width), 0, Size.Width);
                        return true;
                    }
                    break;
                case 'h':
                    if (ParseSubstring(out double? output1))
                    {
                        Output = int.Clamp((int)(output1! * Size.Height), 0, Size.Height);
                        return true;
                    }
                    break;
                default:
                    if (int.TryParse(Input, out int output2))
                    {
                        Output = output2;
                        return true;
                    }
                    break;
            }
        Output = null;
        return false;
    }
    public static bool GetInt32(string Input, out int? Output)
    {
        if (int.TryParse(Input, out int output1))
        {
            Output = output1;
            return true;
        }
        Output = null;
        return false;
    }
    public static void InvalidCommand()
        => WriteLine("Invalid command. See \"help\" for a list of possible commands. Be aware that all commands are case-sensitive.");
    public static readonly Dictionary<string, (string Description, string[] Aliases, string?[][] Syntax)> Documentation = new()
    {
        {
            "Help", (
            "Displays information about commands.",
            new string[] { "help", "hlp", "h"},
            new string?[][]
            {
                Array.Empty<string>(),
                new string?[] {"command"},
            })
        },
        {
            "Exit", (
            "Terminates the program.",
            new string[] { "exit", "ex", "x"},
            new string?[][]
            {
                Array.Empty<string?>(),
            })
        },
        {
            "Clear", (
            "Clears the console.",
            new string[] { "clear", "cl", "c"},
            new string?[][]
            {
                Array.Empty<string?>(),
            })
        },
        {
            "New", (
            "Initializes a new bitmap.",
            new string[] { "new", "nw", "n"},
            new string?[][]
            {
                new string?[] {"width", "height"},
                new string?[] {"width", "height", "color"},
            })
        },
        {
            "Save", (
            "Saves the current image.",
            new string[] { "save", "sv", "s"},
            new string?[][]
            {
                new string?[] {"filename"},
            })
        },
        {
            "Wipe", (
            "Clears the bitmap.",
            new string[] { "wipe", "wp", "w"},
            new string?[][]
            {
                Array.Empty<string?>(),
            })
        },
        {
            "Draw Rectangle", (
            "Draws a rectangle.",
            new string[] { "drawrectangle", "drawrect", "dr"},
            new string?[][]
            {
                new string?[] {"color","lineWidth", "x", "y","width", "height"},
            })
        },
        {
            "Draw Ellipse", (
            "Draws an ellipse.",
            new string[] { "drawellipse", "drawelps", "de"},
            new string?[][]
            {
                new string?[] {"color", "lineWidth", "x", "y","width", "height"},
            })
        },
        {
            "Draw Line", (
            "Draws a line.",
            new string[] { "drawline", "dl"},
            new string?[][]
            {
                new string?[] {"color", "lineWidth", "x0", "y0","x1", "y1"},
            })
        },
        {
            "Draw Polygon", (
            "Draws a polygon.",
            new string[] { "drawpolygon", "drawpoly", "dp" },
            new string?[][]
            {
                new string?[] {"color", "lineWidth", "x0", "y0","x1", "y1", null, "xN", "yN"},
            })
        },
        {
            "Fill", (
            "Fills the bitmap with a given color.",
            new string[] { "fill", "fl", "f"},
            new string?[][]
            {
                new string?[] {"color"},
            })
        },
        {
            "Fill Rectangle", (
            "Fills a rectangle.",
            new string[] { "fillrectangle", "fillrect", "fr"},
            new string?[][]
            {
                new string?[] {"color", "x", "y","width", "height"},
            })
        },
        {
            "Fill Ellipse", (
            "Fills an ellipse.",
            new string[] { "fillellipse", "fillelps", "fe"},
            new string?[][]
            {
                new string?[] {"color", "x", "y","width", "height"},
            })
        },
        {
            "Fill Polygon", (
            "Fills a polygon.",
            new string[] { "fillpolygon", "fillpoly", "fp" },
            new string?[][]
            {
                new string?[] {"color", "x0", "y0","x1", "y1", null, "xN", "yN"},
            })
        },
        {
            "Set Backdrop", (
            "Sets the backdrop of the display.",
            new string[] { "setbackdrop", "backdrop", "setback", "back", "setdrop", "drop", "sbd", "bd", "sb", "b", "sd", "d" },
            new string?[][]
            {
                new string?[] {"color"},
            })
        },
    };
}