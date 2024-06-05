using System;
using System.Text;

public class DialogWindow
{
    private int width;
    private int height;
    private int x;
    private int y;

    public DialogWindow(int width, int height, int x, int y)
    {
        this.width = width;
        this.height = height;
        this.x = x;
        this.y = y;
    }

    public void Draw(string title, string message)
    {
        //CalculatePositionAndSize();

        // Draw the outer border
        DrawBorder();

        // Draw the title
        DrawTitle(title);

        // Draw the message
        DrawMessage(message);
    }

    // private void CalculatePositionAndSize()
    // {
    //     int consoleWindowWidth = Console.WindowWidth;
    //     int consoleWindowHeight = Console.WindowHeight;

    //     width = Math.Min(width, consoleWindowWidth - 4);
    //     height = Math.Min(height, consoleWindowHeight - 4); 

    //     x = (consoleWindowWidth - width) / 2;
    //     y = (consoleWindowHeight - height) / 2;
    // }

    private void DrawBorder()
    {
        Console.SetCursorPosition(x, y);
        Console.Write("┌" + new string('─', width - 2) + "┐"); // Top border

        for (int i = 1; i < height - 1; i++)
        {
            Console.SetCursorPosition(x, y + i);
            Console.Write("│" + new string(' ', width - 2) + "│"); // Side borders
        }

        Console.SetCursorPosition(x, y + height - 1);
        Console.Write("└" + new string('─', width - 2) + "┘"); // Bottom border
    }

    private void DrawTitle(string title)
    {
        int titleX = x + (width - title.Length) / 2;
        Console.SetCursorPosition(titleX, y);
        Console.Write(title);
    }

    private void DrawMessage(string message)
    {
        string[] lines = WrapText(message, width - 4).ToArray();
        int startY = y + (height - lines.Length) / 2;

        for (int i = 0; i < lines.Length; i++)
        {
            Console.SetCursorPosition(x + 2, startY + i);
            Console.Write(lines[i]);
        }
    }

    private IEnumerable<string> WrapText(string text, int maxLineLength)
    {
        StringBuilder line = new StringBuilder();
        foreach (string word in text.Split(' '))
        {
            if (line.Length + word.Length + 1 > maxLineLength)
            {
                yield return line.ToString();
                line.Clear();
            }
            if (line.Length > 0)
            {
                line.Append(" ");
            }
            line.Append(word);
        }
        if (line.Length > 0)
        {
            yield return line.ToString();
        }
    }
}
