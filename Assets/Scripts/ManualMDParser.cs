using System.Collections.Generic;
using System.IO;

public class ManualMDParser {
    const string filePath = @"C:\Users\Jeremy.Katja\Documents\Obsidian\General\Space Plumbing\Manual.md";

    public static string ParseFile() {
        string result = "";
        string currentLine;

        var lines = File.ReadAllLines(filePath);
        for (int i = 0; i < lines.Length; i++) {
            currentLine = lines[i]; // at some point, get rid of this-- we don't *need* it, it just feels nicer while I'm writing
            if (currentLine.StartsWith("# ")) {
                result += $"<style=\"h1\">{currentLine.Remove(0, 2)}</style>\n\n";
            }
            else if (currentLine.StartsWith("## ")) {
                result += $"<style=\"h2\">{currentLine.Remove(0, 3)}</style>\n\n";
            }
            else if (currentLine.StartsWith("### ")) {
                result += $"<style=\"h3\">{currentLine.Remove(0, 4)}</style>\n\n";
            }
            else if (currentLine.ToLower().Trim().StartsWith("> [!warning]")) {
                result += $"<style=\"warn\">{lines[++i].Remove(0, 2)}</style>\n";
            }
            else if (currentLine.ToLower().Trim().StartsWith("> [!info]")) {
                result += $"<style=\"info\">{lines[++i].Remove(0, 2)}</style>\n";
            }
            else if (currentLine == "Status | Pattern") { // just gonna hardcode this one
                result += @"<page>
______________________________

<u>STATE</u> <pos=50%><u>PATTERN</u></pos>

Operational<pos=50%>On steadily</pos>

Incident <pos=50%>Blinks 3 times</pos>
<pos=50%>intermittently</pos>

Stabilized <pos=50%>Blinks 1 time</pos>
<pos=50%>intermittently</pos>

Self-testing <pos=50%>Blinks steadily</pos>

<line-height=50%>______________________________</line-height>" + "\n\n";
                i += 5;
            }
            else if (currentLine.Trim() == "Control | Value") {
                i += 2;
                string[] values = new string[5];
                for (int j = 0; j < 5; j++) {
                    values[j] = lines[i + j].Split(" | ")[1];
                }
                result += "\n" + @$"<page>______________________________

<u>CONTROL</u> <pos=60%><u>VALUE</u></pos>

Phase Modulation <pos=60%>{values[0]}</pos>
Unit

Khurana Matrix <pos=60%>{values[1]}</pos>
Configuration

Proxy Filtration<pos=60%>{values[2]}</pos>
Intensity

Module Stabilizer <pos=60%>{values[3]}</pos>
Amplitude

Catalyst Energy<pos=60%>{values[4]}</pos>
Ceiling
<line-height=50%>______________________________</line-height>" + "\n\n";
                i += 5;
            }
            else if (currentLine.StartsWith('1')) {
                bool foundEnd = false;
                List<string> listItems = new List<string>();
                while (!foundEnd) {
                    if (!lines[i].StartsWith('1')) {
                        foundEnd = true;
                    }
                    else {
                        listItems.Add(lines[i].Remove(0, 3));
                    }
                    i++;
                }
                foreach (var (entry, index) in listItems.WithIndex()) {
                    result += $"\n<pos=8%>{index + 1}.</pos><indent=16%>{entry}</indent>\n";
                }
                result += "\n";
            }
            else if (!currentLine.StartsWith("%%")) {
                result += $"{currentLine}\n";
            }

        }
        return result;
    }
}