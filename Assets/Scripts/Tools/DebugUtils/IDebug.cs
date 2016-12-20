using System;

public interface IDebug
{
	void Log(string format, params object[] args);
	void LogWarning(string format, params object[] args);
	void LogError(string format, params object[] args);
	void LogException(Exception e);
}

public sealed class DebugIgnore : IDebug
{
    public void Log(string format, params object[] args) { }
    public void LogWarning(string format, params object[] args) { }
    public void LogError(string format, params object[] args) { }
    public void LogException(Exception e) { }
}
