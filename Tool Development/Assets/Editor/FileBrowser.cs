using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class FileBrowser
{
	public static void OpenFileBrowser()
	{
		string path = EditorUtility.OpenFilePanel("File Browser", "", "png");
		if (path.Length != 0)
		{
			var fileContent = File.ReadAllBytes(path);
		}
    }
}
