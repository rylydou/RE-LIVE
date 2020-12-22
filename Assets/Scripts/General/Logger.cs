using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour
{
	// Singleton
	static Logger m_current;
	public static Logger current { get => m_current; }

	// Data
	StreamWriter writer;
	string startTime;
	string logPath;
	Dictionary<LogType, string> logTypeString = new Dictionary<LogType, string>()
	{
		{LogType.Error, "!  "},
		{LogType.Assert, "=> "},
		{LogType.Warning, "*  "},
		{LogType.Log, ">  "},
		{LogType.Exception, "*!*"},
		};

	void Awake()
	{
		if (m_current != null)
			Destroy(gameObject);
		m_current = this;
		DontDestroyOnLoad(gameObject);

		logPath = $"{Application.dataPath}/Logs";

		startTime = System.DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss");

		CreateLog();

		Application.logMessageReceivedThreaded += (condition, stackTrace, type) => OnLogRecive(condition, stackTrace, type);
		Application.logMessageReceived += (condition, stackTrace, type) => OnLogRecive(condition, stackTrace, type);
		Application.lowMemory += () => OnLowMem();
	}

	void OnLogRecive(string condition, string stackTrace, LogType type)
	{
		writer.WriteLine("====================");
		writer.WriteLine(System.DateTime.Now.ToString("HH:mm:ss"));
		writer.WriteLine(type);
		writer.WriteLine(condition);
		writer.WriteLine(stackTrace);
	}

	void OnLowMem()
	{
		Debug.Log("Low Mem!!!");
	}

	void CreateLog()
	{
		if (!Directory.Exists(logPath)) Directory.CreateDirectory(logPath);

		using (var fs = File.Create($"{logPath}/{startTime}.log")) writer = new StreamWriter(fs);

		writer.WriteLine($"{Application.productName} v{Application.version} from {Application.companyName}");
		writer.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
		writer.WriteLine($"Platform: {Application.platform}");
		writer.WriteLine($"Unity Version: {Application.unityVersion}");
		writer.WriteLine($"Sandbox Type: {Application.sandboxType}");
		writer.WriteLine($"Installer Name: {Application.installerName} Mode: {Application.installMode}");
		writer.WriteLine($"Process Priority: {Application.backgroundLoadingPriority}");
		writer.WriteLine($"Internet Reachability: {Application.internetReachability}");
		writer.WriteLine(Application.genuineCheckAvailable ? Application.genuine ? "Not modded" : "Modded" : "Can't check if modded");
	}

	void OnApplicationQuit() => writer.Dispose();

	void OnDisable() => writer.Dispose();

	void OnDestroy() => writer.Dispose();
}