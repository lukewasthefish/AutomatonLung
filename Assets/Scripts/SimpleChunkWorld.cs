using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleChunkWorld : MonoBehaviour {
	public GameObject[] chunks;

	public float distanceBetweenChunks;

	private Vector3 lastMeasuredPlayerPosition;

	private Dictionary<Vector3, bool> positionsOccupied; //true = occupied; false = open to being occupied

	private void Start(){
		positionsOccupied = new Dictionary<Vector3, bool>();

		for(int i = 0; i < chunks.Length; i++){
			positionsOccupied.Add(chunks[i].transform.position, true);
		}

		lastMeasuredPlayerPosition = GameManager.Instance.thirdPersonCharacterMovement.transform.position;
	}

	bool ranFirstUpdate = false;
	private void Update(){
		if(!ranFirstUpdate){
			ranFirstUpdate = true;
			return;
		}

		if(Vector3.Distance(lastMeasuredPlayerPosition, GameManager.Instance.thirdPersonCharacterMovement.transform.position) > distanceBetweenChunks / 4f){
			Regenerate();
			//RecenterWorld();
		}
	}

	private void Regenerate(){
		List<Vector3> potentialChunkPositions = new List<Vector3>();

		for(int x = 0; x < Mathf.Sqrt(chunks.Length); x++){
			for(int z = 0; z < Mathf.Sqrt(chunks.Length); z++){
				Vector3 player = GameManager.Instance.thirdPersonCharacterMovement.transform.position;
				Vector3 potentialChunkPosition = new Vector3((player.x - distanceBetweenChunks - distanceBetweenChunks) + (x*distanceBetweenChunks), 0f, (player.z - distanceBetweenChunks - distanceBetweenChunks) + (z*distanceBetweenChunks));
				potentialChunkPosition = new Vector3(RoundToNearestGrid(potentialChunkPosition.x), potentialChunkPosition.y, RoundToNearestGrid(potentialChunkPosition.z));
				potentialChunkPositions.Add(potentialChunkPosition);
			}
		}

		List<GameObject> dispossesedChunks = new List<GameObject>();

		for(int i = 0; i < chunks.Length; i++){
			if(!potentialChunkPositions.Contains(chunks[i].transform.position)){
				RemoveChunkFromPosition(chunks[i]);
				dispossesedChunks.Add(chunks[i]);
			}
		}

		for(int i = 0; i < potentialChunkPositions.Count; i++){
			if(!positionsOccupied.ContainsKey(potentialChunkPositions[i])){
				if(dispossesedChunks.Count > 0 && dispossesedChunks[0]){
				GameObject currentChunk = dispossesedChunks[0];
				AssignChunkToPosition(currentChunk, potentialChunkPositions[i]);
				dispossesedChunks.Remove(currentChunk);
				}
			}
		}
	}

	private void AssignChunkToPosition(GameObject chunk, Vector3 destinationPosition){
		chunk.transform.position = destinationPosition;
		positionsOccupied.Add(destinationPosition, true);

		chunk.transform.localScale = new Vector3(chunk.transform.localScale.x, chunk.transform.localScale.y, Random.Range(1f, 12000f));
		chunk.transform.Rotate(0f, 0f, 90f);
	}

	private void RemoveChunkFromPosition(GameObject chunk){
		positionsOccupied.Remove(chunk.transform.position);
	}

	private float RoundToNearestGrid(float pos)
	{
		float xDiff = pos % distanceBetweenChunks;
		pos -= xDiff;
		if (xDiff > (distanceBetweenChunks / 2))
		{
			pos += distanceBetweenChunks;
		}
		return pos;
	}

	private void RecenterWorld(){
		GameManager.Instance.thirdPersonCharacterMovement.transform.position = new Vector3(0f, GameManager.Instance.thirdPersonCharacterMovement.transform.position.y, 0f);
		lastMeasuredPlayerPosition = GameManager.Instance.thirdPersonCharacterMovement.transform.position;
	}
}
