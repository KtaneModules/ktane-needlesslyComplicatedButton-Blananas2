using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class needlesslyComplicatedButtonScript : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;

    public TextMesh Label;
    public TextMesh[] Indicators;
    public Material[] ButtonMat;
    public Color[] LabelColor;
    public GameObject ButtonObject;
    public KMSelectable Button;

    private List<string> LabelList = new List<string>{ " ", "Abort", "Blank", "Blue", "Button", "Cyan", "Detonate", "Green", "Hold", "Literally\nBlank", "Magenta", "No Label", "Nothing", "Press", "Red", "Yellow" };
    int LabelIx = -1;
    string LabelText = "";
    int IndA = -1;
    int IndB = -1;
    int IndC = -1;
    int TableRule = -1;
    private List<int> TableRow = new List<int>{  };
    int firstInd = -1;
    int secondInd = -1;
    int remainingInd = -1;
    int ButtonColor = -1;
    int flowchartOutput = -1000;
    int colorWheelOutput = -1;
    int initialOutput = -1000;
    int finalOutput = -1000;
    int diagramBix = 0;
    string Venn = "AHIFB?GJDABIJCF?";
    char diagramBchar = ' ';
    private List<string> ColorNames = new List<string>{ "Black", "Blue", "Green", "Cyan", "Red", "Magenta", "Yellow", "White" };
    float HELD = -1;
    float RELEASED = -1;
    int rounds = 10;
    int RNG = -1;
    bool Gucci = false;

    //Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    void Awake () {
        moduleId = moduleIdCounter++;

        Button.OnInteract += delegate () { buttonPress(); return false; };
        Button.OnInteractEnded += delegate () { buttonRelease(); };
    }

    void Start () {
        //Part A: The Button
        LabelIx = UnityEngine.Random.Range(0, 16);
        LabelText = LabelList[LabelIx];
        Label.text = LabelText;
        Debug.LogFormat("[Needlessly Complicated Button #{0}] Label of the button: \"{1}\"", moduleId, LabelText.Replace("\n", " "));

        IndA = UnityEngine.Random.Range(0, 1000);
        IndB = UnityEngine.Random.Range(0, 1000);
        IndC = UnityEngine.Random.Range(0, 1000);
        if (IndA < 10) { Indicators[0].text = "00" + IndA.ToString(); } else if (IndA < 100) { Indicators[0].text = "0" + IndA.ToString(); } else { Indicators[0].text = IndA.ToString(); }
        if (IndB < 10) { Indicators[1].text = "00" + IndB.ToString(); } else if (IndB < 100) { Indicators[1].text = "0" + IndB.ToString(); } else { Indicators[1].text = IndB.ToString(); }
        if (IndC < 10) { Indicators[2].text = "00" + IndC.ToString(); } else if (IndC < 100) { Indicators[2].text = "0" + IndC.ToString(); } else { Indicators[2].text = IndC.ToString(); }
        Debug.LogFormat("[Needlessly Complicated Button #{0}] The three indicators: {1}, {2}, {3}", moduleId, IndA, IndB, IndC);

        if (Bomb.GetBatteryCount() > Bomb.GetPortCount()) {
            TableRule = 0;
        } else if (Bomb.GetIndicators().Any(x => new[] { LabelText.ToUpper()[0], LabelText.ToUpper()[LabelText.Length - 1] }.Any(y => x.Contains(y)))) {
            TableRule = 1;
        } else if (Bomb.GetSerialNumberLetters().Any( x => LabelText.Contains(x))) {
            TableRule = 2;
        } else {
            TableRule = 3;
        }

        switch (LabelIx) {
            case 0: TableRow.Add(23); TableRow.Add(23); TableRow.Add(13); TableRow.Add(12); break;
            case 1: TableRow.Add(13); TableRow.Add(23); TableRow.Add(13); TableRow.Add(12); break;
            case 2: TableRow.Add(12); TableRow.Add(23); TableRow.Add(13); TableRow.Add(23); break;
            case 3: TableRow.Add(23); TableRow.Add(12); TableRow.Add(12); TableRow.Add(13); break;
            case 4: TableRow.Add(13); TableRow.Add(23); TableRow.Add(13); TableRow.Add(23); break;
            case 5: TableRow.Add(23); TableRow.Add(13); TableRow.Add(13); TableRow.Add(12); break;
            case 6: TableRow.Add(13); TableRow.Add(12); TableRow.Add(23); TableRow.Add(13); break;
            case 7: TableRow.Add(12); TableRow.Add(13); TableRow.Add(13); TableRow.Add(23); break;
            case 8: TableRow.Add(12); TableRow.Add(13); TableRow.Add(12); TableRow.Add(23); break;
            case 9: TableRow.Add(12); TableRow.Add(13); TableRow.Add(13); TableRow.Add(23); break;
            case 10: TableRow.Add(13); TableRow.Add(23); TableRow.Add(13); TableRow.Add(13); break;
            case 11: TableRow.Add(13); TableRow.Add(13); TableRow.Add(23); TableRow.Add(12); break;
            case 12: TableRow.Add(13); TableRow.Add(12); TableRow.Add(23); TableRow.Add(13); break;
            case 13: TableRow.Add(23); TableRow.Add(23); TableRow.Add(13); TableRow.Add(13); break;
            case 14: TableRow.Add(23); TableRow.Add(23); TableRow.Add(13); TableRow.Add(13); break;
            case 15: TableRow.Add(13); TableRow.Add(12); TableRow.Add(23); TableRow.Add(23); break;
            default: Debug.Log("FUCK!"); break;
        }

        if (TableRow[TableRule] == 12) {
            firstInd = IndA; secondInd = IndB; remainingInd = IndC;
        } else if (TableRow[TableRule] == 13) {
            firstInd = IndA; secondInd = IndC; remainingInd = IndB;
        } else if (TableRow[TableRule] == 23) {
            firstInd = IndB; secondInd = IndC; remainingInd = IndA;
        } else {
            Debug.Log("FUCK!");
        }
        Debug.LogFormat("[Needlessly Complicated Button #{0}] Ind 1: {1} Ind 2: {2} Remaining Ind: {3}", moduleId, firstInd, secondInd, remainingInd);

        ButtonColor = UnityEngine.Random.Range(0, 8);
        ButtonObject.GetComponent<MeshRenderer>().material = ButtonMat[ButtonColor];
        Label.color = LabelColor[ButtonColor];
        Debug.LogFormat("[Needlessly Complicated Button #{0}] Button color: {1}", moduleId, ColorNames[ButtonColor]);

        //Part B: The indicators
        if ("13579".Contains(Bomb.GetSerialNumber().Last())) {
            //odd
            if (firstInd % 2 == 0) {
                //yes
                if (secondInd % 2 == 1) {
                    //yes AA > D
                    if (Bomb.GetBatteryCount(Battery.AA) > Bomb.GetBatteryCount(Battery.D)) {
                        flowchartOutput = -1;
                    } else {
                        flowchartOutput = 3;
                    }
                } else {
                    //no *FRK or *CLR
                    if (Bomb.IsIndicatorOn(Indicator.FRK) || Bomb.IsIndicatorOn(Indicator.CLR)) {
                        flowchartOutput = 5;
                    } else {
                        flowchartOutput = -4;
                    }
                }
            } else {
                //no
                if (secondInd % 2 == 0) {
                    //yes RJ or ser
                    if (Bomb.IsPortPresent(Port.RJ45) || Bomb.IsPortPresent(Port.Serial)) {
                        flowchartOutput = 4;
                    } else {
                        flowchartOutput = -2;
                    }
                } else {
                    //no par
                    if (Bomb.IsPortPresent(Port.Parallel)) {
                        flowchartOutput = 3;
                    } else {
                        flowchartOutput = 1;
                    }
                }
            }
        } else {
            //even
            if (secondInd % 2 == 1) {
                //yes
                if (firstInd % 2 == 1) {
                    //yes RJ or ser
                    if (Bomb.IsPortPresent(Port.RJ45) || Bomb.IsPortPresent(Port.Serial)) {
                        flowchartOutput = 4;
                    } else {
                        flowchartOutput = -2;
                    }
                } else {
                    //no *FRK or *CLR
                    if (Bomb.IsIndicatorOn(Indicator.FRK) || Bomb.IsIndicatorOn(Indicator.CLR)) {
                        flowchartOutput = 5;
                    } else {
                        flowchartOutput = -4;
                    }
                }
            } else {
                //no
                if (firstInd % 2 == 0) {
                    //yes par
                    if (Bomb.IsPortPresent(Port.Parallel)) {
                        flowchartOutput = 3;
                    } else {
                        flowchartOutput = 1;
                    }
                } else {
                    //no AA > D
                    if (Bomb.GetBatteryCount(Battery.AA) > Bomb.GetBatteryCount(Battery.D)) {
                        flowchartOutput = -1;
                    } else {
                        flowchartOutput = 3;
                    }
                }
            }
        }
        Debug.LogFormat("[Needlessly Complicated Button #{0}] Flowchart output: {1}", moduleId, flowchartOutput);

        switch (ButtonColor) {
            case 0: colorWheelOutput = 1; break; //000 K
            case 1: colorWheelOutput = 3; break; //001 B
            case 2: colorWheelOutput = 4; break; //010 G
            case 3: colorWheelOutput = 6; break; //011 C
            case 4: colorWheelOutput = 8; break; //100 R
            case 5: colorWheelOutput = 0; break; //101 M
            case 6: colorWheelOutput = 7; break; //110 Y
            case 7: colorWheelOutput = 9; break; //111 W
            default: Debug.Log("FUCK!"); break;
        }
        Debug.LogFormat("[Needlessly Complicated Button #{0}] Color wheel output: {1}", moduleId, colorWheelOutput);

        initialOutput = flowchartOutput + colorWheelOutput;
        Debug.LogFormat("[Needlessly Complicated Button #{0}] Initial output: {1}", moduleId, initialOutput);
        finalOutput = initialOutput + (Bomb.GetModuleNames().Count() - Bomb.GetSolvableModuleNames().Count()) * 4;
        if (((firstInd - 1) % 9) + 1 == initialOutput && ((secondInd - 1) % 9) + 1 == initialOutput) {
            //GO FUCK YOURSELF
        } else if (((firstInd - 1) % 9) + 1 == initialOutput) {
            finalOutput = finalOutput + (((secondInd - 1) % 9) + 1);
        } else if (((secondInd - 1) % 9) + 1 == initialOutput) {
            finalOutput = finalOutput + (((firstInd - 1) % 9) + 1);
        }
        if ((IndA % 10 == colorWheelOutput || IndB % 10 == colorWheelOutput) || IndC % 10 == colorWheelOutput) {
            finalOutput = finalOutput + (colorWheelOutput * 2);
        }
        finalOutput = (finalOutput + 100) % 10;
        Debug.LogFormat("[Needlessly Complicated Button #{0}] Final output: {1}", moduleId, finalOutput);

        if ((ButtonColor == 1 || ButtonColor == 2) || ButtonColor == 4) { //SOLID LINE CONDITION
            diagramBix += 1;
        }
        if (Bomb.GetSerialNumberNumbers().Any(n => n % 2 == 0)) { //DOTTED
            diagramBix += 2;
        }
        if (IsPrime(remainingInd)) { //DASHED
            diagramBix += 4;
        }
        if ((((remainingInd - 1) % 9) + 1) % 2 == 1) { //MIXED
            diagramBix += 8;
        }
        diagramBchar = Venn[diagramBix];
        Debug.LogFormat("[Needlessly Complicated Button #{0}] Venn Diagram letter: {1}", moduleId, diagramBchar);
    }

    void buttonPress () {
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonPress, transform);
        GetComponent<KMSelectable>().AddInteractionPunch();
        if (!moduleSolved) {
            HELD = Bomb.GetTime();
            if (diagramBchar == '?' && (int)Math.Floor(HELD) % 10 == finalOutput) {
                rounds = 10;
                StartCoroutine(UntilSatisfied());
            }
        }
    }

    void buttonRelease () {
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonRelease, transform);
        GetComponent<KMSelectable>().AddInteractionPunch();
        if (!moduleSolved) {
            RELEASED = Bomb.GetTime();
            if ((int)Math.Floor(HELD) % 10 == finalOutput) {
                switch (diagramBchar) {
                    case 'I': if ((int)Math.Round(Math.Abs(HELD - RELEASED)) == 0) { Gucci = true; } else { Gucci = false; }; break;
                    case 'A': if ((int)Math.Round(Math.Abs(HELD - RELEASED)) == 5) { Gucci = true; } else { Gucci = false; }; break;
                    case 'B': if ((int)Math.Round(Math.Abs(HELD - RELEASED)) == 10) { Gucci = true; } else { Gucci = false; }; break;
                    case 'C': if ((int)Math.Round(Math.Abs(HELD - RELEASED)) == 15) { Gucci = true; } else { Gucci = false; }; break;
                    case 'D': if ((int)Math.Round(Math.Abs(HELD - RELEASED)) == 20) { Gucci = true; } else { Gucci = false; }; break;
                    case 'F': if ((int)Math.Floor(RELEASED) % 10 == 1) { Gucci = true; } else { Gucci = false; }; break;
                    case 'G': if ((int)Math.Floor(RELEASED) % 10 == 3) { Gucci = true; } else { Gucci = false; }; break;
                    case 'H': if ((int)Math.Floor(RELEASED) % 10 == 5) { Gucci = true; } else { Gucci = false; }; break;
                    case 'J': if ((int)Math.Floor(RELEASED) % 10 == 8) { Gucci = true; } else { Gucci = false; }; break;
                    default: Debug.Log("FUCK!"); break;
                }
                if (Gucci) {
                    Debug.LogFormat("[Needlessly Complicated Button #{0}] You held and released correctly, module solved.", moduleId);
                    GetComponent<KMBombModule>().HandlePass();
                    moduleSolved = true;
                } else {
                    Debug.LogFormat("[Needlessly Complicated Button #{0}] You released at the wrong time, Strike!", moduleId);
                    GetComponent<KMBombModule>().HandleStrike();
                }
            } else {
                Debug.LogFormat("[Needlessly Complicated Button #{0}] You held at the wrong time, Strike!", moduleId);
                GetComponent<KMBombModule>().HandleStrike();
            }
        }
    }

    IEnumerator UntilSatisfied () {
        yield return new WaitForSeconds(2f);
        RNG = UnityEngine.Random.Range(0, rounds);
        if (RNG == 0) {
            Debug.LogFormat("[Needlessly Complicated Button #{0}] You held until satisfied, module solved.", moduleId);
            GetComponent<KMBombModule>().HandlePass();
            moduleSolved = true;
        } else {
            rounds -= 1;
            StartCoroutine(UntilSatisfied());
        }
    }

    bool IsPrime(int number)
    {
        if (number <= 1) return false;
        if (number == 2) return true;
        if (number % 2 == 0) return false;

        var boundary = (int)Math.Floor(Math.Sqrt(number));

        for (int i = 3; i <= boundary; i+=2)
            if (number % i == 0)
                return false;

        return true;
    }

}
