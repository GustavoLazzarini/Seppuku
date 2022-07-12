//Made by Galactspace Studios

using System;
using QFSW.QC;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class DebugManager : MonoBehaviour
{
    private static StreamWriter _writer;
    private static Queue<string> _logs = new Queue<string>();

    private static string FilePath => $"{Application.persistentDataPath}/{Application.productName}_Logs.txt";
    private static string CustomFilePath => $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/{Application.productName}_Logs.txt";

    private static bool HasAnyLog => _logs.Count > 0;
    private static bool WriterOnline => _writer != null && _writer.BaseStream != null;

    private void Awake() 
    {
        StartWriter();
    }

    private void Update() 
    {
        if (HasAnyLog) WriteLog(_logs.Dequeue());
    }

    private static void WriteLog(string message)
    {
        Debug.Log(message);
        if (WriterOnline) _writer.WriteLine(message);
    }

    public static void StartWriter()
    {
        if (WriterOnline)
        {
            Info("[Debug Manager] Writer is already online");
            return;
        }

        _writer = new StreamWriter(FilePath);
        Info("[DebugManager] Initialized");
    }

    public static void CloseWriter()
    {
        if (!WriterOnline)
        {
            Info("[DebugManager] Writer is not online");
            return;
        }

        Info("[Debug Manager] Closing Writer...");
        _writer.Close();
    }

    [Command("Debug.Print")]
    private static string CustomWrite(string message)
    {
        StreamWriter sr = new StreamWriter(CustomFilePath, true);
        sr.WriteLine($"[{DateTime.Now.ToShortTimeString()}] {message}");
        sr.Close();
        return "[DebugManager] Message successfully printed to desktop file!";
    }

    private static void Enqueue(string message) => _logs.Enqueue(message);

    public static void Engine(string message) => Enqueue($"[2D_Engine] {message}");
    public static void Info(string message) => Enqueue($"[INFO] {message}");
    public static void Warning(string message) => Enqueue($"[*WARN*] {message}");
    public static void Error(string message) => Enqueue($"[**ERROR**] {message}");
    public static void Danger(string message) => Enqueue($"[!!DANGER!!] {message}");
}