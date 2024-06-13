using System.Runtime.InteropServices;
using static GraphicsConsole.Interpretation;
using static System.Console;
namespace GraphicsConsole;
internal static partial class Program
{
    public static Color BackDrop = Color.Black;
    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool AllocConsole();
    [STAThread]
    static void Main()
    {
        const int displayWidth = 600;
        _ = AllocConsole();
        Title = "Graphics Console";
        WriteLine("Initializing...");
        ApplicationConfiguration.Initialize();
        Form form = new()
        {
            FormBorderStyle = FormBorderStyle.FixedDialog,
            BackColor = Color.Black,
            Size = new Size(displayWidth, displayWidth),
            MinimumSize = new Size(displayWidth, displayWidth),
            MaximumSize = new Size(displayWidth, displayWidth),
            Text = "Display",
            Icon = Resources.Icon_256,
        };
        form.FormClosing += (object? Sender, FormClosingEventArgs E) => E.Cancel = true;
        Thread thread0 = new(() => Application.Run(form));
        thread0.Start();
        Bitmap? bitmap = null;
        Graphics? graphics = null;
        Size? size = null;
        form.Paint += PaintFunc;
        void PaintFunc(object? Sender, PaintEventArgs E)
        {
            E.Graphics.Clear(BackDrop);
            if (bitmap is not null)
            {
                double ratio = int.Max(bitmap.Width, bitmap.Height) / (double)displayWidth;
                int width = (int)(bitmap.Width / ratio);
                int height = (int)(bitmap.Height / ratio);
                E.Graphics.DrawImage(bitmap, 0, 0, width, height);
            }
        }
        Thread.Sleep(750);
        WriteLine("Initialized.");
        Thread.Sleep(750);
        Clear();
        WriteLine("Welcome to Graphics Console!");
        WriteLine("Type \"help\" for a list of possible commands! Be aware that all commands are case-sensitive.");
        while (Interpret(ref bitmap, ref graphics, ref size, ReadLine() ?? ""))
            form.Invoke(form.Refresh);
        form.Invoke(form.Close);
        bitmap?.Dispose();
    }
}