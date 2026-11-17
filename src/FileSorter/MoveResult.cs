using System;

public class MoveResult
{
	public string SourcePath { get; }
	public string FinalPath { get; }
	public bool Success { get; }
	public string ErrorMessage {  get; }
    public bool WasRenamed => Success && Path.GetFileName(SourcePath) != Path.GetFileName(FinalPath);
    public MoveResult(string sourcePath, string finalPath, bool success, string errorMessage = "")
	{
		SourcePath = sourcePath;
		FinalPath = finalPath;
		Success = success;
		ErrorMessage = errorMessage;
	}
}
