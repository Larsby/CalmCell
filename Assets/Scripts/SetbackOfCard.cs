using UnityEngine;
using System.Collections;

public class SetbackOfCard : MonoBehaviour
{
	public GameObject back;
	private bool setSize = false;
	public GameObject parent;

	public void CombineMeshes ()
	{
		Matrix4x4 transformMatrix = transform.worldToLocalMatrix;
		var meshFilter = GetComponent<MeshFilter> ();
		var backInstance = Instantiate (back);
	
		backInstance.transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - 0.00001f);
	
		var backMeshFilter = backInstance.GetComponent<MeshFilter> ();

		CombineInstance[] combine = new CombineInstance[2];
	
		combine [1].mesh = meshFilter.sharedMesh;

		combine [1].transform = transformMatrix * meshFilter.transform.localToWorldMatrix;

		combine [0].mesh = backMeshFilter.mesh;
		combine [0].transform = transformMatrix * backMeshFilter.transform.localToWorldMatrix;

		Mesh newMesh = new Mesh ();
		transform.GetComponent<MeshFilter> ().mesh = newMesh;

		transform.GetComponent<MeshFilter> ().mesh.CombineMeshes (combine, false, true);


		transform.gameObject.active = true;
		Destroy (backInstance);
	}



	void Awake ()
	{
		CombineMeshes ();
		//GetComponent<GameCard> ().enabled = true;
	}


	

}
