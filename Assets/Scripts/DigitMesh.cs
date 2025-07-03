using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Please note this script is NOT meant to be used directly on your gameobjects. Use NumberMesh in congruence with it.
/// Fake "UI" element used to display a number to the player. Uses different meshes to represent different numbers (1 through 9.)
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class DigitMesh : MonoBehaviour {

	public int digitToDisplay = 0;

	[Header("The meshes with which to represent number values")]
	public Mesh[] digitMeshes;

	private MeshFilter meshFilter;

	private void Awake(){
		meshFilter = this.GetComponent<MeshFilter>();
	}

	public void UpdateMesh(){
		meshFilter.mesh = digitMeshes[digitToDisplay];
	}
}
