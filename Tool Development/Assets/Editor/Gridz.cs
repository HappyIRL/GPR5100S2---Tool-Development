using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

public class Gridz : EditorWindow
{
	private int _rows;
	private int _columns;
	private float _tileWidthX;
	private float _tileWidthY;
	private Vector3 _startPosition;

	private AnimBool _usePrefab;
	private List<GameObject> _prefab;

	private ParentType _parentType;
	private string _parentName;
	private string _rowName;


	//make property, changed by interface
	private string _tileName = "Tile";
	private Transform[,] _tileTransforms;

	private void OnEnable()
	{
		InitValues();
	}

	private void InitValues()
	{
		_usePrefab = new AnimBool(false);
		_usePrefab.valueChanged.AddListener(Repaint);

		_rows = 1;
		_columns = 1;
		_tileWidthX = 1;
		_tileWidthY = 1;
		_parentName = "Tiles";
		 _rowName = "Row";
		 _prefab = new List<GameObject>();
	}

	[MenuItem("Tools/Gridz")]
	private static void ShowWindow()
	{
		GetWindow(typeof(Gridz));
	}

	private void OnGUI()
	{
		EditorGUILayout.Space();
		GUILayout.Label("Create Grid", EditorStyles.boldLabel);

		EditorGUILayout.Space();

		_rows = EditorGUILayout.IntField( "Rows",_rows);
		_columns = EditorGUILayout.IntField("Columns",_columns);
		_tileWidthX = EditorGUILayout.FloatField("Tile Width",_tileWidthX);
		_tileWidthY = EditorGUILayout.FloatField("Tile Height", _tileWidthY);
		EditorGUILayout.Space();
		_startPosition = EditorGUILayout.Vector3Field("Start Position",_startPosition);
		EditorGUILayout.Space();
		_parentType = (ParentType)EditorGUILayout.EnumPopup(_parentType);
		EditorGUILayout.Space();

		_usePrefab.target = EditorGUILayout.ToggleLeft("Use Prefabs", _usePrefab.target);

		if (EditorGUILayout.BeginFadeGroup(_usePrefab.faded))
		{
			EditorGUI.indentLevel++;

			int newCount = Mathf.Max(0, EditorGUILayout.DelayedIntField("size", _prefab.Count));
			while (newCount < _prefab.Count)
				_prefab.RemoveAt(_prefab.Count - 1);
			while (newCount > _prefab.Count)
			{
				if (_prefab.Count > 0 && _prefab[0] != null) _prefab.Add(_prefab[0]);
				else _prefab.Add(null);
			}

			for (int i = 0; i < _prefab.Count; i++)
			{
				_prefab[i] = (GameObject)EditorGUILayout.ObjectField(_prefab[i], typeof(GameObject), allowSceneObjects: true);
			}

			EditorGUI.indentLevel--;
		}
		else
		{
			_prefab.Clear();
		}

		EditorGUILayout.EndFadeGroup();

		EditorGUILayout.Space();

		if(GUILayout.Button("Create Tiles"))
		{
			SpawnObjects();
		}
		
		//EditorGUI.BeginDisabledGroup
	}

	private void SpawnObjects()
	{
		GameObject go = null;

		if (!_usePrefab.value)
		{
			go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			_prefab.Add(go);
		}

		if (_usePrefab.value && _prefab.Count == 0)
		{
			Debug.LogError("You must assign a prefab, if 'Use Prefabs' is checked");
			return;
		}

		if(!_usePrefab.value)
			_prefab[0].transform.localScale = new Vector3(_tileWidthX, _tileWidthY, _tileWidthX);

		if (string.IsNullOrEmpty(_parentName)) _parentName = "Tiles";

		if (_rowName == String.Empty) _rowName = "Row";

		Transform parent = new GameObject(_parentName).transform;

		_tileTransforms = new Transform[_rows, _columns];

		for (int i = 0; i < _rows; i++)
		{
			GameObject rowParent = null;

			if (_parentType == ParentType.ParentRow)
			{
				rowParent = new GameObject($"{_rowName} [{i}]");
			}

			for (int j = 0; j < _columns; j++)
			{
				Transform goInstance = GameObject.Instantiate(_prefab[0], new Vector3(_startPosition.x + _tileWidthX * j, _startPosition.y, _startPosition.z - _tileWidthX * i), Quaternion.identity).transform;
				_tileTransforms[i, j] = goInstance.transform;

				if (rowParent == null)
				{
					goInstance.parent = parent;
					goInstance.name = $"{_tileName} [{i}][{j}]";
				}
				else
				{
					_tileTransforms[i, j].transform.parent = rowParent.transform;
					_tileTransforms[i, j].name = $"{_tileName} [{j}]";
				}
			}

			if(rowParent != null) rowParent.transform.parent = parent;
		}

		DestroyImmediate(go, true);
		//FileBrowser.OpenFileBrowser();
	}
}

public enum ParentType
{
	JustParent,
	ParentRow
}
