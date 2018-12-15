using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayController : MonoBehaviour {

    private string ResolveTextSize(string input, int lineLength)
    {

        // Split string by char " "         
        string[] words = input.Split(" "[0]);

        // Prepare result
        string result = "";

        // Temp line string
        string line = "";

        // for each all words        
        foreach (string s in words)
        {
            // Append current word into line
            string temp = line + " " + s;

            // If line length is bigger than lineLength
            if (temp.Length > lineLength)
            {

                // Append current line into result
                result += line + "\n";
                // Remain word append into new line
                line = s;
            }
            // Append current word into current line
            else
            {
                line = temp;
            }
        }

        // Append last line into result        
        result += line;

        // Remove first " " char
        return result.Substring(1, result.Length - 1);
    }

    //public void FitToWidth(float wantedWidth)
    //{

    //    if (width <= wantedWidth) return;

    //    string oldText = textMesh.text;
    //    textMesh.text = "";

    //    string[] lines = oldText.Split('\n');

    //    foreach (string line in lines)
    //    {
    //        textMesh.text += wrapLine(line, wantedWidth);
    //        textMesh.text += "\n";
    //    }
    //}
    //private string wrapLine(string s, float w)
    //{
    //    // need to check if smaller than maximum character length, really...
    //    if (w == 0 || s.Length <= 0) return s;

    //    char c;
    //    char[] charList = s.ToCharArray();

    //    float charWidth = 0;
    //    float wordWidth = 0;
    //    float currentWidth = 0;

    //    string word = "";
    //    string newText = "";
    //    string oldText = textMesh.text;

    //    for (int i = 0; i < charList.Length; i++)
    //    {
    //        c = charList[i];

    //        if (dict.ContainsKey(c))
    //        {
    //            charWidth = (float)dict[c];
    //        }
    //        else
    //        {
    //            textMesh.text = "" + c;
    //            charWidth = renderer.bounds.size.x;
    //            dict.Add(c, charWidth);
    //            //here check if max char length
    //        }

    //        if (c == ' ' || i == charList.Length - 1)
    //        {
    //            if (c != ' ')
    //            {
    //                word += c.ToString();
    //                wordWidth += charWidth;
    //            }

    //            if (currentWidth + wordWidth < w)
    //            {
    //                currentWidth += wordWidth;
    //                newText += word;
    //            }
    //            else
    //            {
    //                currentWidth = wordWidth;
    //                newText += word.Replace(" ", "\n");
    //            }

    //            word = "";
    //            wordWidth = 0;
    //        }

    //        word += c.ToString();
    //        wordWidth += charWidth;
    //    }

    //    textMesh.text = oldText;
    //    return newText;
    //}
}
